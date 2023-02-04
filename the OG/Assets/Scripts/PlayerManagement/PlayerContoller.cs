using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
	private bool m_IsSwitched = false;

	public bool Playing = false;

	public int PlayerIndex => GetComponent<PlayerInput>().playerIndex + (m_IsSwitched ? 1 : 0);

	public float LeftStickDeadZone = 0.2f;

	public float RightStickDeadZone = 0.2f;

	private void Awake()
	{
		GameManager.Instance.CheatActions.actions.Switch.performed += SwitchOnPerformed;
		name = $"{name}{PlayerIndex}";
	}

	private void SwitchOnPerformed(InputAction.CallbackContext ctx)
	{
		m_IsSwitched = !m_IsSwitched;
	}

	public void OnPlayerMove(InputAction.CallbackContext ctx)
	{
		if(!Playing) return;
		var value = ctx.ReadValue<Vector2>();
		switch (PlayerIndex)
		{
			case 0:
				if (ctx.performed && value.x * value.x >= LeftStickDeadZone * LeftStickDeadZone)
				{
					float xDir = value.x > 0 ? 1 : -1;
					EventManager.Player1MovePerformed(xDir);
				}
				else if (ctx.canceled)
				{
					EventManager.Player1MoveCanceled();
				}
				break;
			case 1:
				if (ctx.performed)
				{
					EventManager.Player2MovePerformed(value);
				}
				else if (ctx.canceled)
				{
					EventManager.Player2MoveCanceled();
				}
				break;
			default:
				throw new IndexOutOfRangeException($"PlayerIndex:{PlayerIndex} out of range!");
		}
	}
	public void OnPlayerAim(InputAction.CallbackContext ctx)
	{
		if (!Playing) return;
		if (PlayerIndex == 1) return;
		var value = ctx.ReadValue<Vector2>();
		if (ctx.performed && value.sqrMagnitude >= RightStickDeadZone)
		{
			EventManager.PlayerAimPerformed(value);
		}
		else if (ctx.canceled)
		{
			EventManager.PlayerAimCanceled();
		}
	}
	public void OnPlayerAttack(InputAction.CallbackContext ctx)
	{
		if (!Playing) return;
		if (ctx.performed || ctx.canceled) return;
		EventManager.PlayerAttack();
	}
	public void OnPlayerReady(InputAction.CallbackContext ctx)
	{
		if (Playing) return;
		if (ctx.performed || ctx.canceled) return;
		EventManager.PlayerReady(PlayerIndex);
	}

	private void OnDestroy()
	{
		GameManager.Instance.CheatActions.actions.Switch.performed -= SwitchOnPerformed;
	}
}

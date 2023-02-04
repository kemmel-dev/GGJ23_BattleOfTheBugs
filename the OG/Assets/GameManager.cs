using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public PlayerInputManager PlayerIM;
	public GameObject Tree;

	public CheatActions CheatActions;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		PlayerIM = GetComponent<PlayerInputManager>();

		CheatActions = new CheatActions();
		CheatActions.Enable();
		CheatActions.actions.ExtraJoin.performed += ExtraJoinOnPerformed;
	}

	private void ExtraJoinOnPerformed(InputAction.CallbackContext ctx)
	{
		if(PlayerIM.playerCount >= PlayerIM.maxPlayerCount) return;
		GameObject.Instantiate(PlayerIM.playerPrefab, Vector3.zero, Quaternion.identity).
			GetComponent<PlayerInput>().DeactivateInput();
	}

	private void OnDestroy()
	{
		CheatActions.actions.ExtraJoin.performed -= ExtraJoinOnPerformed;
	}
}

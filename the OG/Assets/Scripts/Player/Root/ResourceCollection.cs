using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection : MonoBehaviour
{

	[SerializeField]
    private List<Transform> randomSpawns;

    public ObstacleSpawner ObstacleSpawnerScript;

    private void Awake()
    {
        transform.position = randomSpawns[Random.Range(0, randomSpawns.Count)].position;
        ObstacleSpawnerScript.SpawnRandomObstacles(4);
    }

    public void ChangePosition()
    {
	    transform.position = randomSpawns[Random.Range(0, randomSpawns.Count)].position;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "RootEnd") return;
        EventManager.ResourceCollected();
    }
}

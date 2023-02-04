using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> LeftSpawns;
    public List<GameObject> RightSpawns;
    public List<GameObject> leftTargets;
    public List<GameObject> rightTargets;
    public GameObject EnemyPref;
    public float spawnDelay = 2f;
    private GameObject _activeSpawn;
    
    private Transform _target;
    private int scale;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnStartGame += OnStartGame;
        //StartCoroutine(SpawnEnemy());
    }

    private void OnStartGame()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            int rdmSide = Random.Range(0, 2);
            if(rdmSide == 1)
            {
                int rdmSpawn = Random.Range(0, RightSpawns.Count);
                _activeSpawn = RightSpawns[rdmSpawn];
                scale = -1;
                int rdmTarget = Random.Range(0, rightTargets.Count);
                _target = rightTargets[rdmTarget].transform;
            }else
            {
                int rdmSpawn = Random.Range(0, LeftSpawns.Count);
                _activeSpawn = LeftSpawns[rdmSpawn];
                scale = 1;
                int rdmTarget = Random.Range(0, leftTargets.Count);
                _target = leftTargets[rdmTarget].transform;
            }

 
            GameObject enemy = Instantiate(EnemyPref, _activeSpawn.transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyBehaviour>().target = _target;
            enemy.GetComponent<EnemyBehaviour>().scale = scale;
            yield return new WaitForSeconds(spawnDelay);
        }
        
    }

    private void OnDestroy()
    {
        EventManager.OnStartGame -= OnStartGame;

    }
}

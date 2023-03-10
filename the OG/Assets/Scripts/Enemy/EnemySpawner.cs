using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> LeftSpawns;
    public List<GameObject> RightSpawns;
    public List<GameObject> leftTargets;
    public List<GameObject> rightTargets;
    public GameObject EnemyPref;
    public float spawnDelay = 2f;
    private GameObject _activeSpawn;

    private AudioSource audioSource;

    private Transform _target;
    private int scale;

    private Transform _prevSpawn;

    // Start is called before the first frame update
    void Start()
    {

        EventManager.OnStartGame += OnStartGame;
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(SpawnEnemy());
    }

    public void playChapSound()
    {
        if (audioSource.isPlaying)
            return;
        audioSource.Play();
    }

    private void OnStartGame()
    {   //TODO BUILD INDEX FIX PLS :>
        if (SceneManager.GetActiveScene().name != "Level1 Tut")
        {
            StartCoroutine(SpawnEnemy());
        }
        
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {

            GetRandoms();
 
            GameObject enemy = Instantiate(EnemyPref, _activeSpawn.transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyBehaviour>().target = _target;
            enemy.GetComponent<EnemyBehaviour>().scale = scale;
            yield return new WaitForSeconds(spawnDelay);
        }
        
    }

    private void GetRandoms()
    {
        int rdmSide = Random.Range(0, 2);
        if (rdmSide == 1)
        {
            int rdmSpawn = Random.Range(0, RightSpawns.Count);
            _activeSpawn = RightSpawns[rdmSpawn];
            scale = -1;
            int rdmTarget = Random.Range(0, rightTargets.Count);
            _target = rightTargets[rdmTarget].transform;
        }
        else
        {
            int rdmSpawn = Random.Range(0, LeftSpawns.Count);
            _activeSpawn = LeftSpawns[rdmSpawn];
            scale = 1;
            int rdmTarget = Random.Range(0, leftTargets.Count);
            _target = leftTargets[rdmTarget].transform;
        }
        if(_prevSpawn == _target)
        {
            GetRandoms();
        }
        else
        {
            _prevSpawn = _target;
            return;
        }

    }

    public void SpawnTutorialEnemy()
    {
        StartCoroutine(tutorialspawnEnemy());
        
    }

    public void TutorialSetNormalSpawn()
    {
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator tutorialspawnEnemy()
    {
        
        yield return new WaitForSeconds(5f);
 
        GameObject enemy = Instantiate(EnemyPref, LeftSpawns[0].transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyBehaviour>().target = leftTargets[0].transform;
        enemy.GetComponent<EnemyBehaviour>().scale = 1;
    }

    private void OnDestroy()
    {
        EventManager.OnStartGame -= OnStartGame;

    }
}

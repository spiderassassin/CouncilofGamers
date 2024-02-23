using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    //all the spawn points from where the enemies will spawn
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    public Transform playerTransform;
    public GruntPlayer gruntPlayer;
    public GruntGoal gruntgoal;
    public Tank tank;
 
    public bool isSpawning;

    public List<Enemy> livingEnemies; // TODO: remove from this as enemies die.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void OnEnable()
    {
        livingEnemies = new List<Enemy>();
    }

    private void Update()
    {
        if((GameManager.Instance.wavemode == true) && (isSpawning == false) ) {
            if(livingEnemies.Count == 0)
            {
                GameManager.Instance.wavemode = false;
                print("Wave Over");
                GameManager.Instance.gameStage++;
            }
        }
    }

    





    public void StartWave(Wave wave) {
        GameManager.Instance.wavemode = true;
        if (isSpawning == false)
        {
            StartCoroutine(Spawn(wave));
        }
        
    }

    

    IEnumerator Spawn(Wave waveChunk)
    {
        isSpawning = true;

        for (int i = 0; i < waveChunk.enemies.Count; i++)
        {
            yield return new WaitForSeconds(waveChunk.enemies[i].spawndelay);
            Enemy e = null;
            switch (waveChunk.enemies[i].enemyType)
            {
                case Wave.EnemyType.None:
                    break;
                case Wave.EnemyType.GruntGoal:
                    e = gruntgoal;
                    break;
                case Wave.EnemyType.GruntPlayer:
                    e = gruntPlayer;
                    break;
                case Wave.EnemyType.Tank:
                    e = tank;
                    break;
                    
            }
            GameObject enemy1 = Instantiate(e.gameObject);
            livingEnemies.Add(enemy1.GetComponent<Enemy>());

            Transform spawnPoint = null;
            switch (waveChunk.enemies[i].spawnPoint)
            {
                case Wave.SpawnPoint.SpawnPoint1:
                    spawnPoint = spawnPoint1;
                    break;
                case Wave.SpawnPoint.SpawnPoint2:
                    spawnPoint = spawnPoint2;
                    break;
                case Wave.SpawnPoint.SpawnPoint3:
                    spawnPoint = spawnPoint3;
                    break;
                case Wave.SpawnPoint.SpawnPoint4:
                    spawnPoint = spawnPoint4;
                    break;

            }

            enemy1.transform.position = spawnPoint.position;
            enemy1.GetComponent<Enemy>().player = playerTransform;



        }
        isSpawning = false;
    }
}


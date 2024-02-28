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
    public Transform goalPoint;
    public Transform playerTransform;
    public GruntPlayer gruntPlayer;
    public GruntGoal gruntgoal;
    public Tank tank;

    public bool wavemode = false;//check wheather the game is in wave mode or downtime mode.
    public Wave wave1;
    public Wave wave2;
    public Wave wave3;

    public bool isSpawning;
    public List<Enemy> livingEnemies; // TODO: remove from this as enemies die.

    public int TotalLivingEnemies => livingEnemies.Count;

    public GameObject paroleGuardSprite;
    public Dialogue preWave1Dialogue;

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

    private void startPreWave1Dialogue()
    {
        paroleGuardSprite.SetActive(true);
        FindObjectOfType<DialogueManager>().StartDialogue(preWave1Dialogue);
    }

    private void Update()
    {
        if (InputManager.Instance.startwave)
        {
            StartWave(wave1);
        }

        if ((wavemode == true) && (isSpawning == false) ) {
            if(livingEnemies.Count == 0)
            {
                wavemode = false;
                print("Wave Over");
                
                GameManager.Instance.gameStage++;
                if (GameManager.Instance.gameStage == GameManager.GameStage.PreWave1 )
                {
                    startPreWave1Dialogue();
                }
                //Debug.Log(GameManager.Instance.gameStage);
            }
        }
    }

    public void StartWave(Wave wave) {
        wavemode = true;
        if (isSpawning == false)
        {
            isSpawning = true;
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
            enemy1.GetComponent<Enemy>().goal = goalPoint;


        }
        isSpawning = false;
    }
}


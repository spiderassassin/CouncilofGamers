using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GruntGoal gruntStayWithTank;
    public Tank tank;
    public Explosive explosive;

    public bool wavemode = false;//check wheather the game is in wave mode or downtime mode.
    public Wave wave1;
    public Wave wave2;
    public Wave wave3;

    public bool isSpawning;
    public List<Enemy> livingEnemies; // TODO: remove from this as enemies die.

    public int TotalLivingEnemies => livingEnemies.Count;

    public GameObject paroleGuardSprite;
    public Dialogue downtime1Dialogue;
    public Dialogue downtime2Dialogue;
    public Dialogue downtime3Dialogue;

    public AudioClip enemySpawn;
    public AudioClip TankSpawn;
    public AudioClip paroleHello;

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

    private void startDowntimeDialogue()
    {
        paroleGuardSprite.SetActive(true);
        // Start the downtime dialogue corresponding to the current GameStage.
        Dialogue downtimeDialogue = null;
        switch (GameManager.Instance.gameStage)
        {
            case GameManager.GameStage.Downtime1:
                downtimeDialogue = downtime1Dialogue;
                break;
            case GameManager.GameStage.Downtime2:
                downtimeDialogue = downtime2Dialogue;
                break;
            case GameManager.GameStage.Downtime3:
                downtimeDialogue = downtime3Dialogue;
                break;
        }
        FindObjectOfType<DialogueManager>().StartDialogue(downtimeDialogue);
        //SoundManager.Instance.PlaySoundloop(paroleHello, paroleGuardSprite.transform);
    }

    private void Update()
    {
        if (InputManager.Instance.startwave)
        {
            // Start the wave corresponding to the current GameStage.
            switch (GameManager.Instance.gameStage)
            {
                case GameManager.GameStage.TutorialWave:
                    // For now, start wave 1, but this should be changed when the tutorial is implemented.
                    StartWave(wave1);
                    break;
                case GameManager.GameStage.Downtime1: // ? added to work
                    StartWave(wave1);
                    break;
                case GameManager.GameStage.Wave1:
                    StartWave(wave1);
                    break;
                case GameManager.GameStage.Wave2:
                    StartWave(wave2);
                    break;
                case GameManager.GameStage.Wave3:
                    StartWave(wave3);
                    break;
            }
        }

        if ((wavemode == true) && (isSpawning == false) ) {
            if(livingEnemies.Count == 0)
            {
                wavemode = false;
                print("Wave Over");
                SoundManager.Instance.MusicStop();

                GameManager.Instance.gameStage++;
                if (GameManager.Instance.gameStage == GameManager.GameStage.Ending) {
                    // Go to the ending scene. Make sure it is positioned after the current one in the build settings!
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                } else {
                    startDowntimeDialogue();
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
            float t = 0;
            while( true)
            {
                if (t >= waveChunk.enemies[i].spawndelay) break;
                if (waveChunk.enemies[i].waitUntilPreviousDead && t > .1f && TotalLivingEnemies == 0) break;
                    yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }

            Enemy e = null;
            for(int j = 0; j < waveChunk.enemies[i].count; ++j)
            {
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
                    case Wave.EnemyType.GruntGoalWithTank:
                        e = gruntStayWithTank;
                        break;
                    case Wave.EnemyType.Explosive:
                        e = explosive;
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
                //AudioClip clip = enemySpawn;
                if(waveChunk.enemies[i].enemyType == Wave.EnemyType.Tank)
                {
                    //clip = TankSpawn;
                }

                enemy1.SetActive(false);
                enemy1.transform.position = spawnPoint.position;
                enemy1.GetComponent<Enemy>().player = playerTransform;
                enemy1.GetComponent<Enemy>().goal = goalPoint;
                enemy1.SetActive(true);
                if((waveChunk.enemies[i].spawndelay == 0 && i>0) == false)
                {
                    //SoundManager.Instance.PlaySoundOnce(clip, spawnPoint);
                    SoundManager.Instance.PlayOneShot(FMODEvents.Instance.enemySpawn, spawnPoint.position);
                }

                
            }
            


        }
        isSpawning = false;
    }
}


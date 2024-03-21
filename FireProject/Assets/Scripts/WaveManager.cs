using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    public Wave tutorialFireballWave;
    public Wave tutorialPunchWave;


    public Wave wave1;
    public Wave wave2;
    public Wave wave3;

    public bool isSpawning;
    public List<Enemy> livingEnemies; // TODO: remove from this as enemies die.

    public int TotalLivingEnemies => livingEnemies.Count;

    public GameObject paroleGuardSprite;
    public GameObject baseUI;


    //public GameObject tutorialManager;

    public Dialogue tutorialIntroDialogue;
    public Dialogue tutorialPlayerSeesExitDialogue;
    public Dialogue tutorialTeachFireballDialogue;
    public Dialogue tutorialTeachPunchDialogue;
    public Dialogue tutorialPunchSkullsDialogue;
    public Dialogue tutorialGPWaveDialogue;
    public Dialogue tutorialGGWaveDialogue;
    public Dialogue tutorialEndDialogue;

    public enum TutorialStage { IntroDialogue, PlayerSeesExit, TeachFireball, TeachPunch, PunchSkulls, GPWave, GGWave, End };
    public TutorialStage tutorialStage;

    public bool tutorialExitSeen; //make this into trigger?
    public bool tutorialExitDialogueGiven;
    public bool tutorialFireballExplained;
    public bool tutorialFireballEnemyReleasedStart;  //becuase this is on a timer, can have a start and end
    public bool tutorialFireballEnemyReleasedEnd;
    public bool tutorialFireballKill;
    public bool tutorialPunchExplained;

    public bool tutorialIntroDialogueSeen;

    

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
            Debug.Log("Game stage on awake: " + GameManager.Instance.gameStage);
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

    private void startTutorialDialogue()
    {
        Dialogue tutorialDialogue = null;
        switch (tutorialStage)
        {
            case TutorialStage.IntroDialogue:
                tutorialDialogue = tutorialIntroDialogue;
                break;
            case TutorialStage.PlayerSeesExit:
                tutorialDialogue = tutorialPlayerSeesExitDialogue;
                break;
            case TutorialStage.TeachFireball:
                tutorialDialogue = tutorialTeachFireballDialogue;
                break;
            case TutorialStage.TeachPunch:
                tutorialDialogue = tutorialTeachPunchDialogue;
                break;
            case TutorialStage.PunchSkulls:

                break;
            case TutorialStage.GPWave:
                break;
            case TutorialStage.GGWave:
                break;
            case TutorialStage.End:
                break;
            default:
                break;

        }

        FindObjectOfType<DialogueManager>().StartDialogue(tutorialDialogue);
    }


    private void Update()
    {

        if (GameManager.Instance.tutorialGameStages.Contains(GameManager.Instance.gameStage))
        {
            Tutorial();
            
        }
        if (InputManager.Instance.startwave)
        {
            // Start the wave corresponding to the current GameStage.
            switch (GameManager.Instance.gameStage)
            {
                case GameManager.GameStage.Downtime1: // ? added to work
                    StartWave(wave1);
                    print("wave one");
                    break;
                case GameManager.GameStage.Downtime2: // ? added to work
                    StartWave(wave2);
                    print("wave 2");
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

    IEnumerator Spawn(Wave wave)
    {
        Debug.Log("spawning");
        isSpawning = true;

        for (int i = 0; i < wave.chunks.Count; i++)
        {
            if(wave.chunks[i].spawnDelay>0) yield return new WaitForSeconds(wave.chunks[i].spawnDelay);
            float t = 0; // time in chunk
            while (true)
            {
                if (wave.chunks[i].maxWait != -1 &&
                    t >= wave.chunks[i].maxWait) break;

                if (wave.chunks[i].waitUntilPreviousDead && 
                    (t > .1f && TotalLivingEnemies == 0)) break;

                if (!wave.chunks[i].waitUntilPreviousDead) break;

                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }

            Enemy e = null;
            for(int j = 0; j < wave.chunks[i].count; ++j)
            {
                switch (wave.chunks[i].enemyType)
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
                switch (wave.chunks[i].spawnPoint)
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
                if(wave.chunks[i].enemyType == Wave.EnemyType.Tank)
                {
                    //clip = TankSpawn;
                }

                enemy1.SetActive(false);
                enemy1.transform.position = spawnPoint.position;
                enemy1.GetComponent<Enemy>().player = playerTransform;
                enemy1.GetComponent<Enemy>().goal = goalPoint;
                enemy1.SetActive(true);
                if((wave.chunks[i].maxWait == 0 && i>0) == false)
                {
                    //SoundManager.Instance.PlaySoundOnce(clip, spawnPoint);
                    SoundManager.Instance.PlayOneShot(FMODEvents.Instance.enemySpawn, spawnPoint.position);
                }

                
            }
            


        }
        isSpawning = false;
        if (!tutorialFireballEnemyReleasedEnd)
        {
            tutorialFireballEnemyReleasedEnd = true;
        }
    }

    private IEnumerator WaitAndSpawnWave(float num_seconds, Wave wave)
    {
        yield return new WaitForSeconds(num_seconds);
        StartCoroutine(Spawn(wave));

    }


    public void Tutorial()
    {
        Debug.Log("on update, stage = " + tutorialStage.ToString());
        
        switch (tutorialStage)
        {
            
            case TutorialStage.IntroDialogue:
               if (!tutorialExitSeen && !tutorialIntroDialogueSeen)
                {
                    startTutorialDialogue();
                    tutorialIntroDialogueSeen = true;
                }
                if (tutorialExitSeen)
                {
                    baseUI.SetActive(true);
                    tutorialStage = TutorialStage.PlayerSeesExit;
                }
                break;
            case TutorialStage.PlayerSeesExit:
                if (!tutorialExitDialogueGiven)
                {
                    startTutorialDialogue();
                    tutorialExitDialogueGiven = true;
                }
                break;
            case TutorialStage.TeachFireball:
                if (!tutorialFireballExplained)
                {
                    startTutorialDialogue();
                    tutorialFireballExplained = true;
                }
                else if (!tutorialFireballEnemyReleasedStart)
                {
                    //release enemy
                   StartCoroutine(WaitAndSpawnWave(5.0f, tutorialFireballWave));
                   tutorialFireballEnemyReleasedStart = true;
                }
                else if (tutorialFireballEnemyReleasedEnd && livingEnemies.Count == 0)
                {
                    tutorialFireballKill = true;
                    tutorialStage = TutorialStage.TeachPunch;
                }
                break;
            case TutorialStage.TeachPunch:
                if (!tutorialPunchExplained)
                {
                    startTutorialDialogue();
                    tutorialPunchExplained = true;
                }
                break;
            case TutorialStage.PunchSkulls:
                break;
            case TutorialStage.GPWave:
                break;
            case TutorialStage.GGWave:
                break;
            case TutorialStage.End:
                break;
            default:
                break;

        }
    }
}


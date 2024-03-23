using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD.Studio;


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
    public GruntPlayer gruntStayWithTank;
    public Tank tank;
    public Explosive explosive;

    public bool wavemode = false;//check wheather the game is in wave mode or downtime mode.
    public Wave tutorialFireballWave;
    public Wave tutorialPunchWave;
    public Wave tutorialGruntPlayerWave;
    public Wave tutorialGruntGoalWave;
    


    public Wave wave1;
    public Wave wave2;
    public Wave wave3;
    public Wave endingWave;

    public bool isSpawning;
    public List<Enemy> livingEnemies; // TODO: remove from this as enemies die.

    public int TotalLivingEnemies => livingEnemies.Count;

    public GameObject paroleGuardSprite;
    public GameObject baseUI;
    public GameObject actionPrompts;


    //public GameObject tutorialManager;

    public Dialogue tutorialIntroDialogue;
    public Dialogue tutorialPlayerSeesExitDialogue;
    public Dialogue tutorialTeachFireballDialogue;
    public Dialogue tutorialTeachPunchDialogue;
    public Dialogue tutorialGPWaveDialogue;
    public Dialogue tutorialGGWaveDialogue;
    public Dialogue tutorialFindParoleGuardDialogue;

    public Dialogue tutorialFlameAttackDialogue;
    public Dialogue tutorialSnapDialogue;

    public Dialogue tutorialEndDialogue;

    public enum TutorialStage { IntroDialogue, PlayerSeesExit, TeachFireball, TeachPunch, GPWave, GGWave, PunchSkulls, End};
    public TutorialStage tutorialStage;

    private bool tutorialIntroDialogueSeen;
    private bool tutorialExitSeen; //make this into trigger?
    private bool tutorialExitDialogueGiven;
    private bool tutorialFireballExplained;
    private bool tutorialFireballEnemyReleasedStart;
    private bool tutorialFireballEnemyReleasedEnd;
    private bool tutorialFireballKill;
    private bool tutorialPunchExplained;
    private bool tutorialPunchEnemyReleasedStart;
    private bool tutorialPunchEnemyReleasedEnd;
    private bool tutorialPunchKill;

    private bool tutorialGPEnemiesReleasedStart;
    private bool tutorialGPEnemiesReleasedEnd;
    private bool tutorialGGEnemiesReleasedStart;
    private bool tutorialGGEnemiesReleasedEnd;

    private bool tutorialFindParoleGuardDialogueSeen;

    private bool tutorialSkullPunchExplained;
    private bool tutorialSkullPunched;


    public bool SnapPromptHidden;
    public GameObject SnapPrompt;

    public bool PunchPromptHidden;
    public GameObject PunchPrompt;

    public bool FlameAttackPromptHidden;
    public GameObject FlameAttackPrompt;

    public bool FireballPromptHidden;
    public GameObject FireballPrompt;

    

    

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
            case TutorialStage.GPWave:
                break;
            case TutorialStage.GGWave:
                break;
            case TutorialStage.PunchSkulls:
                tutorialDialogue = tutorialFindParoleGuardDialogue;
                break;
            default:
                break;

        }

        // If we're done the tutorial, start the downtime dialogue instead of tutorial dialogue.
        if (GameManager.Instance.gameStage == GameManager.GameStage.Downtime1) {
            startDowntimeDialogue();
        } else {
            FindObjectOfType<DialogueManager>().StartDialogue(tutorialDialogue);
        }
    }


    private void Update()
    {

        if (GameManager.Instance.tutorialGameStages.Contains(GameManager.Instance.gameStage))
        {
            Tutorial();
            
        }
        else if (InputManager.Instance.startwave)
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
                case GameManager.GameStage.Downtime3: // ? added to work
                    StartWave(wave3);
                    print("wave 3");
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
                //SoundManager.Instance.MusicStop();
                SoundManager.Instance.WaveMusicStop();
                GameManager.Instance.gameStage++;
                if (GameManager.Instance.gameStage == GameManager.GameStage.Ending) {
                    // Turn off the music.
                    //SoundManager.Instance.MusicStop();

                    // Start the ending wave.
                    StartWave(endingWave);
                } else {
                    startDowntimeDialogue();
                }
                //Debug.Log(GameManager.Instance.gameStage);
            }
        }
    }

    public void StartWave(Wave wave) {
        SoundManager.Instance.WaveMusicPlay();
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
                    case Wave.EnemyType.GruntPlayerWithTank:
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
                if((wave.chunks[i].spawnDelay == 0 && i>0) == false)
                {
                    //SoundManager.Instance.PlaySoundOnce(clip, spawnPoint);
                    Debug.Log("oo");
                    SoundManager.Instance.PlayOneShot(FMODEvents.Instance.enemySpawn, spawnPoint.position);
                }

                
            }
            


        }
        isSpawning = false;
        if (!tutorialFireballEnemyReleasedEnd)
        {
            tutorialFireballEnemyReleasedEnd = true;
        } else if (!tutorialPunchEnemyReleasedEnd)
        {
            tutorialPunchEnemyReleasedEnd = true;
        } else if (!tutorialGPEnemiesReleasedEnd)
        {
            tutorialGPEnemiesReleasedEnd = true;
        } else if (!tutorialGGEnemiesReleasedEnd)
        {
            tutorialGGEnemiesReleasedEnd = true;
        }
    }

    private IEnumerator WaitAndSpawnWave(float num_seconds, Wave wave)
    {
        yield return new WaitForSeconds(num_seconds);
        StartCoroutine(Spawn(wave));

    }


    public void Tutorial()
    {

        // Debug.Log("on update, stage = " + tutorialStage.ToString());

        if (SnapPromptHidden)
        {
            SnapPrompt.SetActive(false);
        }
        else
        {
            SnapPrompt.SetActive(true);
        }

        if (PunchPromptHidden)
        {
            PunchPrompt.SetActive(false);
        }
        else
        {
            PunchPrompt.SetActive(true);
        }

        if (FlameAttackPromptHidden)
        {
            FlameAttackPrompt.SetActive(false);
        }
        else
        {
            FlameAttackPrompt.SetActive(true);
        }

        if (FireballPromptHidden)
        {
            FireballPrompt.SetActive(false);
        }
        else
        {
            FireballPrompt.SetActive(true);
        }

        if (tutorialExitDialogueGiven)
        {
            baseUI.SetActive(true);
        }

        
        switch (tutorialStage)
        {
            
            case TutorialStage.IntroDialogue:
                //actionPrompts.SetActive(true);
               if (!tutorialExitSeen && !tutorialIntroDialogueSeen)
                {
                    SoundManager.Instance.wave0.start();
                    
                    startTutorialDialogue();
                    tutorialIntroDialogueSeen = true;
                }
                if (tutorialExitSeen)
                {
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 1);//next stage of dynamic music
                    baseUI.SetActive(true);
                    tutorialStage = TutorialStage.PlayerSeesExit;
                }
                break;
            case TutorialStage.PlayerSeesExit:
                if (!tutorialExitDialogueGiven)
                {
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 2);
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
                /*
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
                }*/
                else if (tutorialTeachFireballDialogue.dialogueOver && !tutorialFireballEnemyReleasedStart)
                {
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 3);
                    FireballPromptHidden = false;
                    StartCoroutine(Spawn(tutorialFireballWave));
                    tutorialFireballEnemyReleasedStart = true;
                }
                if (tutorialFireballEnemyReleasedEnd && livingEnemies.Count == 0)
                {
                    tutorialFireballKill = true;
                    tutorialStage = TutorialStage.TeachPunch;
                }
                {

                }
                
                break;
            case TutorialStage.TeachPunch:
                if (!tutorialPunchExplained)
                {
                    startTutorialDialogue();
                    tutorialPunchExplained = true;
                }
                
                
                else if (!tutorialPunchEnemyReleasedStart && tutorialTeachPunchDialogue.dialogueOver)
                {
                    FireballPromptHidden = true;
                    PunchPromptHidden = false;
                    StartCoroutine(Spawn(tutorialPunchWave));
                    tutorialPunchEnemyReleasedStart = true;
                }
                else if (tutorialPunchEnemyReleasedEnd && livingEnemies.Count == 0)
                {
                    tutorialPunchKill = true;
                    tutorialStage = TutorialStage.GPWave;
                }
                break;

            case TutorialStage.GPWave:
                if (!tutorialGPEnemiesReleasedStart)
                {
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 4);
                    FireballPromptHidden = false;
                    StartCoroutine(Spawn(tutorialGruntPlayerWave));
                    tutorialGPEnemiesReleasedStart = true;
                }
                else if (tutorialGPEnemiesReleasedEnd && livingEnemies.Count == 0)
                {
                    tutorialStage = TutorialStage.GGWave;
                }
                break;
            case TutorialStage.GGWave:
                if (!tutorialGGEnemiesReleasedStart)
                {
                    StartCoroutine(Spawn(tutorialGruntGoalWave));
                    tutorialGGEnemiesReleasedStart=true;
                }
                else if (tutorialGGEnemiesReleasedEnd && livingEnemies.Count == 0)
                {
                    tutorialStage = TutorialStage.PunchSkulls;
                }
                break;
            case TutorialStage.PunchSkulls:
                if (!tutorialFindParoleGuardDialogueSeen)
                {
                    SoundManager.Instance.wave0.stop(STOP_MODE.ALLOWFADEOUT);
                    SoundManager.Instance.hello.start();
                    // End of tutorial, set the game stage to downtime1.
                    GameManager.Instance.gameStage = GameManager.GameStage.Downtime1;
                    
                    startTutorialDialogue();
                    tutorialFindParoleGuardDialogueSeen = true;
                }

                break;
            default:
                break;

        }
    }
}


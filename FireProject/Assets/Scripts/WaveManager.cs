using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD.Studio;
using System;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine.InputSystem;


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
    public GruntPlayer gruntPlayerTutorial;
    public Enemy explosiveSafer;
    public GameObject arrow;

    public bool wavemode = false;//check wheather the game is in wave mode or downtime mode.
    public Wave tutorialFireballWave;
    public Wave tutorialPunchWave;
    public Wave tutorialGruntPlayerWave;
    public Wave tutorialGruntGoalWave;
    


    public WaveDataObject wave1;
    public WaveDataObject wave2;
    public WaveDataObject wave3;
    public WaveDataObject endingWave;

    public GameObject blockadeCrumblesBeforeWave1;
    public GameObject blockadeCrumblesBeforeWave2;
    public GameObject blockadeCrumblesBeforeWave3;

    public bool isSpawning;
    public List<Enemy> livingEnemies; // TODO: remove from this as enemies die.

    public int TotalLivingEnemies => livingEnemies.Count;

    public GameObject paroleGuardSprite;
    public GameObject triggerAreaForParoleDialogue;
    public GameObject triggerAreaForSkullPromptDialogue;
    public GameObject triggerAreaForSprintDialogue;
    public GameObject triggerAreaForFireballDialogue;
    public GameObject baseUI;
    public GameObject actionPrompts;
    public GameObject gatePointLight;
    public GameObject player;
    public GameObject waveNumberText;
    public GameObject findGatePrompt;
    public GameObject bloodrushBar;
    public GameObject dialogueText;
    public GameObject restartWaveButton;



    //public GameObject tutorialManager;

    public Dialogue tutorialIntroDialogue;
    public Dialogue tutorialTeachSprintDialogue;
    public Dialogue tutorialTeachFireballDialogue;
    public Dialogue tutorialTeachPunchDialogue;
    public Dialogue tutorialGPWaveDialogue;
    public Dialogue tutorialGGWaveDialogue;
    public Dialogue tutorialFindParoleGuardDialogue;

    public Dialogue tutorialFlameAttackDialogue;
    public Dialogue tutorialSnapDialogue;

    public Dialogue tutorialEndDialogue;
    public Dialogue endParoleGuardDialogue;
    public Dialogue endDemoDialogue;
    private bool demoEnding;

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
    private bool conversationPromptFixed;

    private bool tutorialGPWaveDialogueOver;

    private bool tutorialGPEnemiesReleasedStart;
    private bool tutorialGPEnemiesReleasedEnd;
    private bool tutorialGGEnemiesReleasedStart;
    private bool tutorialGGEnemiesReleasedEnd;

    private bool tutorialFindParoleGuardDialogueSeen;

    private bool tutorialSkullPunchExplained;
    public bool tutorialSkullPilePunched;


    public bool SnapPromptHidden;
    public GameObject SnapPrompt;
    public GameObject QPrompt;

    public bool PunchPromptHidden;
    public GameObject PunchPrompt;
    public GameObject EPrompt;

    public bool FlameAttackPromptHidden;
    public GameObject FlameAttackPrompt;
    public GameObject LeftClickPrompt;

    public bool FireballPromptHidden;
    public GameObject FireballPrompt;
    public GameObject RightClickPrompt;

    

    

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

        //paroleGuardSprite.SetActive(true);
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
                tutorialDialogue = tutorialTeachSprintDialogue;
                break;
            case TutorialStage.TeachFireball:
                tutorialDialogue = tutorialTeachFireballDialogue;
                break;
            case TutorialStage.TeachPunch:
                tutorialDialogue = tutorialTeachPunchDialogue;
                break;
            case TutorialStage.GPWave:
                tutorialDialogue = tutorialGPWaveDialogue;
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
        if (GameManager.Instance.gameStage == GameManager.GameStage.Downtime1 || tutorialStage == TutorialStage.PunchSkulls) {
            startDowntimeDialogue();
        } else {
            FindObjectOfType<DialogueManager>().StartDialogue(tutorialDialogue);
        }
    }


    private void Update()
    {

        if (Gamepad.current != null)
        {
            GameManager.Instance.usingController = true;
        }
        else
        {
            GameManager.Instance.usingController = false; 
        }

        if (GameManager.Instance.tutorialGameStages.Contains(GameManager.Instance.gameStage) || GameManager.Instance.gameStage == GameManager.GameStage.Downtime1)
        {
            Tutorial();
            
        }
        else if (wavemode == false)
        {
            // Skip wave starting if we're in the demo ending.
            if (demoEnding) return;
            bloodrushBar.SetActive(true);
            // Start the wave corresponding to the current GameStage.
            switch (GameManager.Instance.gameStage)
            {
                case GameManager.GameStage.Wave1:
                    tutorialSkullPilePunched = true;
                    Color32 activeColor = new Color32(255, 255, 255, 255);
                    SnapPrompt.GetComponent<Image>().color = activeColor;
                    QPrompt.GetComponent<Image>().color = activeColor;
                    player.GetComponent<Controller>().snapAllowed = true;
                    FlameAttackPrompt.GetComponent<Image>().color = activeColor;
                    LeftClickPrompt.GetComponent<Image>().color = activeColor;
                    player.GetComponent<Controller>().flameAttackAllowed = true;
                    //StartCoroutine(PromptFlash(LeftClickPrompt, FlameAttackPrompt));
                    StartCoroutine(LeftClickPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    StartCoroutine(FlameAttackPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    //StartCoroutine(PromptFlash(QPrompt, SnapPrompt));
                    StartCoroutine(QPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    StartCoroutine(SnapPrompt.GetComponent<UIElement>().SizeFlash(1.5f));

                    blockadeCrumblesBeforeWave1.SetActive(false);
                    blockadeCrumblesBeforeWave2.SetActive(true);
                    blockadeCrumblesBeforeWave3.SetActive(true);
                    triggerAreaForSprintDialogue.SetActive(false);
                    triggerAreaForFireballDialogue.SetActive(false);
                    StartWave(wave1.wave);
                    break;
                case GameManager.GameStage.Wave2:
                    tutorialSkullPilePunched = true;
                    blockadeCrumblesBeforeWave1.SetActive(false);
                    blockadeCrumblesBeforeWave2.SetActive(false);
                    blockadeCrumblesBeforeWave3.SetActive(true);
                    StartWave(wave2.wave);
                    break;
                case GameManager.GameStage.Wave3:
                    tutorialSkullPilePunched = true;
                    blockadeCrumblesBeforeWave1.SetActive(false);
                    blockadeCrumblesBeforeWave2.SetActive(false);
                    blockadeCrumblesBeforeWave3.SetActive(false);
                    StartWave(wave3.wave);
                    break;
            }
        }

        if ((wavemode == true) && (isSpawning == false) ) {
            bloodrushBar.SetActive(true);
            if (livingEnemies.Count == 0)
            {
                if(!GameManager.Instance.gameOver) endwave();
                
                //Debug.Log(GameManager.Instance.gameStage);
            }
        }
    }

    public void endwave()
    {
        // yield return new WaitForSeconds(0f);
        wavemode = false;
        waveNumberText.SetActive(false);
        triggerAreaForParoleDialogue.SetActive(true);
        print("Wave Over");
        restartWaveButton.SetActive(false);

        SoundManager.Instance.WaveMusicStop();

        // If we're in the demo mode, end the game after the second wave.
        if (GameManager.Instance.demoMode && GameManager.Instance.gameStage == GameManager.GameStage.Wave2)
        {
            demoEnding = true;
            triggerAreaForParoleDialogue.SetActive(false);
            StartCoroutine(startDemoEndingDialogue());
            return;
        }

        // Don't increment the game stage if we're in the ending, so that the wave repeats if the player somehow survives.
        if (GameManager.Instance.gameStage != GameManager.GameStage.Ending) GameManager.Instance.gameStage++;
        if (GameManager.Instance.gameStage == GameManager.GameStage.Ending)
        {
            paroleGuardSprite.GetComponent<paroleAnimations>().notbetray();
            paroleGuardSprite.GetComponent<paroleAnimations>().hide();
            // Turn off the music.
            //SoundManager.Instance.MusicStop();

            // Start the ending wave.
            StartWave(endingWave.wave);
            print("Ending Wave");
        }
        else if (GameManager.Instance.gameStage == GameManager.GameStage.PreEnding)
        {
            StartCoroutine(startEndingDialogue());
        }
        else
        {
            paroleGuardSprite.GetComponent<paroleAnimations>().unhide();
            SoundManager.Instance.hello = SoundManager.Instance.CreateInstance(FMODEvents.Instance.hello);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(SoundManager.Instance.hello, paroleGuardSprite.transform);
            SoundManager.Instance.hello.start();
            SoundManager.Instance.hello.release();
            arrow.SetActive(true);
            startDowntimeDialogue();

        }

    }

    IEnumerator startEndingDialogue() {
        yield return new WaitForSeconds(2.0f);
        FindObjectOfType<DialogueManager>().StartDialogue(endParoleGuardDialogue, null, true);
    }

    IEnumerator startDemoEndingDialogue() {
        yield return new WaitForSeconds(1.0f);
        FindObjectOfType<DialogueManager>().StartDialogue(endDemoDialogue, null, true);
    }

    public void StartWave(Wave wave) {
        triggerAreaForParoleDialogue.SetActive(false);
        waveNumberText.SetActive(true);
        restartWaveButton.SetActive(true);


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
        int currentChunk;
        for (int i = 0; i < wave.chunks.Count; i++)
        {
            print("Spawning Chunk: " + i);
            if (wave.chunks[i].enemyType == Wave.EnemyType.None) print("Break " + wave.chunks[i].spawnDelay+"s");
            currentChunk = i;
            if(wave.chunks[i].spawnDelay>0) yield return new WaitForSeconds(wave.chunks[i].spawnDelay);
            float t = 0; // time in chunk
            while (true)
            {
                if (wave.chunks[i].maxWait != -1 &&
                    t >= wave.chunks[i].maxWait) break;

                if (wave.chunks[i].waitUntilPreviousDead && 
                    ((t > 25f && TotalLivingEnemies <= 3)||t>5f&&TotalLivingEnemies==0)) break;

                if (!wave.chunks[i].waitUntilPreviousDead) break;

                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }

            Enemy e = null;
            for(int j = 0; j < Mathf.RoundToInt(wave.chunks[i].count*wave.chunks[i].countMultiplier*wave.countMultiplier); ++j)
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
                        e.GetComponent<GruntPlayer>().tutorialGrunt = false;
                        break;
                    case Wave.EnemyType.Tank:
                        e = tank;
                        break;
                    case Wave.EnemyType.GruntPlayerWithTank:
                        e = gruntStayWithTank;
                        e.GetComponent<GruntPlayer>().tutorialGrunt = false;
                        break;
                    case Wave.EnemyType.Explosive:
                        e = explosive;
                        break;
                    case Wave.EnemyType.ExplosiveSafer:
                        e = explosiveSafer;
                        break;
                    case Wave.EnemyType.GruntPlayerTutorial:
                        e = gruntPlayerTutorial;
                        e.GetComponent<GruntPlayer>().tutorialGrunt = true;
                        Debug.Log("spawnwd grunt player tutorial");
                        break;

                }
                GameObject enemy1 = Instantiate(e.gameObject);
                livingEnemies.Add(enemy1.GetComponent<Enemy>());
                enemy1.GetComponent<Enemy>().Restart(wave.chunks[i].healthMultiplier*wave.healthMultiplier,wave.chunks[i].damageMultiplier*wave.damageMultiplier);

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

        Color32 inactiveColor = new Color32(0, 0, 0, 100);
        Color32 activeColor = new Color32(255, 255, 255, 255);

        // Debug.Log("on update, stage = " + tutorialStage.ToString());

        if (SnapPromptHidden)
        {
            //SnapPrompt.SetActive(false);
            SnapPrompt.GetComponent<Image>().color = inactiveColor;
            QPrompt.GetComponent<Image>().color = inactiveColor;
            player.GetComponent<Controller>().snapAllowed = false;

        }
        else
        {
            SnapPrompt.GetComponent<Image>().color = activeColor;
            QPrompt.GetComponent<Image>().color = activeColor;
            player.GetComponent<Controller>().snapAllowed = true;
        }

        if (PunchPromptHidden)
        {
            PunchPrompt.GetComponent<Image>().color = inactiveColor;
            EPrompt.GetComponent<Image>().color = inactiveColor;
            player.GetComponent<Controller>().punchAllowed = false;
        }
        else
        {
            PunchPrompt.GetComponent<Image>().color = activeColor;
            EPrompt.GetComponent<Image>().color = activeColor;
            player.GetComponent<Controller>().punchAllowed = true;
        }

        if (FlameAttackPromptHidden)
        {
            FlameAttackPrompt.GetComponent<Image>().color = inactiveColor;
            LeftClickPrompt.GetComponent<Image>().color = inactiveColor;
            player.GetComponent<Controller>().flameAttackAllowed = false;
        }
        else
        {
            FlameAttackPrompt.GetComponent<Image>().color = activeColor;
            LeftClickPrompt.GetComponent<Image>().color = activeColor;
            player.GetComponent<Controller>().flameAttackAllowed = true;
        }

        if (FireballPromptHidden)
        {
            FireballPrompt.GetComponent<Image>().color = inactiveColor;
            RightClickPrompt.GetComponent<Image>().color = inactiveColor;
            player.GetComponent<Controller>().fireballAllowed = false;
        }
        else
        {
            FireballPrompt.GetComponent<Image>().color = activeColor;
            RightClickPrompt.GetComponent<Image>().color = activeColor;
            player.GetComponent<Controller>().fireballAllowed = true;
        }

        /*if (!tutorialExitDialogueGiven)
        {
            baseUI.SetActive(false);
        }
        else
        {
            baseUI.SetActive(true);
        }*/

        
        switch (tutorialStage)
        {
            
            case TutorialStage.IntroDialogue:
                //actionPrompts.SetActive(true);
               if (!tutorialExitSeen && !tutorialIntroDialogueSeen)
                {
                    //triggerAreaForParoleDialogue.SetActive(false);
                    //triggerAreaForSkullPromptDialogue.SetActive(false);
                    FMOD.Studio.PLAYBACK_STATE state;
                    SoundManager.Instance.wave0.getPlaybackState(out state);
                    if(state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                    {
                        SoundManager.Instance.wave0.start();
                    }
                    
                    GameManager.Instance.fuel = 50f;
                    restartWaveButton.SetActive(true);

                    dialogueText.transform.SetPositionAndRotation(new Vector3(dialogueText.transform.position.x, dialogueText.transform.position.y + 40f, dialogueText.transform.position.z), dialogueText.transform.rotation); ;

                    blockadeCrumblesBeforeWave1.SetActive(true);
                    blockadeCrumblesBeforeWave2.SetActive(true);
                    blockadeCrumblesBeforeWave3.SetActive(true);
                    startTutorialDialogue();
                    tutorialIntroDialogueSeen = true;
                }


                if (tutorialIntroDialogueSeen)
                 {
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 1);//next stage of dynamic music
                    
                }

               if (tutorialIntroDialogue.dialogueOver && !tutorialExitSeen)
                {
                    //gatePointLight.SetActive(true);
                    //findGatePrompt.SetActive(true);
                }

               
                if (tutorialExitSeen)
                {
                   
                    
                    //baseUI.SetActive(true);
                    tutorialStage = TutorialStage.PlayerSeesExit;
                }
                break;
            case TutorialStage.PlayerSeesExit:
                if (!tutorialExitDialogueGiven)
                {
                    //gatePointLight.SetActive(false);
                    //findGatePrompt.SetActive(false);
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

                else if (tutorialTeachFireballDialogue.dialogueOver && !tutorialFireballEnemyReleasedStart)
                {
                    triggerAreaForSprintDialogue.SetActive(false);
                    triggerAreaForFireballDialogue.SetActive(false);
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 3);
                    FireballPromptHidden = false;
                    //StartCoroutine(PromptFlash(RightClickPrompt, FireballPrompt));
                    StartCoroutine(RightClickPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    StartCoroutine(FireballPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    StartCoroutine(Spawn(tutorialFireballWave));
                    tutorialFireballEnemyReleasedStart = true;
                }
                if (tutorialFireballEnemyReleasedEnd && livingEnemies.Count != 0 && GameManager.Instance.fuel < GameManager.Instance.fireballCost)
                {
                    GameManager.Instance.fuel += GameManager.Instance.punchRefuel;
                }
                if (tutorialFireballEnemyReleasedEnd && livingEnemies.Count == 0)
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
                
                
                else if (!tutorialPunchEnemyReleasedStart && tutorialTeachPunchDialogue.dialogueOver)
                {
                    FireballPromptHidden = true;
                    PunchPromptHidden = false;
                    //StartCoroutine(PromptFlash(EPrompt, PunchPrompt));
                    StartCoroutine(EPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    StartCoroutine(PunchPrompt.GetComponent<UIElement>().SizeFlash(1.5f));
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 4);
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
                if (!tutorialGPWaveDialogue.dialogueOver && !tutorialGPWaveDialogueOver)
                {
                    startTutorialDialogue();
                    tutorialGPWaveDialogueOver = true;
                }
                else if (!tutorialGPEnemiesReleasedStart && tutorialGPWaveDialogue.dialogueOver)
                {
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 5);
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
                //Debug.Log("punching skulls stage");
                if (!tutorialFindParoleGuardDialogueSeen)
                {
                    //triggerAreaForParoleDialogue.GetComponent<DialogueTrigger>().conversationStartPrompt.SetActive(false);
                    SoundManager.Instance.wave0.setParameterByName("wave0looping", 6);

                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(SoundManager.Instance.hello, paroleGuardSprite.transform);
                    SoundManager.Instance.hello.start();
                    SoundManager.Instance.hello.release();
                    arrow.SetActive(true);
                    // End of tutorial, set the game stage to downtime1.

                    GameManager.Instance.gameStage = GameManager.GameStage.Downtime1;
                    if (GameManager.Instance.fuel >= GameManager.Instance.GetMaxFuel() - GameManager.Instance.punchRefuel)
                    {
                        GameManager.Instance.fuel -= GameManager.Instance.punchRefuel * 4;
                    }
                    tutorialFindParoleGuardDialogueSeen = true;
                    //triggerAreaForSkullPromptDialogue.SetActive(true);
                    dialogueText.transform.SetPositionAndRotation(new Vector3(dialogueText.transform.position.x, dialogueText.transform.position.y - 40f, dialogueText.transform.position.z), dialogueText.transform.rotation); ;
                    restartWaveButton.SetActive(false);
                    startTutorialDialogue();

                    
                }
                

                break;
            default:
                break;

        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirstGearGames.SmoothCameraShaker;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameStage { TutorialIntro, TutorialFireballWave, TutorialPunchWave, Downtime1, Wave1, Downtime2, Wave2, Downtime3, Wave3, PreEnding, Ending, Endless};
    public GameStage gameStage;
    public GameStage debugGameStage;
    // If true, the game ends after wave 2.
    public bool demoMode;

    public List<GameStage> tutorialGameStages;
    public float AdrenalinePercent => (float)adrenaline / GetMaxAdrenaline();
    // Use this variable to notify the game manager that a successful snap occurred.
    public bool snapped = false;
    public bool dialogueState;
    public bool nextSentenceReady;
    // ADRENALINE VARIABLES
    [Header("Bloodrush/Adrenaline System")]
    public float adrenaline = 0;
    private float interpolant => Time.deltaTime * 1;
    
    private float adrenalineUnit => GetMaxAdrenaline() * 0.01f; // 1/100 of the possible range
    // When this is true, we're waiting for the adrenaline to reach 0 before it starts to recharge.
    private bool recentlySnapped = false;

    private float maxHealth = 100;
    public float intensity = 0; // amount of adrenaline gained per second
    public float maxIntensity;
    public float gainAdrenalineThreshold = 999; // UNUSED!!! the value which intensity must be at before adrenaline increases
    public float enemiesOnFireFactor = 0.1f; // how much effect the number of enemies on fire has on intensity

    public float enemiesOnFireExponent = 2f;
    public float playerHealthFactor = 0.1f; // how much effect player health loss has on intensity
    public float decayAdrenalineThreshold = 0; // the value intensity will decrease if its at or below
    public float decayAmount = 0.1f; // how fast adrenaline is lost when intensity is low enough
    private float enemiesOnFire = 0;
    private float playerHealthLoss;


    [Header("Fuel System")]
    public float fuel = 0;
    public float flamethrowerCost = 0.001f;
    public float fireballCost = 5;
    public float FuelPercent => (float)fuel / GetMaxFuel();
    public float punchRefuel = 10f;
    public float punchSkullRefuel = 50;

    [Header("Endless Mode")]
    public bool endlessMode;
    public int score;
    public GameObject restartWaveButton;
    public GameObject assistModeButton;
    public Wave.EnemyType[] enemyTypes;
    public float[] enemyTypeProbabilities;
    public Wave.SpawnPoint[] spawnPoints;
    public float[] spawnPointProbabilities;
    public int[] enemyCounts;
    public float[] enemyCountProbabilities;
    public int[] spawnDelays;
    public float[] spawnDelayProbabilities;
    public float[] healthMultipliers;
    public float[] healthMultiplierProbabilities;
    public float[] damageMultipliers;
    public float[] damageMultiplierProbabilities;
    public int tankGrunts;      // Number of grunt enemies accompanying a single tank.
    public int maxTanks;         // Maximum number of tanks that can spawn at once.

    [Header("Other")]

    public bool gamePaused;
    public bool gameEndDeath;  // Whether the player has died during the ending.
    public bool gameEndCreditsOver; // Whether the credits have finished.

    public CanvasGroup creditsCanvasGroup;
    public CanvasGroup endCanvasGroup;
    public float fadeDuration = 5f;
    public float creditsAlpha = 0.3f;
    private bool isFading = false;

    public bool baseDamage;
    public bool gameOver = false;
    public int baseFlashCount;
    public ShakeData firballShake;
    public ShakeData punchShake;
    public ShakeData fireShake;
    public ShakeData rumblingShake;
    public ShakeData snapShake;

    public bool usingController;


    private void UpdateAdrenaline() // Adjustable as needed.
    {
        if (recentlySnapped)
        {
            adrenaline = adrenaline - 0.5f;
        } else {
            // If the player has not snapped, check current intensity
            // adrenaline decays if intensity is too low
            // if its high enough, it gets added to adrenaline every tick
            enemiesOnFire = FireManager.manager.EntitiesOnFire;
            playerHealthLoss = maxHealth-Controller.Instance.Health; 
            intensity = Mathf.Pow(enemiesOnFire, enemiesOnFireExponent)*enemiesOnFireFactor + playerHealthLoss*playerHealthFactor;

            intensity = Mathf.Clamp(intensity, 0, maxIntensity); // intensity has a cap
            //intensity = intensity * Time.deltaTime;
            if (intensity <= decayAdrenalineThreshold)
            {
                adrenaline = adrenaline - adrenalineUnit * decayAmount * Time.deltaTime;
                
            }
            else
            {
                intensity = intensity*Time.deltaTime;
                adrenaline = adrenaline + intensity;
            }
            // if (enemy has died) {gain some adrenaline}
            //adrenaline = Mathf.Lerp(adrenaline, FireManager.manager.EntitiesOnFire + incThreshold, interpolant);
        }

        // Clamp the adrenaline value to the range [0, maxAdrenaline].
        adrenaline = Mathf.Clamp(adrenaline, 0, GetMaxAdrenaline());

        if (adrenaline <= 0)
        {
            // Wait until we reach zero adrenaline to start gaining it again.
            recentlySnapped = false;
        }
    }

    public void UpdateFuel(bool isFiring, bool isFireball, bool isRegularPunch = false,bool isSkullPunch = false)
    {
        if (isFiring)
        {
            fuel -= flamethrowerCost*Time.deltaTime;

        }
        if (isFireball)
        {
            fuel -= fireballCost;
        }
        if (isRegularPunch)
        {
            fuel += punchRefuel;
        }
        if (isSkullPunch)
        {
            fuel += punchSkullRefuel;
        }
        fuel = Mathf.Clamp(fuel, 0, GetMaxFuel());
    }

    public float GetMaxFuel() // Adjustable as needed
    {
        //return Mathf.Max(1, WaveManager.Instance.TotalLivingEnemies);
        return 100f;
    }


    public float GetMaxAdrenaline() // Adjustable as needed
    {
        //return Mathf.Max(1, WaveManager.Instance.TotalLivingEnemies);
        return 100;
    }

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

    private void Start()
    {
        if(debugGameStage != gameStage)
        {
            Debug.LogError("WARNING: game stage is being overrided by GameManager.debugGameStage. Set to gameStage for expected behaviour.");
            gameStage = debugGameStage;
        }
        fuel = GetMaxFuel();
        tutorialGameStages = new List<GameStage> { GameStage.TutorialIntro, GameStage.TutorialFireballWave, GameStage.TutorialPunchWave };

        // Set endless mode from player prefs.
        endlessMode = PlayerPrefs.GetInt("EndlessMode", 0) == 1;
        if (endlessMode) gameStage = GameStage.Endless;
        score = 0;
        // Disable certain pause menu options.
        restartWaveButton.SetActive(false);
        assistModeButton.SetActive(false);
    }

    void Update()
    {
        // Check for a successful snap before updating adrenaline.
        if (snapped) {
            recentlySnapped = true;
            snapped = false;
        }
        UpdateAdrenaline();

        // Currently, credits need to finish, and player needs to die.
        if (gameEndDeath && gameEndCreditsOver)
        {
            // Fade out the entire canvas.
            if (!isFading)
            {
                // Assign the canvas group to the end canvas group.
                GameManager.Instance.endCanvasGroup = GameObject.Find("Canvas").transform.Find("FadeToBlackBackground").GetComponent<CanvasGroup>();
                StartCoroutine(FadeOutCanvas(endCanvasGroup, 1f, true));
            }
        } else if (gameEndDeath) {
            // If the player has died during the ending, wait for the credits to finish, but fade out to black.
            if (!isFading)
            {
                // Assign the canvas group to the credits canvas group.
                GameManager.Instance.creditsCanvasGroup = GameObject.Find("Canvas").transform.Find("FadeToDimBackground").GetComponent<CanvasGroup>();
                StartCoroutine(FadeOutCanvas(creditsCanvasGroup, 1f));
            }
        } else if (GameManager.Instance.gameStage == GameManager.GameStage.Ending) {
            // Darken the screen a bit for the credits section.
            if (!isFading)
            {
                // Assign the canvas group to the credits canvas group.
                GameManager.Instance.creditsCanvasGroup = GameObject.Find("Canvas").transform.Find("FadeToDimBackground").GetComponent<CanvasGroup>();
                StartCoroutine(FadeOutCanvas(creditsCanvasGroup, creditsAlpha));
            }
        }
    }

    public IEnumerator FadeOutCanvas(CanvasGroup canvasGroup, float targetAlpha, bool end = false)
    {
        isFading = true;

        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        isFading = false;

        if (end) {
            // First cleanup instances.
            FMODEvents.Instance.StopAllSounds();
            Destroy(SoundManager.Instance.gameObject);
            Destroy(WaveManager.Instance.gameObject);
            Destroy(GameManager.Instance.gameObject);
            Destroy(CombatUI.Instance.gameObject);
            Destroy(Controller.Instance.gameObject);
            Cursor.lockState = CursorLockMode.None;

            // Indicate that player has beaten the game at least once.
            PlayerPrefs.SetInt("GameBeaten", 1);

            // Return to the menu.
            SceneManager.LoadScene(0);
        }
    }
}

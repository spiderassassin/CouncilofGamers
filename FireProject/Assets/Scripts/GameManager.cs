using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameStage { TutorialIntro, TutorialFireballWave, TutorialPunchWave, Downtime1, Wave1, Downtime2, Wave2, Downtime3, Wave3, Ending};
    public GameStage gameStage;
    public GameStage debugGameStage;

    public List<GameStage> tutorialGameStages;
    public float AdrenalinePercent => (float)adrenaline / GetMaxAdrenaline();
    // Use this variable to notify the game manager that a successful snap occurred.
    public bool snapped = false;
    public bool dialogueState;
    public bool nextSentenceReady;
    // ADRENALINE VARIABLES
    public float adrenaline = 0;
    private float interpolant => Time.deltaTime * 1;
    
    private float adrenalineUnit => GetMaxAdrenaline() * 0.01f; // 1/100 of the possible range
    // When this is true, we're waiting for the adrenaline to reach 0 before it starts to recharge.
    private bool recentlySnapped = false;

    private float maxHealth = 100;

    public float intensity = 0; // amount of adrenaline gained per second
    public float maxIntensity;
    public float gainAdrenalineThreshold = 10; // the value intensity must be at before adrenaline increases (might wanna change?)
    public float enemiesOnFireFactor = 0.1f; // how much effect the number of enemies on fire has on intensity
    public float playerHealthFactor = 0.1f; // how much effect player health loss has on intensity
    public float decayAdrenalineThreshold = 0; // the value intensity will decrease if its at or below
    public float decayAmount = 0.1f; // how fast adrenaline is lost when intensity is low enough
    private float enemiesOnFire = 0;
    private float playerHealthLoss;

    // FUEL SYSTEM
    public float fuel = 0;
    public float flamethrowerCost = 0.001f;
    public float fireballCost = 5;
    public float FuelPercent => (float)fuel / GetMaxFuel();
    public float punchRefuel = 10f;

    public bool gamePaused;
    public int gameEndCompletion;  // Number of completed requirements before we return to the main menu.
    public int gameEndCompletionTarget = 2; // Number of requirements before we return to the main menu.

    public CanvasGroup creditsCanvasGroup;
    public CanvasGroup endCanvasGroup;
    public float fadeDuration = 5f;
    public float creditsAlpha = 0.3f;
    private bool isFading = false;


    private void UpdateAdrenaline() // Adjustable as needed.
    {
        if (recentlySnapped)
        {
            // If the player has snapped, use linear interpolation to smoothly decrease the
            // adrenaline value to 0. Aim for a little bit past 0 to ensure the interpolation
            // reaches 0, since it slows down on the edges.
            adrenaline = adrenaline - 0.5f;
        } else {
            // If the player has not snapped, use linear interpolation to smoothly increase the
            // adrenaline value to the number of enemies on fire. Aim for a little bit past the max
            // to ensure the interpolation reaches 0, since it slows down on the edges.
            enemiesOnFire = FireManager.manager.EntitiesOnFire;
            playerHealthLoss = maxHealth-Controller.Instance.Health; 
            intensity = Mathf.Pow(enemiesOnFire, 2)*enemiesOnFireFactor + playerHealthLoss*playerHealthFactor;


            intensity = Mathf.Clamp(intensity, 0, maxIntensity); // max intensity of 5
            intensity = intensity*Time.deltaTime;

            
            if (intensity <= decayAdrenalineThreshold)
            {
                adrenaline = adrenaline - adrenalineUnit * decayAmount * Time.deltaTime;
                
            }
            else
            {
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

    public void UpdateFuel(bool isFiring, bool isFireball, bool isPunch = false)
    {
        if (isFiring)
        {
            fuel -= flamethrowerCost*Time.deltaTime;

        }
        if (isFireball)
        {
            fuel -= fireballCost;
        }
        if (isPunch)
        {
            fuel += punchRefuel;
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
    }

    void Update()
    {
        // Check for a successful snap before updating adrenaline.
        if (snapped) {
            recentlySnapped = true;
            snapped = false;
        }
        UpdateAdrenaline();

        // Darken the screen a bit for the credits section.
        if (GameManager.Instance.gameStage == GameManager.GameStage.Ending && gameEndCompletion < 2) {
            if (!isFading)
            {
                StartCoroutine(FadeOutCanvas(creditsCanvasGroup, creditsAlpha));
            }
        }

        // Increment this elsewhere when the player has completed a requirement for the ending.
        // Currently, credits need to finish, and player meeds to die.
        if (gameEndCompletion == 2)
        {
            // Fade out the entire canvas.
            if (!isFading)
            {
                StartCoroutine(FadeOutCanvas(endCanvasGroup, 1f, true));
            }
        }
    }

    private IEnumerator FadeOutCanvas(CanvasGroup canvasGroup, float targetAlpha, bool end = false)
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
            // Load the main menu.
            SceneManager.LoadScene(0);
        }
    }
}

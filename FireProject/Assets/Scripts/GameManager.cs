using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameStage { TutorialWave, PreWave1, Wave1, PreWave2, Wave2, PreWave3, Wave3, PreFinale, Finale};
    public GameStage gameStage;
    public float AdrenalinePercent => (float)adrenaline / GetMaxAdrenaline();
    public bool snapPossible = false;

    private float adrenaline = 0;
    private float interpolant => Time.deltaTime * 2;
    // Use 1/10 of the possible range to add on to the interpolation so we reach the target faster.
    private float threshold => GetMaxAdrenaline() * 0.1f;
    private bool recentlySnapped = false;
    
    private void UpdateAdrenaline() // Adjustable as needed.
    {
        if (recentlySnapped)
        {
            // If the player has snapped, use linear interpolation to smoothly decrease the
            // adrenaline value to 0. Aim for a little bit past 0 to ensure the interpolation
            // reaches 0, since it slows down on the edges.
            adrenaline = Mathf.Lerp(adrenaline, 0 - threshold, interpolant);
        } else {
            // If the player has not snapped, use linear interpolation to smoothly increase the
            // adrenaline value to the number of enemies on fire. Aim for a little bit past the max
            // to ensure the interpolation reaches 0, since it slows down on the edges.
            float incThreshold = FireManager.manager.EntitiesOnFire > 0 ? threshold :  0;
            adrenaline = Mathf.Lerp(adrenaline, FireManager.manager.EntitiesOnFire + incThreshold, interpolant);
        }

        // Clamp the adrenaline value to the range [0, maxAdrenaline].
        adrenaline = Mathf.Clamp(adrenaline, 0, GetMaxAdrenaline());

        if (adrenaline <= 0)
        {
            // Wait until we reach zero adrenaline to start gaining it again.
            recentlySnapped = false;
        }
    }
    public float GetMaxAdrenaline() // Adjustable as needed.
    {
        //return Mathf.Max(1, WaveManager.Instance.TotalLivingEnemies);
        return 10;
    }

    private void CheckForSnap()
    {
        snapPossible = AdrenalinePercent >= 1f;
        if (InputManager.Instance.snap && snapPossible)
        {
            recentlySnapped = true;
        }
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
        gameStage = GameStage.TutorialWave;
    }

    void Update()
    {
        // Check and set this first, so even if we decrease the adrenaline immediately,
        // other classes can recognize the successful snap.
        CheckForSnap();
        UpdateAdrenaline();
    }

}

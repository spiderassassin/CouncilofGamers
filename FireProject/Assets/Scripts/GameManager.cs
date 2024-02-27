using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameStage { TutorialWave, PreWave1, Wave1, PreWave2, Wave2, PreWave3, Wave3, PreFinale, Finale};
    public GameStage gameStage;

    private float adrenaline = 0;
    public bool dialogueState;

    public float AdrenalinePercent => (float)adrenaline / GetMaxAdrenaline();
    private void UpdateAdrenaline() // Adjustable as needed.
    {
        adrenaline = FireManager.manager.EntitiesOnFire;
    }
    public float GetMaxAdrenaline() // Adjustable as needed.
    {
        //return Mathf.Max(1, WaveManager.Instance.TotalLivingEnemies);
        return 10;
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
        UpdateAdrenaline();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool wavemode = false;//check wheather the game is in wave mode or downtime mode. It is affected by the wavemanager script
    public enum GameStage { TutorialWave, PreWave1, Wave1, PreWave2, Wave2, PreWave3, Wave3, PreFinale, Finale};
    public GameStage gameStage;
    public int adrenaline = 0;
    public int MAX_ADRENALINE = 10;
    public int health = 100;
   
    public Wave wave1;
    public Wave wave2;
    public Wave wave3;
   
    public int MAX_HEALTH = 100;

    public int baseHealth = 5;
    public int Damageboost = 2;
    public int SnapMultiplier = 1;

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

        adrenaline = FireManager.manager.EntitiesOnFire;
        

        if (InputManager.Instance.startwave)
        {
           
            WaveManager.Instance.StartWave(wave1);
        }
        
        
    }



}

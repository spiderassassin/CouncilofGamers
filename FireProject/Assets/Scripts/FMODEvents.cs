using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance;
    //one shot sounds
    public EventReference punch;
    public EventReference fireball;
    public EventReference enemySpawn;
    public EventReference enemyDamage;
    public EventReference playerDamage;
    public EventReference snap;
    public EventReference slowmotion;

    //loop sounds
    public EventReference flamethrower;
    public EventReference walk;
    public EventReference run;

    public EventInstance punchInstance;



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
}
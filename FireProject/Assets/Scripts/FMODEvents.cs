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
    public EventReference pigeons;
    public EventReference punchImpact;
    public EventReference baseDamage;
    public EventReference blood;
    //loop sounds
    public EventReference flamethrower;
    public EventReference walk;
    public EventReference run;
    public EventReference hello;
    public EventReference hhhh;
    public EventReference firespread;
    public EventReference explosionscream;

    //music
    public EventReference wave0;
    public EventReference wave1;
    public EventReference wave2;
    public EventReference wave3;
    public EventReference downtime1;
    public EventReference downtime2;
    public EventReference betrayal;

    public EventReference pauseMenu;



    public Bus master;
    public Bus music;
    public Bus sfx;

    public EventInstance punchInstance;
    public EventInstance pause;


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
        //for sound mixing
        master = RuntimeManager.GetBus("bus:/");
        music = RuntimeManager.GetBus("bus:/music");
        sfx = RuntimeManager.GetBus("bus:/sfx");
    }

    public void StopAllSounds()
    {
        master.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
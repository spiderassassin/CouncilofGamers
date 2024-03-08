using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance;
    public EventReference punch;
    public EventReference fireball;
    public EventReference enemySpawn;
    public EventReference enemyDamage;
    public EventReference playerDamage;
    public EventReference snap;
    public EventReference slowmotion;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

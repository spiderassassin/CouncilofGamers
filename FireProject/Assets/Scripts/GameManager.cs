using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public Transform spawnPoint;
    public static GameManager Instance;
    public Transform playerTransform;
    public List<Enemy> enemies;
    public GruntPlayer gruntPlayer;
    public GruntGoal gruntgoal;
    public Tank tank;
    public Wave wave1;
    public bool isSpawning;





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
       if(InputManager.Instance.startwave)
        {
            StartWave();
        }
    }


    void StartWave() {
        if (isSpawning == false)
        {
            StartCoroutine(Spawn(wave1));
        }
        
    }

    IEnumerator Spawn(Wave wave)
    {
        isSpawning = true;
        for (int i = 0; i < wave.enemies.Count; i++)
        {
            yield return new WaitForSeconds(wave.enemies[i].spawndelay);
            Enemy e = null;
            switch (wave.enemies[i].enemyType)
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
                    

            }
            GameObject enemy1 = Instantiate(e.gameObject);
            enemies.Add(enemy1.GetComponent<Enemy>());
            enemy1.transform.position = spawnPoint.position;
            enemy1.GetComponent<Enemy>().player = playerTransform;



        }
        isSpawning = false;
    }




}

[System.Serializable]
public class Wave
{
    [System.Serializable]
    public class EnemyInstance
    {
        public EnemyType enemyType;
        public float spawndelay = 1f;

    }

    public enum EnemyType { None, GruntGoal, GruntPlayer, Tank};
    public List<EnemyInstance> enemies;

    IEnumerator Spawn()
    {
        for(int i = 0; i<enemies.Count; i++)
        {
            yield return new WaitForSeconds(enemies[i].spawndelay);


        }
    }



}
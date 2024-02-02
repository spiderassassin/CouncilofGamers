using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define the type of enemy
public enum EnemyType
{
    Tank,
    GruntGoal,
    GruntPlayer
}

// Define the state of the enemy
public enum EnemyState
{
    Idling,
    Moving,
    Attacking
}

public abstract class Enemy : MonoBehaviour
{
    public Vector3 goal;
    public Transform player;
    public abstract void update();

    protected UnityEngine.AI.NavMeshAgent agent;
    protected float speed;
    protected EnemyState state = EnemyState.Moving;

    public IEnumerator sleep(int seconds) {
        yield return new WaitForSeconds(seconds);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = 5;
    }

    // Update is called once per frame
    void Update()
    {
        update();
    }
}

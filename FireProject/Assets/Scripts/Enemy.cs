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

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Vector3 goal;
    public Transform player;

    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        switch (enemyType)
        {
            case EnemyType.Tank:
                agent.speed = 5;
                break;
            case EnemyType.GruntGoal:
                agent.speed = 10;
                break;
            case EnemyType.GruntPlayer:
                agent.speed = 10;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Tank:
                // Focus on getting to the goal.
                agent.SetDestination(goal);
                break;
            case EnemyType.GruntGoal:
                // Focus on getting to the goal.
                agent.SetDestination(goal);
                break;
            case EnemyType.GruntPlayer:
                // Focus on getting to the player.
                agent.SetDestination(player.position);
                break;
        }
    }
}

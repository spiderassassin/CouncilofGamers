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
    Attacking,
    LongRangeAttacking
}

public abstract class Enemy : FlammableEntity
{
    public Vector3 goal;
    public Transform player;
    public abstract void update();
    public Animator animator;

    protected UnityEngine.AI.NavMeshAgent agent;
    protected float speed;
    protected EnemyState state = EnemyState.Moving;

    private Camera mainCamera;

    public IEnumerator sleep(int seconds) {
        yield return new WaitForSeconds(seconds);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = 5;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        update();
        // Animation updates.
        if (state == EnemyState.Moving) {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isLongRangeAttacking", false);
        } else if (state == EnemyState.Attacking) {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", true);
            animator.SetBool("isLongRangeAttacking", false);
        } else if (state == EnemyState.LongRangeAttacking) {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isLongRangeAttacking", true);
        } else {  // Idling.
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isLongRangeAttacking", false);
        }
    }

    void LateUpdate() {
        // Billboarding effect.
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}

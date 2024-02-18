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
    protected override void Update()
    {
        // Scale speed based on adrenaline.
        agent.speed = speed + (GameManager.Instance.adranaline);
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

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker, dmg);

        if (dmg.pushBack != 0)
        {
            Vector3 knockbackDirection = (transform.position - Controller.Instance.transform.position);
            knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
            //print(knockbackDirection);
            Rigidbody enemyRigidbody = gameObject.GetComponent<Rigidbody>();
            //print(enemyRigidbody.drag);
            agent.enabled = false;
            transform.position += Controller.Instance.transform.forward * Time.deltaTime * 100;
            enemyRigidbody.AddForce(knockbackDirection * 100, ForceMode.Impulse);
            agent.enabled = true;
        }
    }

    
    public void Attack() {
        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, 5);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable but not Enemy.
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !hit.GetComponent<Enemy>()) {
                damageable.OnDamaged(this, new DamageInformation(10, 0, DamageType.None));
            }
        }
    }
}

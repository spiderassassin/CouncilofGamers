using System.Collections;
using UnityEngine;

public class Tank: Enemy {
    [SerializeField] private GameObject projectile;
    public DamageInformation playerDirectHitDmg;
    public int projectileAttackCooldown = 3;
    private Task waitingShortRange;
    private Task waitingLongRange;
    private Task waitingCooldown;
    public float attackRange = 6;
    public int longAttackPause = 2;
    public int closeAttackPause = 1;
    public float closeAttackCooldown = 1;
    public float closeAttackRange = 5;

    float timeHit;
    
    protected override void Update() {
        healthbar.fillAmount = Health / maxHealth;
        // Handle the short range waiting if it's currently being used.
        if (waitingShortRange != null) {
            if (!waitingShortRange.Running) {
                // Reset task.
                waitingShortRange = null;
                // Keep attacking.
                state = EnemyState.Attacking;
            }
        } else if (waitingLongRange != null) {
            if (!waitingLongRange.Running) {
                // Reset task.
                waitingLongRange = null;
                // Fire!
                Vector3 origin = transform.position;
                origin.y += 1;
                Instantiate(projectile, origin, Quaternion.identity);
                // Resume movement.
                if(agent.enabled)agent.isStopped = false;
                state = EnemyState.Moving;
                // Set cooldown on long range attack.
                waitingCooldown = new Task(projectileAttackCooldown);
            }
        } else if (state == EnemyState.Moving) {
            SetDestination(currentTarget.position);
            if (Vector3.Distance(transform.position, currentTarget.position) < attackRange) {
                // If within 6 units of the goal, attack it.
                state = EnemyState.Attacking;
            } else if (waitingCooldown == null || !waitingCooldown.Running) {
                // If we're not on cooldown, attack the player with a long range attack.
                state = EnemyState.LongRangeAttacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and short range attack.
            SetDestination(transform.position);
            Attack();
            // Wait for attack animation.
            waitingShortRange = new Task(closeAttackPause);
        } else if (state == EnemyState.LongRangeAttacking) {
            // Stop moving and long range attack.
            if(agent.enabled)agent.isStopped = true;
            // Wait for attack animation.
            waitingLongRange = new Task(longAttackPause);
        }

        if(Vector3.Distance(transform.position,currentTarget.position)< closeAttackRange)
        {
            if (Time.timeSinceLevelLoad - timeHit >= closeAttackCooldown)
            {
                Controller.Instance.OnDamaged(this, playerDirectHitDmg);
                timeHit = Time.timeSinceLevelLoad;
            }
        }
    }
}

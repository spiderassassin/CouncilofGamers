using System.Collections;
using UnityEngine;

public class Tank: Enemy {
    [SerializeField] private GameObject projectile;
    private Task waitingShortRange;
    private Task waitingLongRange;
    private Task waitingCooldown;

    public Tank() {
        speed = 2;
    }

    public override void update() {
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
                Instantiate(projectile, transform.position, Quaternion.identity);
                // Resume movement.
                agent.isStopped = false;
                state = EnemyState.Moving;
                // Set cooldown on long range attack.
                waitingCooldown = new Task(5);
            }
        } else if (state == EnemyState.Moving) {
            agent.SetDestination(goal);
            if (Vector3.Distance(transform.position, goal) < 6) {
                // If within 6 units of the goal, attack it.
                state = EnemyState.Attacking;
            } else if (waitingCooldown == null || !waitingCooldown.Running) {
                // If we're not on cooldown, attack the player with a long range attack.
                state = EnemyState.LongRangeAttacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and short range attack.
            agent.SetDestination(transform.position);
            Attack();
            waitingShortRange = new Task(1);
        } else if (state == EnemyState.LongRangeAttacking) {
            // Stop moving and long range attack.
            agent.isStopped = true;
            waitingLongRange = new Task(3);
        }
    }
}
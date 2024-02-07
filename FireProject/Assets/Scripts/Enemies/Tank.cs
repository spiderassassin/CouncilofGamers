using System.Collections;
using UnityEngine;

public class Tank: Enemy {
    public Tank() {
        speed = 2;
    }

    private bool waitingForCooldown = false;
    private bool waitingForLongRangeAttack = false;
    [SerializeField] private GameObject projectile;

    public IEnumerator waitForCooldown(int seconds) {
        yield return new WaitForSeconds(seconds);
        state = EnemyState.LongRangeAttacking;
        waitingForCooldown = false;
    }

    public IEnumerator waitForLongRangeAttack(int seconds) {
        yield return new WaitForSeconds(seconds);
        Instantiate(projectile, transform.position, Quaternion.identity);
        agent.isStopped = false;
        state = EnemyState.Moving;
        waitingForLongRangeAttack = false;
    }

    public override void update() {
        if (state == EnemyState.Moving) {
            agent.SetDestination(goal);
            if (Vector3.Distance(transform.position, goal) < 5) {
                // If within 5 units of the goal, attack it.
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            agent.SetDestination(transform.position);
            // If dest (i.e. player) moves out of range, start moving again.
            if (Vector3.Distance(transform.position, player.position) > 5) {
                state = EnemyState.Moving;
            }
        } else if (state == EnemyState.LongRangeAttacking) {
            if (!waitingForLongRangeAttack) {
                // Stop moving and attack.
                agent.isStopped = true;
                waitingForLongRangeAttack = true;
                StartCoroutine(waitForLongRangeAttack(3));
            }
        }

        // If we're not already waiting for a long range attack cooldown, start waiting.
        if (!waitingForCooldown) {
            waitingForCooldown = true;
            StartCoroutine(waitForCooldown(5));
        }
    }
}

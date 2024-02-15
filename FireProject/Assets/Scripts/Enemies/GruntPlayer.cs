using System.Collections;
using UnityEngine;

public class GruntPlayer: Enemy {
    private Task waiting;

    public GruntPlayer() {
        speed = 10;
    }

    public IEnumerator waitForCooldown(int seconds) {
        yield return new WaitForSeconds(seconds);
    }

    public void Attack() {
        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable but not Enemy.
            IDamagable damageable = hit.GetComponent<IDamagable>();
            if (damageable != null && !hit.GetComponent<Enemy>()) {
                damageable.OnDamaged(this, new DamageInformation(10, 0, DamageType.None));
            }
        }
    }

    public override void update() {
        if (state == EnemyState.Moving) {
            agent.SetDestination(player.position);
            // If we get within 5 units of the player, attack
            if (Vector3.Distance(transform.position, player.position) < 5) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            agent.SetDestination(transform.position);
            Attack();
            state = EnemyState.Idling;
            waiting = new Task(waitForCooldown(5));
        } else if (state == EnemyState.Idling) {
            // Don't do anything if we're on cooldown.
            if (waiting != null && !waiting.Running) {
                // If player moves out of range, start moving again.
                if (Vector3.Distance(transform.position, player.position) > 5) {
                    state = EnemyState.Moving;
                } else {
                    // If we're still in range, attack again.
                    state = EnemyState.Attacking;
                }
            }
        }
    }
}

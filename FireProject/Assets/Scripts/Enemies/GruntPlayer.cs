using System.Collections;
using UnityEngine;

public class GruntPlayer: Enemy {
    private Task waiting;

    public GruntPlayer() {
        speed = 10;
    }

    public override void update() {
        if (!player) return;

        // Handle the waiting if it's currently being used.
        if (waiting != null) {
            if (!waiting.Running) {
                // Reset task.
                waiting = null;
                // If player moves out of range, start moving again.
                if (Vector3.Distance(transform.position, player.position) > 5) {
                    state = EnemyState.Moving;
                } else {
                    // If we're still in range, attack again.
                    state = EnemyState.Attacking;
                }
            }
        } else if (state == EnemyState.Moving) {
            agent.SetDestination(player.position);
            // If we get within 5 units of the player, attack.
            if (Vector3.Distance(transform.position, player.position) < 5) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            agent.SetDestination(transform.position);
            Attack();
            waiting = new Task(1);
        }
    }
}

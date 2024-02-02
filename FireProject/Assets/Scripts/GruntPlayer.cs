using UnityEngine;

public class GruntPlayer: Enemy {
    public GruntPlayer() {
        speed = 10;
    }

    public override void update() {
        if (state == EnemyState.Moving) {
            agent.SetDestination(player.position);
            // If we get within 5 units of the player, attack
            if (Vector3.Distance(transform.position, player.position) < 5) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // TODO: attack. For now, just stop moving.
            agent.SetDestination(transform.position);
        }
    }
}

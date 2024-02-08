using UnityEngine;

public class GruntGoal: Enemy {
    public GruntGoal() {
        speed = 10;
    }

    private Vector3 dest;

    public override void update() {
        if (state == EnemyState.Moving) {
            // If the player gets within a certain range, start moving towards them instead.
            if (Vector3.Distance(transform.position, player.position) < 15) {
                dest = player.position;
            } else {
                dest = goal;
            }
            agent.SetDestination(dest);

            // Stop within 5 units of the destination and attack it.
            if (Vector3.Distance(transform.position, dest) < 5) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            agent.SetDestination(transform.position);
            // If dest (i.e. player) moves out of range, start moving again.
            if (Vector3.Distance(transform.position, player.position) > 5) {
                state = EnemyState.Moving;
            }
        }
    }
}

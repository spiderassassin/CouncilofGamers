using UnityEngine;

public class GruntGoal: Enemy {
    public GruntGoal() {
        speed = 10;
    }

    public override void update() {
        if (state == EnemyState.Moving) {
            agent.SetDestination(goal);
            // Stop within 5 units of the goal and attack it.
            if (Vector3.Distance(transform.position, goal) < 5) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // TODO: attack. For now, just stop moving.
            agent.SetDestination(transform.position);
        }
    }
}

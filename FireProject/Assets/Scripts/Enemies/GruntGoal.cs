using UnityEngine;

public class GruntGoal: Enemy {
    private Task waiting;
    private Vector3 dest;

    protected override void Start()
    {
        speed = 10;
        base.Start();
    }

    protected override void Update() {
        base.Update();

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
            Attack();
            waiting = new Task(1);
        }
    }
}

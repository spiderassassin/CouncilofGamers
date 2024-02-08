using System.Collections;
using UnityEngine;

public class Tank: Enemy {
    // This enemy takes slow steps with some pause in between.
    private Vector3 stepStart;
    private float stepSize = 2;

    public Tank() {
        speed = 5;
    }

     public IEnumerator waitForNextStep(int seconds) {
        yield return new WaitForSeconds(seconds);
        state = EnemyState.Moving;
        // Record the start of the new step.
        stepStart = transform.position;
        agent.isStopped = false;
    }

    public override void update() {
        if (state == EnemyState.Moving) {
            agent.SetDestination(goal);
            if (Vector3.Distance(transform.position, goal) < 5) {
                // If within 5 units of the goal, attack it.
                state = EnemyState.Attacking;
            } else if (Vector3.Distance(stepStart, transform.position) > stepSize) {
                // Finished taking a step.
                state = EnemyState.Idling;
                agent.isStopped = true;
            }
        } else if (state == EnemyState.Attacking) {
            // TODO: attack. For now, just stop moving.
            agent.SetDestination(transform.position);
        } else if (state == EnemyState.Idling) {
            // Wait for a second.
            StartCoroutine(waitForNextStep(1));
        }
    }
}

using UnityEngine;

// Ranged only.
// Always moves toward the goal.
// If the player is within a certain range, and the goal is too far away, attacks the player.
// Otherwise, attacks the goal.
public class GruntGoal: Enemy {
    [SerializeField] private GameObject projectile;
    public int projectileAttackCooldown = 3;
    private Task waitingLongRange;
    private Task waitingCooldown;
    private Vector3 dest;
    private bool attackingGoal = false;
    public float attackRange = 15;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update() {
        base.Update();

        // Handle the long range attack animation delay if it's currently being used.
        if (waitingLongRange != null) {
            if (!waitingLongRange.Running) {
                // Reset task.
                waitingLongRange = null;
                // Attack!
                Vector3 origin = transform.position;
                origin.y += 1;
                // Instantiate the projectile with specific destination.
                GameObject proj = Instantiate(projectile, origin, Quaternion.identity);
                if (attackingGoal) {
                    proj.GetComponent<Projectile>().dest = currentTarget.position;
                    proj.GetComponent<Projectile>().targetPlayer = false;
                }
                // Resume movement.
                if (agent.enabled) agent.isStopped = false;
                state = EnemyState.Moving;
                // Set cooldown on attack.
                waitingCooldown = new Task(projectileAttackCooldown);
            }
        } else if (state == EnemyState.Moving) {
            SetDestination(currentTarget.position);

            // Stop within attack range of the goal and attack it.
            if (Vector3.Distance(transform.position, currentTarget.position) < attackRange) {
                attackingGoal = true;
                state = EnemyState.Attacking;

            } else if (Vector3.Distance(transform.position, currentTarget.position) < attackRange) {
                // If the player is within range, attack the player.
                attackingGoal = false;
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            if (agent.enabled) agent.isStopped = true;
            // Wait for attack animation.
            waitingLongRange = new Task(1);
        }
    }
}

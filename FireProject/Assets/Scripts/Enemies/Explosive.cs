using System.Collections;
using UnityEngine;

// Run toward the player.
// When close to the player, set a timer.
// When the timer is up, explode and deal damage in a large radius.
public class Explosive: Enemy {
    private Task waitingToExplode;
    public float attackRange = 5;
    public float explosionRadius = 10;
    public int explosionDelay = 3;
    

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update() {
        base.Update();
        if (!currentTarget) return;

        // Handle the explosion timer if it's currently being used.
        if (waitingToExplode != null) {
            if (!waitingToExplode.Running) {
                // Reset task (not really necessary).
                waitingToExplode = null;
                // Boom!
                Attack();
                // Kill the enemy.
                Death();
            }
        } else if (state == EnemyState.Moving) {
            SetDestination(currentTarget.position);
            // If we get within attack range of the player, set detonation timer.
            if (Vector3.Distance(transform.position, currentTarget.position) < attackRange) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            SetDestination(transform.position);
            waitingToExplode = new Task(explosionDelay);
        }
    }

    public override void Attack() {
        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, explosionRadius);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable (including enemies).
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null) damageable.OnDamaged(this, attackDamage);
        }
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker, dmg);

        // If we're damaged, start the attack.
        state = EnemyState.Attacking;
    }
}

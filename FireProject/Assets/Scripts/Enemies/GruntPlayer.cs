using System.Collections;
using UnityEngine;

public class GruntPlayer: Enemy {
    private Task waiting;
    public float attackRange = 5;
    public bool stickWithTank = false;
    public bool tutorialGrunt;

    Tank associatedTank;
    

    protected override void Start()
    {
        base.Start();
        if (stickWithTank)
        {
            associatedTank = FindObjectOfType<Tank>();
            if (associatedTank)
            {
                currentTarget = associatedTank.transform;
            }
        }

        if (tutorialGrunt)
        {
            health = 10f;
        }
    }

    int overrideTargetCounter = 10;
    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker, dmg);
        --overrideTargetCounter;
        if (overrideTargetCounter <= 0)
        {
            currentTarget = player;
        }
    }

    protected override void Update() {
        base.Update();
        if (stickWithTank && !associatedTank) currentTarget = player;
        if (!currentTarget) return;

        // Handle the waiting if it's currently being used.
        if (waiting != null) {
            if (!waiting.Running) {
                // Reset task.
                waiting = null;
                // If player moves out of range, start moving again.
                if (Vector3.Distance(transform.position, currentTarget.position) > attackRange) {
                    state = EnemyState.Moving;
                } else {
                    // If we're still in range, attack again.
                    state = EnemyState.Attacking;
                }
            }
        } else if (state == EnemyState.Moving) {
            SetDestination(currentTarget.position);
            // If we get within 5 units of the player, attack.
            if (Vector3.Distance(transform.position, currentTarget.position) < attackRange) {
                state = EnemyState.Attacking;
            }
        } else if (state == EnemyState.Attacking) {
            // Stop moving and attack.
            SetDestination(transform.position);
            Attack();
            waiting = new Task(1);
        }
    }
}

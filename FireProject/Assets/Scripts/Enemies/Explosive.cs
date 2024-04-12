using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// Run toward the player.
// When close to the player, set a timer.
// When the timer is up, explode and deal damage in a large radius.
public class Explosive: Enemy {
    private Task waitingToExplode;
    public float attackRange = 5;
    public float explosionRadius = 10;
    public int explosionDelay = 3;
    public float explosionAnimationDuration;
    EventInstance explode;



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

                // Once the explosion buildup is complete, kill the enemy (explosion happens on death).
                Death();
            }
            else
            {
                
                // Increase scale of enemy to 2x.
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 0.5f, transform.localScale.y + Time.deltaTime * 0.5f, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 0.2f, transform.position.z);
                
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
            explode = SoundManager.Instance.CreateInstance(FMODEvents.Instance.explosionscream);
            RuntimeManager.AttachInstanceToGameObject(explode, transform);
            explode.start();
            explode.release();
            //SoundManager.Instance.PlayOneShot(FMODEvents.Instance.explosionscream, transform.position);
            waitingToExplode = new Task(explosionDelay);
            
        }
    }

    public override void Attack() {

        DamageInformation d = attackDamage;
        d.damage *= damageMultiplier;

        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, explosionRadius);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable (including enemies).
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null) damageable.OnDamaged(this, d);
        }
    }

    public override void Death()
    {
        // When the enemy dies, whether it's from its own buildup or the player killed it, it should explode.
        explode.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        animator.SetBool("isexploding", true);
        StartCoroutine(waitandexplode());
    }

    IEnumerator waitandexplode()
    {
        yield return new WaitForSeconds(explosionAnimationDuration);
        Attack();
        // Kill the enemy.
        //Instantiate(Blood, bloodpivot.position, Quaternion.identity);
        //SoundManager.Instance.PlayOneShot(FMODEvents.Instance.blood, transform.position);
        base.Death();
    }
}

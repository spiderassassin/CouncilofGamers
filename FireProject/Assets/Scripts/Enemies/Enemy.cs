using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;

// Define the type of enemy
public enum EnemyType
{
    Tank,
    GruntGoal,
    GruntPlayer
}

// Define the state of the enemy
public enum EnemyState
{
    Idling,
    Moving,
    Attacking,
    LongRangeAttacking
}

public abstract class Enemy : FlammableEntity
{
    public Transform goal;
    public Transform player;
    public Animator animator;
    public GameObject Blood;
    public Transform bloodpivot;

    //public AudioClip deathSound;
    public DamageInformation attackDamage;

    protected UnityEngine.AI.NavMeshAgent agent;
    public float speed = 5f;
    protected EnemyState state = EnemyState.Moving;

    private Camera mainCamera;

    private Coroutine pushback;
    protected Transform currentTarget;

    protected float currentBaseSpeed;
    protected float damageMultiplier = 1;

    public Image healthbar;
    public IEnumerator sleep(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void Restart(float healthMultiply, float damageMultiplier)
    {
        health *= healthMultiply;
        this.damageMultiplier = damageMultiplier;
        Start();
    }
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = true;
        currentBaseSpeed = speed;
        agent.speed = currentBaseSpeed;

        mainCamera = Camera.main;

        currentTarget = player;
        healthbar.fillAmount = Health / health;

    }

    protected override void Update()
    {
        healthbar.fillAmount = Health / health;
        base.Update();
        // Scale speed based on adrenaline.
        agent.speed = currentBaseSpeed + (GameManager.Instance.AdrenalinePercent * 3);
        // Animation updates.
        if (state == EnemyState.Moving)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isLongRangeAttacking", false);
        }
        else if (state == EnemyState.Attacking)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", true);
            animator.SetBool("isLongRangeAttacking", false);
        }
        else if (state == EnemyState.LongRangeAttacking)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isLongRangeAttacking", true);
        }
        else
        {  // Idling.
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isLongRangeAttacking", false);
        }
    }

    void LateUpdate()
    {
        // Billboarding effect.
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker, dmg);
        if (dmg.type == DamageType.AdditiveDamage && attacker is Controller)
        {
            SoundManager.Instance.PlayOneShot(FMODEvents.Instance.punchImpact, transform.position);
            CameraShakerHandler.Shake(GameManager.Instance.punchShake);

        }

        if (dmg.pushBack != 0)
        {
            Vector3 knockbackDirection = (transform.position - attacker.Position);
            knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
            if (pushback != null)
            {
                StopCoroutine(pushback);
            }
            pushback = StartCoroutine(Pushback(knockbackDirection * Mathf.Clamp(dmg.pushBack, -10f,10f))); // limit enemy knockback to prevent them ending up in weird places
        }
    }
    IEnumerator Pushback(Vector3 impulse)
    {
        agent.enabled = false;
        body.isKinematic = false;
        body.AddForce(impulse, ForceMode.Impulse);
        int m = 0;
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            if (body.velocity.magnitude <= .1f) break;
            ++m;
            if (m >= 4) break;
        }
        body.isKinematic = true;
        agent.enabled = true;
    }

    public override void Death()
    {
        base.Death();
        WaveManager.Instance.livingEnemies.Remove(this);
        Instantiate(Blood, bloodpivot.position, Quaternion.identity);
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.blood, transform.position);
        //SoundManager.Instance.PlaySoundOnce(deathSound, transform.position);

        Destroy(gameObject);
    }

    public override void Attack()
    {
        base.Attack();
        print("h");

        DamageInformation d = attackDamage;
        d.damage *= damageMultiplier;

        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, 5);
        foreach (Collider hit in hitColliders)
        {
            // Deal damage if the object has class IDamageable but not Enemy.
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !hit.GetComponent<Enemy>())
            {
                damageable.OnDamaged(this, d);
            }
        }
    }

    public bool SetDestination(Vector3 p)
    {
        if (!agent.enabled) return false;
        if (agent.pathPending) return true;
        bool result = agent.SetDestination(p);
        if (agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            Vector3 corner = agent.path.corners[agent.path.corners.Length - 1];
            if ((p - corner).magnitude > 10)
            {
                Death();
                Debug.LogError("Manually Murder " + transform.name +" "+agent.pathStatus+" "+agent.path.corners.Length);
            }
        }
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile :FlammableEntity
{
    [SerializeField] private float speed = 10f;
    public Vector3 dest;
    public bool targetPlayer = true;
    public float damageRadius = 1;
    public DamageInformation dmg;
    public int excludeLayer = 7;
    public int punchSwitchLayer = 7;

    public new Vector3 Position => transform.position;

    Vector3 last;
    bool allowHitEnemy;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (targetPlayer) {
            // Automatically set the destination to the player's position.
            dest = Controller.Instance.transform.position+Vector3.up*1.5f;
        }

        SetTarget(dest);
    }

    public void SetTarget(Vector3 destination)
    {
        body.velocity = (destination - transform.position).normalized * speed;
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        // base.OnDamaged(attacker, dmg);
        currentHealth = Mathf.Clamp(currentHealth - dmg.damage,0,health);

        if(attacker is Controller && dmg.type== DamageType.AdditiveDamage)
        {
            SetTarget(transform.position + Vector3.down);
            body.velocity *= 2;
        }
        else
        {
            float vy = body.velocity.y;
            Vector3 v = body.velocity;
            v *= (currentHealth / health);
            vy *= 1.7f;
            v.y = vy;
            body.velocity = v;
        }
        // if (health <= 0) Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.gameObject.layer == excludeLayer) return;
        Attack();
    }

    public override void Attack() {
        base.Attack();
        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, damageRadius);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable but not Enemy.
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && (!hit.GetComponent<Enemy>()||allowHitEnemy)) {
                damageable.OnDamaged(this, dmg);
            }
        }
        // Destroy the projectile after dealing damage.
        StopAttack();
    }
    public new void StopAttack()
    {
        Destroy(gameObject);
    }

    public override void Death()
    {
        StopAttack();
    }
}

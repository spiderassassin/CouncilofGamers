using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile :FlammableEntity
{
    [SerializeField] private float speed = 10f;
    public Vector3 dest;
    public bool targetPlayer = true;
    public float damageRadius = 1;

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

        body.velocity = (dest - transform.position).normalized * speed;
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        // base.OnDamaged(attacker, dmg);
        health -= dmg.damage;
        if (health <= 0) Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
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
                damageable.OnDamaged(this, new DamageInformation(10, 0, DamageType.AdditiveDamage,0));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile :Entity
{
    [SerializeField] private float speed = 10f;
    public Vector3 dest;
    public bool targetPlayer = true;
    public Rigidbody body;

    public new Vector3 Position => transform.position;

    Vector3 last;

    // Start is called before the first frame update
    protected void Start()
    {
        if (targetPlayer) {
            // Automatically set the destination to the player's position.
            dest = Controller.Instance.transform.position+Vector3.up*1.5f;
        }

        body.velocity = (dest - transform.position).normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Attack();
    }

    public override void Attack() {
        base.Attack();
        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, 1);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable but not Enemy.
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !hit.GetComponent<Enemy>()) {
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

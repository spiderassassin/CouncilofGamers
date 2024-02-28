using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IAttacker
{
    [SerializeField] private float speed = 10f;
    private Transform dest;

    public Vector3 Position => transform.position;

    // Start is called before the first frame update
    void Start()
    {
        dest = Controller.Instance.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Attack();
    }

    public void Attack() {
        // Deal damage to any entities within a certain range.
        Collider[] hitColliders = Physics.OverlapSphere(Position, 1);
        foreach (Collider hit in hitColliders) {
            // Deal damage if the object has class IDamageable but not Enemy.
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !hit.GetComponent<Enemy>()) {
                damageable.OnDamaged(this, new DamageInformation(10, 0, DamageType.AdditiveDamage));
            }
        }
        // Destroy the projectile after dealing damage.
        Destroy(gameObject);
    }
    public void StopAttack()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile toward the player's position at the time of firing.
        transform.position = Vector3.MoveTowards(transform.position, dest.position+Vector3.up, speed * Time.deltaTime);

    }
}

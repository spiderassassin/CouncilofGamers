using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for entities (IDamageable + IAttacker).
/// <para>IAttacker is probably optional (leave implimentation blank if not needed).</para>
/// </summary>
public abstract class Entity : MonoBehaviour, IDamageable, IAttacker
{
    public Collider[] colliders; // Assign the damageable, flammable colliders for this entity.
    public float health = 100;
    

    public float Health => health;

    public Vector3 Position => transform.position;

    private void OnValidate()
    {
        if (colliders != null&&colliders.Length == 0)
        {
            Debug.LogError("Dont forget to assign colliders! " + gameObject.name);
            colliders = new Collider[]
            {
                GetComponent<Collider>()
            };
        }
            
    }

    public virtual void Attack()
    {
        
    }

    public virtual void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        health -= dmg.damage;
        if (health < 0)
        {
            Death(); // might be bad if this is repeatedly called
        }
    }

    public void StopAttack()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Death()
    {
        print("DYING " + gameObject.name);
    }
}

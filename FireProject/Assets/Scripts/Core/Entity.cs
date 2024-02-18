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
        if (colliders.Length == 0) Debug.LogError("Dont forget to assign colliders!");
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        health -= dmg.damage;
    }

    public void StopAttack()
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for entities (IDamageable + IAttacker).
/// <para>IAttacker is probably optional (leave implimentation blank if not needed).</para>
/// </summary>
public abstract class Entity : MonoBehaviour, IDamagable, IAttacker
{
    public Collider[] colliders;
    public float health = 100;

    public float Health => health;

    public Vector3 Position => transform.position;

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

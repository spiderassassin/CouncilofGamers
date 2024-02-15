using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any object which is flammable must impliment this interface to be used with FireManager.
/// </summary>
public interface IFlammable 
{
    public IDamagable Damageable { get; }
    public PassiveFireSources PassiveFireSources { get;}
    public bool IsOnFire { get; }
    public Collider[] Colliders { get; }

    /// <summary>
    /// Called when this flammable should be set fire.
    /// <para>Under the hood will probably activate some fire source according to dmgtype.</para>
    /// </summary>
    /// <param name="type">The damage type (None -> no fire).</param>
    public void SetFire(DamageType type);

    /// <summary>
    /// Call OnEnable.
    /// </summary>
    public void SubscribeToManager()
    {
        FireManager.manager.AddFlammable(this,Colliders);
    }

    /// <summary>
    /// Call OnDisable
    /// </summary>
    public void RemoveFromManager()
    {
        FireManager.manager.RemoveFlammable(this);
    }
}

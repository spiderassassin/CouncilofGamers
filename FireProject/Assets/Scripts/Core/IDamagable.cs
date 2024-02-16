using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Characterizes an object that can be damaged.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// The current health of this damageable.
    /// </summary>
    public float Health { get; }

    /// <summary>
    /// To be called when this damageable should take damage.
    /// </summary>
    /// <param name="attacker">Who attacked?</param>
    /// <param name="dmg"></param>
    public void OnDamaged(IAttacker attacker, DamageInformation dmg);
}

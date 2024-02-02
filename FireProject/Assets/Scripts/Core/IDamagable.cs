using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float Health { get; }
    public void OnDamaged(IAttacker attacker, DamageInformation dmg);
}

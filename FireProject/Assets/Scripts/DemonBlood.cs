using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBlood : FlammableEntity
{

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        if (dmg.type != DamageType.ClearFire)
        {
            currentHealth += dmg.damage;
        }
        base.OnDamaged(attacker, dmg);
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}

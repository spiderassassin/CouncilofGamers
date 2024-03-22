using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : FlammableEntity
{
    protected override void Update()
    {
        base.Update();
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        
        if (dmg.type == DamageType.AdditiveDamage && attacker == Controller.Instance)
        {
            base.OnDamaged(attacker, dmg);
        }
    }

    public override void Death()
    {
        base.Death();
        GameManager.Instance.UpdateFuel(false, false, true);
    }
}

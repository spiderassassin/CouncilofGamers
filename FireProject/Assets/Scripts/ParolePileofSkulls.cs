using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParolePileofSkulls : FlammableEntity
{
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

        //fill fuel to full
        while (GameManager.Instance.fuel != GameManager.Instance.GetMaxFuel())
        {
            GameManager.Instance.UpdateFuel(false, false, true);
        }    
        Destroy(gameObject);
    }
}

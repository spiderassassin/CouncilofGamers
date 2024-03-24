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
        Debug.Log("skull dmaaged 1");
        
        if (dmg.type == DamageType.AdditiveDamage && attacker == Controller.Instance)
        {
            Debug.Log("skull damaged 2")
;           base.OnDamaged(attacker, dmg);
        }
    }

    public override void Death()
    {
        Debug.Log("skull die");
        base.Death();
        GameManager.Instance.UpdateFuel(false, false, true);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveFireSource : FireSource
{
    /* How it works:
     * In addition to FireSource:
     *      Inflict Damage to self every fireTick (guaranteed every fire tick).
     *      
     * When a IFlammable is set on fire. A PassiveFireSource is actived:
     *      This will inflict damage to the entity on fire.
     *      This will also enable a fire volume allowing the fire to spread.
     */

    // The damage to inflict on the owner of this fire source when the fire source is active.
    // Used for fire sources the do passive damage + spread.
    public DamageInformation selfDamage; // DO NOT RENAME

    private void OnValidate()
    {
        if(selfDamage.type != DamageType.None)
        {
            Debug.LogError("Self damage should be typeless (otherwise the entity would be on fire forever).");
            selfDamage.type = DamageType.None;
        }
    }

    protected override void DamageTick()
    {
        base.DamageTick();

        if (self != null)
        {
            self.Damageable.OnDamaged(source, selfDamage);
        }
    }
}

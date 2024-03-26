using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParolePileofSkulls : FlammableEntity
{
    public GameObject waveManager;
    protected override void Update()
    {
        // base.Update();
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {

        if (dmg.type == DamageType.AdditiveDamage && attacker is Controller)
        {
            base.OnDamaged(attacker, dmg);
        }
    }

    public override void Death()
    {
        base.Death();
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.punchImpact, transform.position);

        //fill fuel to full
        while (GameManager.Instance.fuel != GameManager.Instance.GetMaxFuel())
        {
            GameManager.Instance.UpdateFuel(false, false, true);
        }
        waveManager.GetComponent<WaveManager>().tutorialSkullPilePunched = true;

        Destroy(gameObject);
    }
}

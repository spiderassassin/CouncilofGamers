using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The base class for enemies (Entity + IFlammable).
/// </summary>
public abstract class FlammableEntity : Entity, IFlammable
{
    public TextMeshPro text;
    public PassiveFireSources passiveFireSources;
    public bool onFire = false;

    bool IFlammable.IsOnFire { get => onFire; }
    DamageType CurrentType => passiveFireSources.CurrentState;
    public Collider[] Colliders => colliders;
    public IDamageable Damageable => this;
    public PassiveFireSources PassiveFireSources => passiveFireSources;

    protected virtual void Start()
    {
        passiveFireSources.Initialize(this, this);
    }
    protected virtual void OnEnable()
    {
        ((IFlammable)this).SubscribeToManager();
    }
    protected virtual void OnDisable()
    {
        ((IFlammable)this).RemoveFromManager();
    }

    protected virtual void Update()
    {

    }


    public void SetFire(DamageType type)
    {
        if (!Utilities.IsFireType(type)) return;

        PassiveFireSources.Switch(type);
        if(type != DamageType.ClearFire)
        {
                onFire = true;
        }
        else
        {
            onFire = false;
        }
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker,dmg);

        // If the fire type would override the fire to a lower intensity, prevent that.
        if (Utilities.IsFireType(dmg.type)&& dmg.type < CurrentType)
        {
            dmg.type = DamageType.AdditiveDamage;
        }

        if (attacker == null)
            SetFire(dmg.type);
        else if (!attacker.Equals(this))
            SetFire(dmg.type);

        if (text)
            text.text = Health.ToString();
        else
            print("Health is: " + Health.ToString());
    }

    public void StepUpFire()
    {
        DamageType next = Utilities.GetNextFireStep(CurrentType);
        if (next == DamageType.ClearFire) return;
        SetFire(next);
    }
}

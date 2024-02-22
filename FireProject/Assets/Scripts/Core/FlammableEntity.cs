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
        if(type != DamageType.None)
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

        print(dmg.type);

        if (attacker == null)
            SetFire(dmg.type);
        else if (!attacker.Equals(this))
            SetFire(dmg.type);

        if (text)
            text.text = Health.ToString();
        else
            print("Health is: " + Health.ToString());
    }
}

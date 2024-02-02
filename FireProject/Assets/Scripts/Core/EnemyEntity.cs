using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity, IFlammable
{
    public PassiveFireSources switcher;
    public bool onFire = false;

    bool IFlammable.IsOnFire { get => onFire; }
    public Collider[] Colliders => colliders;
    public IDamagable Damageable => this;
    public PassiveFireSources Switcher => switcher;

    private void Start()
    {
        switcher.Initialize(this, this);
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

        Switcher.Switch(type);
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker,dmg);
        if (attacker == null)
            SetFire(dmg.type);
        else if (!attacker.Equals(this))
            SetFire(dmg.type);

        print(" DAMAGED " + dmg.type);
    }
}

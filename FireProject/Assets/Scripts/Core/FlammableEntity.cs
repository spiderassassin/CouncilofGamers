using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

/// <summary>
/// The base class for enemies (Entity + IFlammable).
/// </summary>
public abstract class FlammableEntity : Entity, IFlammable
{
    public const float SPREAD_DELAY = .5f;

    public Rigidbody body;
    public TextMeshPro text;
    public PassiveFireSources passiveFireSources;
    public bool isBlood = false;
    public bool onFire = false;
    public float fireCounterRequired = 3;
    public float counterDecayTime = 2;
    public Transform visualScaler;

    private Vector3 ogScale;

    float decayCounterTimer;
    float currentFireCounter = 0;
    float lastIncrementTime;
    bool IFlammable.IsOnFire { get => onFire; }
    DamageType CurrentType => passiveFireSources.CurrentState;
    public Collider[] Colliders => colliders;
    public IDamageable Damageable => this;
    public PassiveFireSources PassiveFireSources => passiveFireSources;

    public virtual Vector3 AimVelocity => body.velocity;
    public abstract bool ShouldLeadAim { get; }

    private void OnValidate()
    {
        if (!enabled) return;

        if (!body) body = GetComponent<Rigidbody>();
        if (!body)
        {
            Debug.LogError(gameObject.name+ " - Flammable Entities must have a rigid body attached (isKinematic is fine).");
        }
    }
    protected override void Start()
    {
        ogScale = visualScaler.transform.localScale;
        base.Start();
        passiveFireSources.Initialize(this, this);
    }
    protected virtual void OnEnable()
    {
        currentFireCounter = 0;
        ((IFlammable)this).SubscribeToManager();
    }
    protected virtual void OnDisable()
    {
        ((IFlammable)this).RemoveFromManager();
    }

    protected virtual void Update()
    {
        visualScaler.transform.localScale = Vector3.Lerp(visualScaler.transform.localScale, ogScale, Time.deltaTime * 10f);

        if (PassiveFireSources.CurrentState== DamageType.ClearFire)
        {
            currentFireCounter = 0;
        }

        if (PassiveFireSources.CurrentState == DamageType.FirePassive_Lvl1 
            && (Time.timeSinceLevelLoad-lastIncrementTime)>1f // forgiveness of one second before decrementing
            && currentFireCounter > 0)
        {
            decayCounterTimer += Time.deltaTime;
            if (decayCounterTimer > counterDecayTime)
            {
                SetFireCounter(currentFireCounter - 1);
                decayCounterTimer = 0;
            }
        }
    }

    public void SetFire(DamageType type, float delay)
    {
        if (!Utilities.IsFireType(type)) return;

        PassiveFireSources.Switch(type,delay);
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

        if(dmg.damage > 0f && (visualScaler.transform.localScale - ogScale).magnitude < .2f)
        {
            visualScaler.transform.localScale *= 1.07f;
        }

        // If the fire type would override the fire to a lower intensity, prevent that.
        if (Utilities.IsFireType(dmg.type)&& dmg.type < CurrentType)
        {
            dmg.type = DamageType.AdditiveDamage;
        }

        // Level one is now the transitionary state between not being on fire and being on fire.
        if (dmg.type == DamageType.FirePassive_Lvl1)
        {
            if(dmg.fireCounter>0)
                SetFireCounter(currentFireCounter+dmg.fireCounter);
            lastIncrementTime = Time.timeSinceLevelLoad;

            if (currentFireCounter >= fireCounterRequired&& PassiveFireSources.CurrentState < DamageType.FirePassive_Lvl2)
            {
                float delay = dmg.spreadDamage ? SPREAD_DELAY : 0;
                if (attacker == null)
                    SetFire(DamageType.FirePassive_Lvl2,delay);
                else if (!attacker.Equals(this))
                    SetFire(DamageType.FirePassive_Lvl2,delay);
                if (SoundManager.Instance.firespread == false)
                {
                    print("fire");
                    SoundManager.Instance.firespread = true;
                    SoundManager.Instance.PlayOneShot(FMODEvents.Instance.firespread, transform.position);
                    StartCoroutine(SoundManager.Instance.spread());
                    
                }
                //SoundManager.Instance.PlayOneShot(FMODEvents.Instance.firespread, transform.position);
                if (!isBlood)
                {
                    CameraShakerHandler.Shake(GameManager.Instance.fireShake);
                }

                PassiveFireSources.Spread();
                PassiveFireSources.SpreadScale(1f);
            }
            else
            {
                if (attacker == null)
                    SetFire(dmg.type, 0);
                else if (!attacker.Equals(this))
                    SetFire(dmg.type, 0);
            }
        }

        if (text)
            text.text = string.Format("{0:F1}", Health);
    }

    protected void SetFireCounter(float newValue)
    {
        currentFireCounter = Mathf.Clamp(newValue, 0, fireCounterRequired);

        if (currentFireCounter == 0)
        {
            SetFire(DamageType.ClearFire,0);
        }

        float t = Mathf.InverseLerp(0, fireCounterRequired, currentFireCounter);
        PassiveFireSources.Rescale(t);

        if ((visualScaler.transform.localScale - ogScale).magnitude < .25f)
        {
            visualScaler.transform.localScale *= 1.15f;
        }
    }

    public void StepUpFire()
    {
        DamageType next = (PassiveFireSources.CurrentState == DamageType.FirePassive_Lvl2 ? DamageType.FirePassive_Lvl3 : DamageType.ClearFire);
        if (next == DamageType.ClearFire)
        {
            return;
        }
        SetFire(next,0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a fire source in the world (both passive and active).
/// <para>Must be manually initialized using Initialize()</para>
/// </summary>
public class FireSource : MonoBehaviour
{
    /* How it works:
     * Fire ticks every damageRate.
     * If spreadProbability is satisfied during tick (evaluated per collider inside the fire source): 
     *      inflict activeDamage
     */

    public IAttacker source; // Unity doesn't serialize interfaces :(
    public IFlammable self; 
    public LayerMask detectionMask; 
    public DamageInformation activeDamage;
    // The damage to inflict on other entities when they are inside the fire source and spreadProbability succeeds. DO NOT RENAME
    [SerializeField]
    private float baseLifespan = Mathf.Infinity; // DO NOT RENAME
    public float tickRate = .25f; // DO NOT RENAME
    public float activeDamageProbability = .1f; // DO NOT RENAME

    public bool tick = true;
    public bool damageOnEnter = false;

    /// <summary>
    /// The lifespan remaining on the fire source before it's deactivated.
    /// </summary>
    public float LifeSpan { get; private set; }

    private List<Collider> inRange;
    private float timer;
    private int originalDamage;

    public void Initialize(IAttacker a, IFlammable d)
    {
        source = a;
        self = d;
    }

    private void Start()
    {
        originalDamage = activeDamage.damage;
    }
    protected virtual void OnEnable()
    {
        inRange = new List<Collider>();

        timer = tickRate;
        LifeSpan = baseLifespan;
    }
    protected virtual void OnDisable()
    {
        if(self != null)
            self.SetFire(DamageType.ClearFire);
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        inRange.Add(other);
        if (damageOnEnter)
        {
            FireManager.manager.FireDamageOnCollider(source, other, activeDamage);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        inRange.Remove(other);
    }

    public void Damage()
    {
        DamageTick();
    }

    private void Update()
    {
        if (tick == true)
        {


            // Timer for deactivating the fire source.
            LifeSpan -= Time.deltaTime;
            if (LifeSpan <= 0f)
            {
                gameObject.SetActive(false);
            }

            // Timer for inflicting damage at the damage rate.
            timer += Time.deltaTime;
            if (timer >= tickRate)
            {
                DamageTick();

                timer = 0;
            }
        }
        
    }

    protected virtual void DamageTick()
    {
        activeDamage.damage = originalDamage + (GameManager.Instance.adrenaline * GameManager.Instance.Damageboost);
        foreach (var c in inRange)
        {
            if (Random.Range(0f, 1f) <= activeDamageProbability)
                FireManager.manager.FireDamageOnCollider(source, c, activeDamage);
        }
    }

    public void Attack()
    {
        
    }
}

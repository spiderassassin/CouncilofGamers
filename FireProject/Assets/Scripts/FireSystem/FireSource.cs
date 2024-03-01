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
    public AudioClip damagedClip;
    public bool hitOnce = false;

    public float DamageMultiplier { get; set; }

    /// <summary>
    /// The lifespan remaining on the fire source before it's deactivated.
    /// </summary>
    public float LifeSpan { get; private set; }

    private List<Collider> inRange;
    private float timer;

    public void Initialize(IAttacker a, IFlammable d)
    {
        source = a;
        self = d;
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
            SoundManager.Instance.PlaySoundOnce(damagedClip, transform);
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
        DamageInformation d = activeDamage;
        d.damage *= DamageMultiplier;
        bool damaged = false;
        if (hitOnce)
        {
            Collider temptarget = null;
            float distance = Mathf.Infinity;
            foreach (var c in inRange)
            {
                if (c != null)
                {
                    float temp = Vector3.Distance(transform.position, c.transform.position);
                    if (temp < distance)
                    {
                        distance = temp;
                        temptarget = c;

                    }

                }
                

            }
            FireManager.manager.FireDamageOnCollider(source, temptarget, d);



        }
        else
        {
            foreach (var c in inRange)
            {
                if (Random.Range(0f, 1f) <= activeDamageProbability)
                {
                    FireManager.manager.FireDamageOnCollider(source, c, d);
                    damaged = true;
                }
            }
        }

        
        if (damaged)
        {
            SoundManager.Instance.PlaySoundOnce(damagedClip, transform);
        }
    }

    public void Attack()
    {
        
    }
}

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
    public int maximumTargets = -1; // use -1 for infinite
    public bool requireVisibility = false;
    public LayerMask visibilityCheckMask;

    public float DamageMultiplier { get; set; }

    /// <summary>
    /// The lifespan remaining on the fire source before it's deactivated.
    /// </summary>
    public float LifeSpan { get; private set; }

    public List<Collider> inRange;
    private float timer;

    public bool isPunch = false;

    Collider col;
    Collider[] colliders = new Collider[10];
    RaycastHit hit;

    public void Initialize(IAttacker a, IFlammable d)
    {
        source = a;
        self = d;
    }

    float max(Vector3 v)
    {
        return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
    }

    protected virtual void OnEnable()
    {
        inRange = new List<Collider>();
        col = GetComponent<Collider>();

        timer = tickRate;
        LifeSpan = baseLifespan;

        if (!isPunch) {
            // patch to execution order causing bug
            int c = Physics.OverlapSphereNonAlloc(transform.position, max(col.bounds.extents), colliders);
            for(int i = 0; i < c; ++i)
            {
                OnTriggerEnter(colliders[i]);
            }
        }
    }
    protected virtual void OnDisable()
    {
        if(self != null)
            self.SetFire(DamageType.ClearFire);
    }
    public void SetActive(bool active)
    {
        if(gameObject.activeInHierarchy!=active)
            gameObject.SetActive(active);
    }

    protected virtual bool TriggerValid(Collider other)
    {
        if (!(detectionMask == (detectionMask | (1 << other.gameObject.layer)))) return false;
        if (other.isTrigger) return false;

        return true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!TriggerValid(other)) return;
        if (inRange.Contains(other)) return;

        inRange.Add(other);
        if (damageOnEnter)
        {
            FireManager.manager.FireDamageOnCollider(source, other, activeDamage);
            SoundManager.Instance.PlaySoundOnce(damagedClip, transform);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!TriggerValid(other)) return;
        inRange.Remove(other);
    }

    public void Damage(float overrideProbability = -1)
    {
        DamageTick(overrideProbability);
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

    int SortColliderByDistance(Collider c1, Collider c2)
    {
        return Vector3.Distance(c1.transform.position, transform.position).CompareTo(Vector3.Distance(c2.transform.position, transform.position));
    }

    new void print(object o)
    {
        if (gameObject.name.Contains("2")) MonoBehaviour.print(o);
    }

    protected virtual void DamageTick(float overrideProbability = -1)
    {

        DamageInformation d = activeDamage;
        // d.damage *= DamageMultiplier; // don't do this

        // inefficient patch for null references
        for(int a = inRange.Count-1; a >= 0; --a)
        {
            if (inRange[a] == null) inRange.RemoveAt(a);
        }

        inRange.Sort(SortColliderByDistance);

        int i = maximumTargets;
        foreach (var c in inRange)
        {
            if (requireVisibility)
            {
                if (Physics.Raycast(transform.position, (c.bounds.center-transform.position).normalized, out hit, 100f,visibilityCheckMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider != c) continue;
                }
                else
                {
                    continue;
                }
            }
            if (maximumTargets!=-1 && i <= 0) break;
            if (Random.Range(0f, 1f) <= (overrideProbability == -1 ? activeDamageProbability : overrideProbability))
            {
                FireManager.manager.FireDamageOnCollider(source, c, d);
                if (isPunch)
                {
                    GameManager.Instance.UpdateFuel(false, false, true);
                }
            }
            --i;
        }

        
    }
        



    public void Attack()
    {
        
    }
}

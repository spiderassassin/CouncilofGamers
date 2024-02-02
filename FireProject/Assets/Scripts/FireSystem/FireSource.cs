using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a fire source in the world (both passive and active).
/// <para>Must be manually initialized using Initialize()</para>
/// </summary>
public class FireSource : MonoBehaviour
{
    [SerializeField]
    private bool active = false;
    public IAttacker source; // Unity doesn't serialize interfaces :(
    public IFlammable self; 
    public LayerMask detectionMask;
    public DamageInformation spreadDamage;
    public DamageInformation activeSelfDamage;
    [SerializeField]
    private float baseLifespan = Mathf.Infinity;
    public float damageRate = .25f;
    public float spreadProbability = .1f;

    private List<Collider> inRange;

    /// <summary>
    /// The lifespan remaining on the fire source before it's deactivated.
    /// </summary>
    public float LifeSpan { get; private set; }

    public void Initialize(IAttacker a, IFlammable d)
    {
        source = a;
        self = d;
    }

    private void OnEnable()
    {
        inRange = new List<Collider>(
            );
    }
    private void OnValidate()
    {
        SetActive(active);
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        inRange.Remove(other);
    }

    /// <summary>
    /// Activates the fire source for its lifespan.
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        this.active = active;
        timer = damageRate;
        LifeSpan = baseLifespan;
        gameObject.SetActive(active);
    }

    float timer;

    private void Update()
    {
        if (active)
        {
            // Timer for deactivating the fire source.
            LifeSpan -= Time.deltaTime;
            if (LifeSpan <= 0f)
            {
                SetActive(false);
            }

            // Timer for inflicting damage at the damage rate.
            timer += Time.deltaTime;
            if (timer >= damageRate)
            {
                if (self != null)
                    self.Damageable.OnDamaged(source, activeSelfDamage);

                foreach (var c in inRange)
                {
                    if (Random.Range(0f, 1f) <= spreadProbability)
                        FireManager.manager.FireDamageOnCollider(source, c, spreadDamage);
                }

                timer = 0;
            }
        }
    }

    public void Attack()
    {
        
    }
}

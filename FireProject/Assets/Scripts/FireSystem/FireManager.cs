using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of all IFlammables in the world. Provides methods for inflicting fire damage on generic collider.
/// </summary>
[DefaultExecutionOrder(-1)]
public class FireManager : MonoBehaviour
{
    public static FireManager manager;

    [SerializeField]
    private int flammableEntitiesOnFire = 0;

    private Dictionary<IFlammable, Collider[]> existingFlammables = new Dictionary<IFlammable, Collider[]>();
    private Dictionary<Collider, IFlammable> flammablesMap = new Dictionary<Collider, IFlammable>();

    /// <summary>
    /// The number of flammable entities currently on fire.
    /// </summary>
    public int FlammableEntitiesOnFire => flammableEntitiesOnFire;

    private void Awake()
    {
        manager = this;
    }

    private void Update()
    {
        int c = 0;
        foreach(var f in existingFlammables)
        {
            if (f.Key.IsOnFire) ++c;
        }
        flammableEntitiesOnFire = c;
    }

    /// <summary>
    /// Do fire damage on a generic collider.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="c"></param>
    /// <param name="activeFire">The damage to inflict.</param>
    public void FireDamageOnCollider(IAttacker attacker, Collider c, DamageInformation activeFire)
    {
        IFlammable f = Lookup(c);
        if (f == null) return;
        f.Damageable.OnDamaged(attacker, activeFire);
        if(activeFire.pushBack != 0)
        {
            GameObject enemy = c.gameObject;
            print(enemy.name);
            Vector3 knockbackDirection = (enemy.transform.position - Controller.Instance.transform.position);
            knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
            //print(knockbackDirection);
            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            //print(enemyRigidbody.drag);
            enemy.transform.position += Controller.Instance.transform.forward * Time.deltaTime * 100;
            enemyRigidbody.AddForce(knockbackDirection * 100, ForceMode.Impulse);
            
        }
    }

    public void AddFlammable(IFlammable f, Collider[] c)
    {
        if (existingFlammables.ContainsKey(f)) return;
        existingFlammables.Add(f,c);
        foreach(var c1 in c)
        {
            flammablesMap.Add(c1, f);
        }
        f.SetFire(DamageType.None);
    }
    public void RemoveFlammable(IFlammable f)
    {
        existingFlammables.Remove(f);
        foreach (var c1 in f.Colliders)
        {
            flammablesMap.Remove(c1);
        }
    }

    private IFlammable Lookup(Collider c)
    {
        if (!flammablesMap.ContainsKey(c)) return null;
        return flammablesMap[c];
    }
}
public enum DamageType { None, FirePassive_Lvl1 }

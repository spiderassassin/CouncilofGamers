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

    public GameObject fireSingle;

    private Dictionary<IFlammable, Collider[]> existingFlammables = new Dictionary<IFlammable, Collider[]>();
    private Dictionary<Collider, IFlammable> flammablesMap = new Dictionary<Collider, IFlammable>();

    private void Awake()
    {
        manager = this;
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

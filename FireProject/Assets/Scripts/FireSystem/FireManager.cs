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
    public int EntitiesOnFire => flammableEntitiesOnFire;

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
        
    }
    public void StepFireLevel()
    {
        foreach (var f in existingFlammables)
        {
            IFlammable i = f.Key;
            if (!i.IsOnFire) continue;

            i.StepUpFire();
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
        f.SetFire(DamageType.ClearFire);
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

// NOTE: Append new values to the end.
// See Utilities.cs and update values as needed.
// REQUIRES FirePassive_LvlX to be in ascending values.
public enum DamageType { ClearFire = 0, AdditiveDamage = 1, FirePassive_Lvl1 = 2, FirePassive_Lvl2 = 3, FirePassive_Lvl3 = 4 }

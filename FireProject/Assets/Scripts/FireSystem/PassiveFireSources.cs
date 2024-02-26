using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch fire source depending on DamageType.
/// <para>This is just a glorified gameobject enabler/disabler - will probably change this later.</para>
/// <para>Must be initialized using Initialize()</para>
/// </summary>
public class PassiveFireSources : MonoBehaviour
{
    public FireSource passiveLevel1, passiveLevel2, passiveLevel3;

    public DamageType CurrentState { get; private set; }

    public void Initialize(IAttacker attacker, IFlammable damagable)
    {
        passiveLevel1.Initialize(attacker, damagable);
        passiveLevel2.Initialize(attacker, damagable);
        passiveLevel3.Initialize(attacker, damagable);
    }
    private void DisableAll()
    {
        passiveLevel1.SetActive(false);
        passiveLevel2.SetActive(false);
        passiveLevel3.SetActive(false);
    }

    /// <summary>
    /// Switch to the input damage type.
    /// </summary>
    /// <param name="type">None means to deactivate everything, otherwise specify the type.</param>
    public void Switch(DamageType type)
    {
        if (!Utilities.IsFireType(type)) return;

        if(type== DamageType.ClearFire)
        {
            DisableAll();
        }
        else if(type== DamageType.FirePassive_Lvl1)
        {
            DisableAll();
            passiveLevel1.SetActive(true);
        }
        else if (type == DamageType.FirePassive_Lvl2)
        {
            DisableAll();
            passiveLevel2.SetActive(true);
        }
        else if (type == DamageType.FirePassive_Lvl3)
        {
            DisableAll();
            passiveLevel3.SetActive(true);
        }
        CurrentState = type;
    }
}

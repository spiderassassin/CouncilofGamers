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
    public FireSource passiveLevel1;

    public DamageType CurrentState { get; private set; }

    public void Initialize(IAttacker attacker, IFlammable damagable)
    {
        passiveLevel1.Initialize(attacker, damagable);
    }

    /// <summary>
    /// Switch to the input damage type.
    /// </summary>
    /// <param name="type">None means to deactivate everything, otherwise specify the type.</param>
    public void Switch(DamageType type)
    {
        if(type== DamageType.None)
        {
            passiveLevel1.SetActive(false);
        }
        else
        {
            passiveLevel1.SetActive(true);
        }
        CurrentState = type;
    }
}

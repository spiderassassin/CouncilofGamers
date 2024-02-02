using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch fire source depending on type.
/// </summary>
public class PassiveFireSources : MonoBehaviour
{
    public FireSource passiveLevel1;

    public DamageType CurrentState { get; private set; }

    public void Initialize(IAttacker attacker, IFlammable damagable)
    {
        passiveLevel1.Initialize(attacker, damagable);
    }

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

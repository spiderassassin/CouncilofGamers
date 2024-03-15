using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Packet of information when inflicting damage.
/// </summary>
[System.Serializable]
public struct DamageInformation
{
    public float damage;
    public float pushBack;
    public DamageType type;
    public float fireCounter; // used to decide whether fire is lit or not

    public DamageInformation(float damage, float pushBack, DamageType type, float fireCounter)
    {
        this.damage = damage;
        this.pushBack = pushBack;
        this.type = type;
        this.fireCounter = fireCounter;
    }
}

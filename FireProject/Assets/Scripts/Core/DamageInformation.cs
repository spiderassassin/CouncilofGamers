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
    public bool passIFrames;
    public bool spreadDamage;

    public DamageInformation(float damage, float pushBack, DamageType type, float fireCounter, bool passIframe, bool spreadDamage)
    {
        this.damage = damage;
        this.pushBack = pushBack;
        this.type = type;
        this.fireCounter = fireCounter;
        passIFrames = passIframe;
        this.spreadDamage = spreadDamage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Packet of information when inflicting damage.
/// </summary>
[System.Serializable]
public struct DamageInformation
{
    public int damage;
    public float pushBack;
    public DamageType type;
}

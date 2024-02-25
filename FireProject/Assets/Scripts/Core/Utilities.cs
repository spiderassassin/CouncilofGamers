using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static bool IsFireType(this DamageType t)
    {
        return t== DamageType.ClearFire|| t== DamageType.FirePassive_Lvl1 || t == DamageType.FirePassive_Lvl2 || t == DamageType.FirePassive_Lvl3;
    }
}

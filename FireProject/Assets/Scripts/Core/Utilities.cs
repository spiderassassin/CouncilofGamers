using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static bool IsFireType(this DamageType t)
    {
        return t== DamageType.ClearFire|| t== DamageType.FirePassive_Lvl1 || t == DamageType.FirePassive_Lvl2 || t == DamageType.FirePassive_Lvl3;
    }
    public static DamageType GetNextFireStep(DamageType current)
    {
        if(current== DamageType.FirePassive_Lvl1)
        {
            return DamageType.FirePassive_Lvl2;
        }
        if (current == DamageType.FirePassive_Lvl2)
        {
            return DamageType.FirePassive_Lvl3;
        }
        if (current == DamageType.FirePassive_Lvl3)
        {
            return DamageType.FirePassive_Lvl3;
        }

        return DamageType.ClearFire;
    }
}

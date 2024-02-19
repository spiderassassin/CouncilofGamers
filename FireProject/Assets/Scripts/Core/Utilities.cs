using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static bool IsFireType(this DamageType t)
    {
        return t== DamageType.None|| t== DamageType.FirePassive_Lvl1;
    }
}

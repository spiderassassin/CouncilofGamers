using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDataObject : MonoBehaviour
{
    // using proper scriptableobjects would be prefered but for time whatever.
    public Wave wave;

    private void OnValidate()
    {
        foreach(var c in wave.chunks)
        {
            string s = "";
            if(c.enemyType== Wave.EnemyType.None)
            {
                s += "break for " + c.spawnDelay + "s";
                c.note = s;
                continue;
            }

            if (c.waitUntilPreviousDead) s += "then ";
            s += Mathf.RoundToInt(c.count*c.countMultiplier*wave.countMultiplier).ToString();
            s += "["+c.enemyType.ToString()+"] ";
            if (c.spawnDelay > 0)
            {
                s += "after " + c.spawnDelay.ToString()+"s ";
            }
            s += "from [" + c.spawnPoint.ToString()+"]";
            float hm = c.healthMultiplier * wave.healthMultiplier;
            float dm = c.damageMultiplier * wave.damageMultiplier;
            if (hm != 1)
            {
                s += " x" + hm + "HP";
                if (dm != 1) s += " |";
            }
            if (dm != 1)
            {
                s += " x" + dm + "DMG";
            }
            c.note = s;
        }
    }
}

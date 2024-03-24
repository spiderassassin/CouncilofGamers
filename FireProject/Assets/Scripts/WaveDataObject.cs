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
            if (c.waitUntilPreviousDead) s += "then ";
            s += c.count.ToString();
            s += "["+c.enemyType.ToString()+"] ";
            if (c.spawnDelay > 0)
            {
                s += "after " + c.spawnDelay.ToString()+"s ";
            }
            s += "from [" + c.spawnPoint.ToString()+"]";
            c.note = s;
        }
    }
}

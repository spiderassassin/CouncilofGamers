using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public List<Collider> inRange;

    public int enemyLayer;

    Collider col;
    Collider[] colliders = new Collider[100];
    public LayerMask detectionMask;
    private float lastTime;
    public float tickRate = .25f;

    float max(Vector3 v)
    {
        return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
    }

    /*protected virtual void OnEnable()
    {
        print("h");
        inRange = new List<Collider>();
        col = GetComponent<Collider>();

        
            // patch to execution order causing bug
        int c = Physics.OverlapSphereNonAlloc(transform.position, max(col.bounds.extents), colliders);
        for (int i = 0; i < c; ++i)
            {
                OnTriggerEnter(colliders[i]);
            }
        inRange.Sort(SortColliderByDistance);




    }*/

    protected virtual bool TriggerValid(Collider other)
    {
        if (!(detectionMask == (detectionMask | (1 << other.gameObject.layer)))) return false;
        if (other.isTrigger) return false;

        return true;
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!TriggerValid(other)) return;
        if (inRange.Contains(other)) return;
        if(other.gameObject.layer == enemyLayer && !(other.gameObject.tag == "tank"))
        {
            inRange.Add(other);
        }

        

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        inRange.Remove(other);
    }

    int SortColliderByDistance(Collider c1, Collider c2)
    {
        return Vector3.Distance(c1.transform.position, transform.position).CompareTo(Vector3.Distance(c2.transform.position, transform.position));
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad - lastTime >= tickRate)
        {
            for (int a = inRange.Count - 1; a >= 0; --a)
            {
                if (inRange[a] == null) inRange.RemoveAt(a);
            }
            inRange.Sort(SortColliderByDistance);

            lastTime = Time.timeSinceLevelLoad;
        }
        
    }


}

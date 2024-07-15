using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public List<Collider> inRange;

    public int enemyLayer;

    Collider col;
    Collider[] colliders = new Collider[100];


    float max(Vector3 v)
    {
        return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
    }

    protected virtual void OnEnable()
    {
        inRange = new List<Collider>();
        col = GetComponent<Collider>();

        
            // patch to execution order causing bug
            int c = Physics.OverlapSphereNonAlloc(transform.position, max(col.bounds.extents), colliders);
            for (int i = 0; i < c; ++i)
            {
                OnTriggerEnter(colliders[i]);
            }
        



    }

    protected virtual void OnTriggerEnter(Collider other)
    {
     
        if (inRange.Contains(other)) return;
        if(other.gameObject.layer == enemyLayer)
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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inRange.Sort(SortColliderByDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BloodDecal;
    public LayerMask groundLayer;
    float groundDistance;
    Vector3 hitNormal;

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100, groundLayer))
        {
            // Ray hit something, get the distance to the hit point
            groundDistance = hitInfo.distance;
            hitNormal = hitInfo.normal;
            //Debug.Log("Distance to ground: " + groundDistance);
            GameObject inst = Instantiate(BloodDecal, new Vector3(transform.position.x, transform.position.y - groundDistance + 0.1f, transform.position.z), Quaternion.identity);
            inst.transform.up = hitNormal;
        }
        
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timer = 5;

    // Start is called before the first frame update
    void OnEnable()
    {
        transform.SetParent(null);
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

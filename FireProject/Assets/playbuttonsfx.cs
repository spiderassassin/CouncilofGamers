using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playbuttonsfx : MonoBehaviour
{


    public void playsound()
    {
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.button, transform.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

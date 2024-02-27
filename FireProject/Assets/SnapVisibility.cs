using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SnapVisibility : MonoBehaviour
{

    public bool visible;
    public GameObject snapReadyPrompt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (visible)
        {
            snapReadyPrompt.SetActive(true);

        }
        else
        {
            snapReadyPrompt.SetActive(false);
        }
    }

    public void setVisibility(bool setting)
    {
        visible = setting;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base : Entity
{

    public TextMeshProUGUI baseHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (baseHealth != null)
        {
            baseHealth.text = "Base Health:\n" + Health.ToString() + "%";
        }
    }

    public override void Death()
    {
        base.Death();
        StartCoroutine(GameObject.Find("Player").GetComponent<Controller>().Die(false));
    }
}

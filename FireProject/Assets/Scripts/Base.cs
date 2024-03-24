using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base : Entity
{
    // Start is called before the first frame update

    public TextMeshProUGUI baseHealthText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (baseHealthText != null)
        {
            baseHealthText.text = "Gate Health\n"+base.Health.ToString()+"%";
        }
        
    }

    public override void Death()
    {
        base.Death();
        StartCoroutine(GameObject.Find("Player").GetComponent<Controller>().Die(false));
    }
}

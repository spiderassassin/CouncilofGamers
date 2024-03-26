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
        if (baseHealthText != null && Health>=0)
        {
            baseHealthText.text = "Gate Health\n" + base.Health.ToString() + " HP";
            if (GameManager.Instance.baseDamage)
            {
                if (GameManager.Instance.baseFlashCount % 2 == 0)
                {
                    baseHealthText.color = Color.red;
                }
                else
                {
                    baseHealthText.color = Color.white;
                }
                GameManager.Instance.baseFlashCount++;
                if (GameManager.Instance.baseFlashCount > 20)
                {
                    baseHealthText.color = Color.white;
                    GameManager.Instance.baseFlashCount = 0;
                    GameManager.Instance.baseDamage = false;
                }
            }
        }
        
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker, dmg);
        GameManager.Instance.baseDamage = true;
        
    }

    public override void Death()
    {
        base.Death();
        StartCoroutine(GameObject.Find("Player").GetComponent<Controller>().Die(false));
    }
}

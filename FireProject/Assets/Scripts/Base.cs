using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base : Entity
{
    // Start is called before the first frame update

    private bool damage_on = false;

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
                if (!damage_on)
                {
                    damage_on = true;
                    StartCoroutine(HealthFlash(baseHealthText));
                }
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

    public IEnumerator HealthFlash(TextMeshProUGUI health)
    {
        Vector3 health_orig = health.GetComponent<RectTransform>().localScale;
        Vector3 health_big = new Vector3((float)(health_orig.x * 1.2), (float)(health_orig.y * 1.2), (float)(health_orig.z * 1.2));
        for (int i = 0; i < 5; i++)
        {
            health.GetComponent<RectTransform>().localScale = health_big;
            yield return new WaitForSeconds(0.25f);
            health.GetComponent<RectTransform>().localScale = health_orig;
            yield return new WaitForSeconds(0.25f);
        }
        damage_on = false;
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {
        base.OnDamaged(attacker, dmg);
        GameManager.Instance.baseDamage = true;
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.baseDamage, transform.position);
        
    }

    public override void Death()
    {
        base.Death();
        StartCoroutine(GameObject.Find("Player").GetComponent<Controller>().Die(false));
    }
}

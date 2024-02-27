using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    public static CombatUI Instance;
    public Image overlay;//damage effect
    public Image snapOverlay;
    public float duration;
    public float fadespeed;
    float durationTimer;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        snapOverlay.color = new Color(snapOverlay.color.r, snapOverlay.color.g, snapOverlay.color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer < duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadespeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);

            }
        }

        if (snapOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer < duration)
            {
                float tempAlpha = snapOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadespeed;
                snapOverlay.color = new Color(snapOverlay.color.r, snapOverlay.color.g, snapOverlay.color.b, tempAlpha);

            }
        }
    }

    public void DamageOverlay()
    {
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.5f);
    }
    public void snap()
    {
        durationTimer = 0;
        snapOverlay.color = new Color(snapOverlay.color.r, snapOverlay.color.g, snapOverlay.color.b, 1f);
    }

}

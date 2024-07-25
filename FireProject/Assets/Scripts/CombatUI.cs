using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class CombatUI : MonoBehaviour
{
    public static CombatUI Instance;
    public Image overlay;//damage effect
    public Image snapOverlay;
    public float duration;
    public float fadespeed;
    float durationTimer;
    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;
    private Vignette vignette;
    private bool grey = false;
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
        
        postProcessVolume.profile.TryGetSettings(out colorGrading);//for saturation effect on snap
        postProcessVolume.profile.TryGetSettings(out vignette);

    }

    // Update is called once per frame
    void Update()
    {
        
        
       vignette.intensity.value = 1 - (Controller.Instance.Health / 100f) - 0.1f;
        
        







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
    public void lerptogrey()
    {

        colorGrading.saturation.value = 50;
        colorGrading.contrast.value = 100;
        grey = true;

    }
    public void lerptocolor()
    {
        colorGrading.saturation.value = 0;
        colorGrading.contrast.value = 0;
        grey = false;
    }

}

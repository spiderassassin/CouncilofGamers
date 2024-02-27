using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdrenalineSlider : MonoBehaviour
{
    public Slider adrenalineBar;
    public Image colourAdjustment;
    public GameObject snapReadyPrompt;
    
    private void Start()
    {
        adrenalineBar = GetComponent<Slider>();
    }

    public void Update()
    {
        adrenalineBar.value = GameManager.Instance.AdrenalinePercent;

        if (adrenalineBar.value >= 1f)
        {
            colourAdjustment.color = Color.white;
            snapReadyPrompt.SetActive(true);
            //GameObject.Find("Snap Ready Prompt").GetComponent<SnapVisibility>().setVisibility(true);
        }
        else
        {
            colourAdjustment.color = new Color32(255,169,0, 255);
            snapReadyPrompt.SetActive(false);
            //GameObject.Find("Snap Ready Prompt").GetComponent<SnapVisibility>().setVisibility(false);
        }
    }
}

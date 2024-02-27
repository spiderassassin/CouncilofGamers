using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdrenalineSlider : MonoBehaviour
{
    public Slider adrenalineBar;
    public Image colourAdjustment;
    
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
        }
        else
        {
            colourAdjustment.color = new Color32(255,169,0, 255);
        }
    }
}

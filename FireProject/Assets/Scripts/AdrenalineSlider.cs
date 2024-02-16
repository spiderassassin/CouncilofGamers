using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdrenalineSlider : MonoBehaviour
{
    public Slider adrenalineBar;
    public int playerAdrenaline;
    private void Start()
    {
        playerAdrenaline = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().adrenaline;
        adrenalineBar = GetComponent<Slider>();
    }
    public void SetAdrenaline(int a)
    {
        adrenalineBar.value = a;
    }

    public void Update()
    {
        adrenalineBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().adrenaline;

        if (adrenalineBar.value >= 100) //bar is full, snap is ready
        {
            GameObject.FindGameObjectWithTag("Adrenaline Bar").GetComponent<Image>().color = Color.red;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Adrenaline Bar").GetComponent<Image>().color = new Color32(255,169,0, 255);
        }
    }
}

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
        playerAdrenaline = GameObject.Find("GameManager").GetComponent<GameManager>().adrenaline;
        adrenalineBar = GetComponent<Slider>();
    }
    public void SetAdrenaline(int a)
    {
        adrenalineBar.value = a;
    }

    public void Update()
    {
        adrenalineBar.value = GameObject.Find("GameManager").GetComponent<GameManager>().adrenaline;

        if (adrenalineBar.value >= GameObject.Find("GameManager").GetComponent<GameManager>().MAX_ADRENALINE)
        {
            GameObject.FindGameObjectWithTag("Adrenaline Bar").GetComponent<Image>().color = Color.white;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Adrenaline Bar").GetComponent<Image>().color = new Color32(255,169,0, 255);
        }
    }
}

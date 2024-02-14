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
    }
}

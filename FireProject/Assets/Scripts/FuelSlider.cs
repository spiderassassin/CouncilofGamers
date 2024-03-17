using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FuelSlider : MonoBehaviour
{
    public Slider fuelBar;
    
    private void Start()
    {
        fuelBar = GetComponent<Slider>();
    }

    public void Update()
    {
        fuelBar.value = GameManager.Instance.FuelPercent;
    }
}

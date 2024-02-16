using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthSlider : MonoBehaviour
{
    public Slider healthBar;
    public float playerHealth;
    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().health;
        healthBar = GetComponent<Slider>();
    }
    public void SetAdrenaline(int a)
    {
        healthBar.value = a;
    }

    public void Update()
    {
        healthBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().health;

    }
}


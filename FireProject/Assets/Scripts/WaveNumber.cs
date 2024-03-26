using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveNumber : MonoBehaviour
{

    public TextMeshProUGUI waveNumberText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waveNumberText.text = GameManager.Instance.gameStage.ToString();
    }
}

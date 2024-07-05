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
        string waveNumber = GameManager.Instance.gameStage.ToString();
        if (waveNumber.Contains("Wave"))
        {
            // Add a space before the number.
            waveNumber = waveNumber.Insert(4, " ");
            waveNumberText.text = waveNumber;
        } else if (waveNumber.Contains("Endless")) {
            waveNumberText.text = "Score: " + GameManager.Instance.score;
        }        
    }
}

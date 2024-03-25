using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueTriggerExplainFireball: MonoBehaviour
{
    public GameObject waveManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (waveManager.GetComponent<WaveManager>().tutorialStage == WaveManager.TutorialStage.PlayerSeesExit)
        {
            Debug.Log("ON TRIGGER ENTER");
            waveManager.GetComponent<WaveManager>().tutorialStage = WaveManager.TutorialStage.TeachFireball;
            Debug.Log(waveManager.GetComponent<WaveManager>().tutorialStage);
        }

    }
}

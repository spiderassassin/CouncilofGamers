using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistMode : MonoBehaviour
{

    public GameObject player;
    public float assistModeHealthRegen;
    public float regularHealthRegen;

    public void Start()
    {
        // Load the assist mode state from the playerprefs.
        bool assistMode = PlayerPrefs.GetInt("AssistMode") == 1;
        ToggleAssistMode(assistMode);
        // Update the toggle component to match the assist mode state.
        GetComponent<UnityEngine.UI.Toggle>().isOn = assistMode;
    }

    public void ToggleAssistMode(bool mode)
    {
        if (mode)
        {
            player.GetComponent<Controller>().healthIncreaseRate = assistModeHealthRegen;
        }
        else
        {
            player.GetComponent<Controller>().healthIncreaseRate = regularHealthRegen;
        }
        // Store the asist mode state in the playerprefs.
        PlayerPrefs.SetInt("AssistMode", mode ? 1 : 0);
    }
}

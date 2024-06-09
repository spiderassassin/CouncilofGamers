using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TextSwitcher : MonoBehaviour
{
    public TextMeshProUGUI continueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            continueText.text = "(Press A to Continue)";
        }
        else
        {
            continueText.text = "(Press Space to Continue)";
        }
    }
}

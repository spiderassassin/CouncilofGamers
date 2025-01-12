using NeKoRoSYS.InputHandling.Mobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-999)]
public class LookInput : MonoBehaviour
{
    public ControlPad controlPad;
    public Text moveText;
    public float scaler = 3f;
    public float rollDecay = 1f;

    Vector2 input;

    private void OnEnable()
    {
        controlPad.OnTouchDrag += Drag;
    }

    private void Update()
    {
        input = Vector2.Lerp(input, Vector2.zero, Time.deltaTime * rollDecay);

        InputManager.Instance.mouseX = input.x;
        InputManager.Instance.mouseY = input.y;

        moveText.text = (input) + "\n" + ((Time.timeSinceLevelLoad * 100) / 100).ToString();
    }

    public void Drag(Vector2 input)
    {
        input *= scaler;
        this.input = input;

    }
}

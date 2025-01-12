using NeKoRoSYS.InputHandling.Mobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class MoveInput : MonoBehaviour
{
    public ControlStick controlStick;

    private void Update()
    {
        Vector2 i = controlStick.input;
        InputManager.Instance.moveX = i.x;
        InputManager.Instance.moveY = i.y;
    }
}

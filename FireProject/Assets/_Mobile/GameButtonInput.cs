using NeKoRoSYS.InputHandling.Mobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonInput : MonoBehaviour
{
    public void FireballInput()
    {
        InputManager.Instance.fireballPerformedThisFrame = true;
    }
    public void Punch()
    {
        InputManager.Instance.punchPerformedThisFrame = true;
    }
    public void Fire()
    {
        InputManager.Instance.fire = !InputManager.Instance.fire;
        InputManager.Instance.stopfire = !InputManager.Instance.fire;
    }
    public void Snap()
    {
        InputManager.Instance.snapPerformedThisFrame = true;
    }
    public void JumpDash()
    {
        InputManager.Instance.jumpPerformedThisFrame = true;
        InputManager.Instance.dash = true;
    }

    private void LateUpdate()
    {
        InputManager.Instance.fireballPerformedThisFrame = false;
        InputManager.Instance.punchPerformedThisFrame = false;
        InputManager.Instance.snapPerformedThisFrame = false;
        InputManager.Instance.jumpPerformedThisFrame = false;
        InputManager.Instance.dash = false;

    }
}

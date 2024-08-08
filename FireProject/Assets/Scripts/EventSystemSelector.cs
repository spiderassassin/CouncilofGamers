using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSelector : MonoBehaviour
{
    public GameObject selection;

    private void OnEnable()
    {
        if(UnityEngine.InputSystem.Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(selection);
        }
        // EventSystem.current.firstSelectedGameObject = selection;
        //EventSystem.current.SetSelectedGameObject(selection);
        // StartCoroutine(Refresh());
    }

    private void Update()
    {
        if (InputManager.Instance)
        {
            if (InputManager.Instance.escapeMenuButtons)
            {
                EventSystem.current.SetSelectedGameObject(selection);
            }
        }
    }
}

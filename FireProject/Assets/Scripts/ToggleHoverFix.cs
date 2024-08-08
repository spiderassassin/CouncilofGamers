using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleHoverFix : MonoBehaviour
{
    // Fix to allow hover to show on toggle button.
    public Toggle toggle;
    public Image imageToAdjust;

    private GameObject old;

    private void OnValidate()
    {
        toggle = gameObject.GetComponent<Toggle>();
    }

    private void Update()
    {
        if (EventSystem.current == null) return;

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (old != null&& current  != old)
        {
            Changed(current);
        }
        old = current;
    }
    private void Changed(GameObject current)
    {
        if (current == toggle.gameObject)
        {
            imageToAdjust.color = toggle.colors.selectedColor;
        }
        else
        {
            imageToAdjust.color = toggle.colors.normalColor;
        }
    }
}

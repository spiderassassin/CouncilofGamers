using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSelector : MonoBehaviour
{
    public GameObject selection;

    private void OnEnable()
    {
        // EventSystem.current.firstSelectedGameObject = selection;
        EventSystem.current.SetSelectedGameObject(selection);
        // StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        GameObject g = EventSystem.current.gameObject;
        g.SetActive(false);
        yield return new WaitForEndOfFrame();
        g.SetActive(true);
    }
}

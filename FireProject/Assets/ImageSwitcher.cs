using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ImageSwitcher : MonoBehaviour
{
    public Sprite defaultImage;
    public Sprite controllerImage;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        /*
        //SwitchImage(true);
        if (GameManager.Instance.usingController)
        {
            SwitchImage(true);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (GameManager.Instance.usingController)
        {
            SwitchImage(true);
        }*/
    }

    public void SwitchImage(bool controller = false)
    {
        if (controller)
        {
            image.GetComponent<Image>().sprite = controllerImage;
        }
        else
        {
            image.GetComponent<Image>().sprite = defaultImage;

        }
    }
    
}

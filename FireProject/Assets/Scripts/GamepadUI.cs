
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamepadUI : MonoBehaviour
{

    public GameObject QImage;
    public GameObject EImage;
    public GameObject LeftClickImage;
    public GameObject RightClickImage;

    public GameObject Flame;
    public GameObject Fireball;

    public GameObject ESCText;
    public GameObject ControllerMenuButton;

    public GameObject DialogueSkipPrompt;
    public GameObject DialogueStartPrompt;

    private bool usingController;
    private bool prevUsingController;

    private Vector3 FireballMousePos;
    private Vector3 FlameMousePos;
    private Vector3 LeftClickMousePos;
    private Vector3 RightClickMousePos;


    // Start is called before the first frame update
    void Start()
    {

        FireballMousePos = Fireball.transform.position;
        FlameMousePos = Flame.transform.position;
        LeftClickMousePos = LeftClickImage.transform.position;
        RightClickMousePos = RightClickImage.transform.position;


        if (GameManager.Instance.usingController)
        {
            //controller values
            setControllerValues();
        }
        else
        {
            //default values
            setMouseValues();
        }
        usingController = GameManager.Instance.usingController;
        prevUsingController = usingController;
    }

    // Update is called once per frame
    void Update()
    {

        usingController = GameManager.Instance.usingController;
        if (usingController != prevUsingController)
        {
            //switch
            if (usingController)
            {
                //controller values
                setControllerValues();
            }
            else
            {
                //defaults
                setMouseValues();
            }
        }


        prevUsingController = usingController;
    }


    void setMouseValues()
    {
        //change prompts to correct image
        QImage.GetComponent<Image>().sprite = QImage.GetComponent<ImageSwitcher>().defaultImage;
        EImage.GetComponent<Image>().sprite = EImage.GetComponent<ImageSwitcher>().defaultImage;

        LeftClickImage.GetComponent<Image>().sprite = LeftClickImage.GetComponent<ImageSwitcher>().defaultImage;
        LeftClickImage.transform.position = LeftClickMousePos;
        Flame.transform.position = FlameMousePos;

        RightClickImage.GetComponent<Image>().sprite = RightClickImage.GetComponent<ImageSwitcher>().defaultImage;
        RightClickImage.transform.position = RightClickMousePos;
        Fireball.transform.position = FireballMousePos;

        //change from esc key text to controller button image
        ESCText.SetActive(true);
        ControllerMenuButton.SetActive(false);

        //change text for dialogue start and dialogue skip
        DialogueSkipPrompt.GetComponent<TextMeshProUGUI>().text = "Press Space to Continue";
        DialogueStartPrompt.GetComponent<TextMeshProUGUI>().text = "Press F to begin conversation";
    }   


    void setControllerValues()
    {
        QImage.GetComponent<Image>().sprite = QImage.GetComponent<ImageSwitcher>().controllerImage;
        EImage.GetComponent<Image>().sprite = EImage.GetComponent<ImageSwitcher>().controllerImage;


        LeftClickImage.GetComponent<Image>().sprite = LeftClickImage.GetComponent<ImageSwitcher>().controllerImage;
        LeftClickImage.transform.position = RightClickMousePos;
        Flame.transform.position = FireballMousePos;

        RightClickImage.GetComponent<Image>().sprite = RightClickImage.GetComponent<ImageSwitcher>().controllerImage;
        RightClickImage.transform.position = LeftClickMousePos;
        Fireball.transform.position = FlameMousePos;

        //change from esc key text to controller button image
        ESCText.SetActive(false);
        ControllerMenuButton.SetActive(true);

        //change text for dialogue start and dialogue skip
        DialogueSkipPrompt.GetComponent<TextMeshProUGUI>().text = "Press A to Continue";
        DialogueStartPrompt.GetComponent<TextMeshProUGUI>().text = "Press Left Bumper to begin conversation";
    }
}

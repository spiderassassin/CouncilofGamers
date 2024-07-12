using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameObject startButton;
    public GameObject settingsButton;
    public GameObject quitButton;
    public GameObject gameLogo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenMenu()
    {
        pauseMenu.SetActive(true);
        startButton.SetActive(false);
        settingsButton.SetActive(false);
        quitButton.SetActive(false);
        gameLogo.SetActive(false);
    }
}

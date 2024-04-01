using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject inputManager;
    public Slider slider;
    
    // Start is called before the first frame update

    private void Awake()
    {
        print("paused");

        //SoundManager.Instance.pause = SoundManager.Instance.CreateInstance(FMODEvents.Instance.pauseMenu);
        
        //SoundManager.Instance.pause.start();
       // SoundManager.Instance.pause.release();

    }

    void Start()
    {
        SetMouseSensitivity();
        //inputManager.GetComponent<InputManager>().mouseSensitivity.x = 0.0f;
        //inputManager.GetComponent<InputManager>().mouseSensitivity.y = 0.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetMouseSensitivity()
    {
        //inputManager.GetComponent<InputManager>().mouseSensitivity.x = slider.value;
        //inputManager.GetComponent<InputManager>().mouseSensitivity.y = slider.value;
        if (slider)
        {
            InputManager.Instance.mouseSensitivity.x = slider.value;
            InputManager.Instance.mouseSensitivity.y = slider.value;
        }
        
    }

    public void SetVolume()
    {
        FMODEvents.Instance.master.setVolume(slider.value);
    }



    public void ResumeGame()
    {
        if (GameManager.Instance.gamePaused)
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.gamePaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            //SoundManager.Instance.pause.stop(STOP_MODE.IMMEDIATE);
            print("done");
            //LockPlayerGameplayInput = false;
            return;
        }
    }

}

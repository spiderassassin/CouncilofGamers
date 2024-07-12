using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Timeline;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject inputManager;
    public Controller controller;
    public bool sensitivity;
    public bool sfx;
    public bool music;

    public GameObject startButton;
    public GameObject settingsButton;
    public GameObject quitButton;
    public GameObject gameLogo;


    public Slider slider;
    
    // Start is called before the first frame update

    private void Awake()
    {

        //SoundManager.Instance.pause = SoundManager.Instance.CreateInstance(FMODEvents.Instance.pauseMenu);
        
        //SoundManager.Instance.pause.start();
       // SoundManager.Instance.pause.release();

    }

    void Start()
    {
        if (sensitivity)
        {
            if (InputManager.Instance)
            {
                slider.value = InputManager.Instance.mouseSensitivity.x;
            }

            if (PlayerPrefs.HasKey("MouseSensitivityX"))
            {
                slider.value = PlayerPrefs.GetFloat("MouseSensitivityX");
            }

        }

        if (music)
        {
            if (FMODEvents.Instance)
            {
                float volume;
                FMODEvents.Instance.music.getVolume(out volume);
                slider.value = volume;
                //SetMouseSensitivity();
                //inputManager.GetComponent<InputManager>().mouseSensitivity.x = 0.0f;
                //inputManager.GetComponent<InputManager>().mouseSensitivity.y = 0.0f;
            }

            if (PlayerPrefs.HasKey("Music"))
            {
                slider.value = PlayerPrefs.GetFloat("Music");
            }


        }
        if (sfx)
        {
            if ((FMODEvents.Instance)){
                float volume;
                FMODEvents.Instance.sfx.getVolume(out volume);
                slider.value = volume;
                //SetMouseSensitivity();
                //inputManager.GetComponent<InputManager>().mouseSensitivity.x = 0.0f;
                //inputManager.GetComponent<InputManager>().mouseSensitivity.y = 0.0f;
            }

            if (PlayerPrefs.HasKey("SFX"))
            {
                slider.value = PlayerPrefs.GetFloat("SFX");
            }



        }
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
            if (InputManager.Instance)
            {
                InputManager.Instance.mouseSensitivity.x = slider.value;
                InputManager.Instance.mouseSensitivity.y = slider.value;
            }
            
            // Store the sensitivity in the playerprefs.
            PlayerPrefs.SetFloat("MouseSensitivityX", slider.value);
        }
        
    }

    public void SetVolume()
    {
        if (FMODEvents.Instance)
        {
            FMODEvents.Instance.master.setVolume(slider.value);
        }
        PlayerPrefs.SetFloat("Volume", slider.value);
        
    }


    public void SetMusic()
    {
        if (FMODEvents.Instance)
        {
            FMODEvents.Instance.music.setVolume(slider.value);
        }

        PlayerPrefs.SetFloat("Music", slider.value);
        
    }


    public void SetSFX()
    {
        if (FMODEvents.Instance)
        {
            FMODEvents.Instance.sfx.setVolume(slider.value);
        }

        PlayerPrefs.SetFloat("SFX", slider.value);
        
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

    public void QuittoMenu()
    {
        Destroy(CombatUI.Instance);

        // Stop all sounds.
        FMODEvents.Instance.StopAllSounds();
        // Destroy all singletons.
        Destroy(SoundManager.Instance.gameObject);
        Destroy(WaveManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
      
        Destroy(CombatUI.Instance.gameObject);
        Destroy(Controller.Instance.gameObject);
        // Re-enable time.
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ReturntoMenu()
    {
        startButton.SetActive(true);
        settingsButton.SetActive(true);
        quitButton.SetActive(true);
        gameLogo.SetActive(true);
        pauseMenu.SetActive(false);
        
        
    }

    public void ResetWave()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Destroy(CombatUI.Instance);
        GameManager.Instance.gamePaused = false;
        Time.timeScale = 1f;
        // Stop all sounds.
        FMODEvents.Instance.StopAllSounds();
        // Destroy all singletons.
        Destroy(SoundManager.Instance.gameObject);
        Destroy(WaveManager.Instance.gameObject);
        Destroy(CombatUI.Instance.gameObject);
        // Reset fuel amount;
        GameManager.Instance.fuel = 100;
        // Restart from the beginning of the current stage.
        GameManager.Instance.gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

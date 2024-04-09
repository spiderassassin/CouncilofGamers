using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject inputManager;
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


    public void SetMusic()
    {
        FMODEvents.Instance.music.setVolume(slider.value);
    }


    public void SetSFX()
    {
        FMODEvents.Instance.sfx.setVolume(slider.value);
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
        Destroy(InputManager.Instance.gameObject);
        Destroy(FMODEvents.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
      
        Destroy(CombatUI.Instance.gameObject);
        Destroy(Controller.Instance.gameObject);
        SceneManager.LoadScene(0);
    }

}

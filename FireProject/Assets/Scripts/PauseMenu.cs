using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject inputManager;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetMouseSensitivity()
    {
        inputManager.GetComponent<InputManager>().mouseSensitivity.x = slider.value;
        inputManager.GetComponent<InputManager>().mouseSensitivity.y = slider.value;
    }

    public void ResumeGame()
    {
        if (GameManager.Instance.gamePaused)
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.gamePaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            //LockPlayerGameplayInput = false;
            return;
        }
    }

}

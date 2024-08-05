using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    public Button endlessButton;
    public float timer;
    public bool trailermode = false;
    public GameObject canvas;
    public GameObject video;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        SoundManager.Instance.menu = SoundManager.Instance.CreateInstance(FMODEvents.Instance.mainMenu);
        SoundManager.Instance.menu.start();
        SoundManager.Instance.menu.release();

        if (PlayerPrefs.GetInt("GameBeaten", 0) == 1) endlessButton.GetComponentInChildren<Button>().interactable = true;

        var highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = $"Highscore: {highscore}";
    }

    // Update is called once per frame
    public void nextScene(bool endless=false)
    {
        SoundManager.Instance.menu.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if (!endless)
        {
            SoundManager.Instance.wave0.start();
            SoundManager.Instance.wave0.release();
        }

        // Set asist mode to off when starting a new game.
        PlayerPrefs.SetInt("AssistMode", 0);
        PlayerPrefs.SetInt("EndlessMode", endless ? 1 : 0);
    }

    // private void Update()
    // {
    //     if (!trailermode)
    //     {
    //         if (!Input.anyKey)
    //         {
    //             timer = timer + Time.deltaTime;
    //         }
    //         else
    //         {
    //             timer = 0f;
    //         }

    //         if(timer > 18f){
    //             video.SetActive(true);
    //         }

    //         if (timer > 20f)
    //         {
    //             timer = 0f;
    //             trailermode = true;
    //             //video.SetActive(true);
    //             canvas.SetActive(false);

    //         }
    //     }

    //     if (trailermode)
    //     {
    //         if (Input.anyKey)
    //         {
    //             trailermode = false;
    //             video.SetActive(false);
    //             canvas.SetActive(true);
    //         }
    //     }
    // }
}

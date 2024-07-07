using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    public Button endlessButton;

    // Start is called before the first frame update
    void Start()
    {
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
}

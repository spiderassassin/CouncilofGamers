using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.menu = SoundManager.Instance.CreateInstance(FMODEvents.Instance.mainMenu);
        SoundManager.Instance.menu.start();
        SoundManager.Instance.menu.release();

        var highscore = PlayerPrefs.GetInt("Highscore", 0);
        var highscoreName = PlayerPrefs.GetString("HighscoreName", "");
        // Format with parentheses.
        if (!string.IsNullOrEmpty(highscoreName)) highscoreName = $"({highscoreName})";

        highscoreText.text = $"Highscore: {highscore}\n{highscoreName}";
    }

    // Update is called once per frame
    public void nextScene()
    {
        SoundManager.Instance.menu.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SoundManager.Instance.wave0.start();
        SoundManager.Instance.wave0.release();

        // Set asist mode to off when starting a new game.
        PlayerPrefs.SetInt("AssistMode", 0);
    }
}

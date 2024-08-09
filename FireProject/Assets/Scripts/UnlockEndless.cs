using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class UnlockEndless : MonoBehaviour
{
    private void Awake()
    {
        if(PlayerPrefs.GetInt("GameBeaten", 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }
    public void UnlockEndlessMode()
    {
        PlayerPrefs.SetInt("GameBeaten", 1);
        SoundManager.Instance.menu.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Destroy(SoundManager.Instance.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

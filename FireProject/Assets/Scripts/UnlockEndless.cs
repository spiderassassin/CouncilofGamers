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
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenText : MonoBehaviour
{
    public int textIndex = 0;
    private bool waitingForInput = false;
    private int fadeDelay = 2;
    public TextMeshProUGUI[] mainText;
    public TextMeshProUGUI continueText;

    
    private void Awake()
    {
        textIndex = 0;
    }
    void Start()
    {
        
        // Set the alpha of all the main text to 0.
        foreach (TextMeshProUGUI text in mainText) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
        // Also disable the continue text.
        ToggleContinueText(false);

        // Fade in the first main text segment.
        StartCoroutine(WaitAndLoadText(textIndex));
        ++textIndex;
    }

    private void Update()
    {
        if (InputManager.Instance.dialogue && waitingForInput)
        {
            // Disable input handling until we've loaded the next text segment.
            waitingForInput = false;
            // If we've reached the end of the main text, load the next scene.
            if (textIndex == mainText.Length) {
                StartCoroutine(WaitAndLoadScene(textIndex));
            } else {
                StartCoroutine(WaitAndLoadText(textIndex));
                ++textIndex;
            }
        }
    }

    private void ToggleContinueText(bool enabled) {
        continueText.enabled = enabled;
    }

    IEnumerator FadeIn(TextMeshProUGUI text) {
        // Set the alpha to 0.
        print("fade in");
        print(text.color.a);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        // Loop until the alpha is 1.
        while (text.color.a < 1.0f) {
            // Increase the alpha by 0.01.
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.01f);
            // Wait for 0.01 seconds.
            yield return new WaitForSeconds(0.01f);
            print(text.color.a);
        }
    }

    IEnumerator FadeOut(TextMeshProUGUI text) {
        // Loop until the alpha is 0.
        while (text.color.a > 0.0f) {
            // Decrease the alpha by 0.01.
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.01f);
            // Wait for 0.01 seconds.
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator WaitAndLoadText(int index) {
        // Fade out the current text segment, if it exists.
        print(index);
        if (index > 0) {
            StartCoroutine(FadeOut(mainText[index - 1]));
            ToggleContinueText(false);
            // Delay for a little...

            yield return new WaitForSeconds(fadeDelay);
        }
        // ...then load the next text segment.
        StartCoroutine(FadeIn(mainText[index]));
        // Delay for a little...
        yield return new WaitForSeconds(fadeDelay);
        // ...then wait for input.
        ToggleContinueText(true);
        waitingForInput = true;
    }

    IEnumerator WaitAndLoadScene(int index) {
        // Fade out the current text segment.
        StartCoroutine(FadeOut(mainText[index - 1]));
        ToggleContinueText(false);
        // Delay for a little...
        yield return new WaitForSeconds(fadeDelay);
        // ...then load the next scene...
        // ...but if it's the last one, restart the game instead.
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
            SceneManager.LoadScene(0);
        } else {
            // Make sure the next scene is positioned after the current one in the build settings!
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

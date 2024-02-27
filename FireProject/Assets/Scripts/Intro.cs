using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    private int textIndex = 0;
    private bool waitingForInput = false;
    private int fadeDelay = 2;
    public TextMeshProUGUI[] introText;
    public TextMeshProUGUI continueText;

    // Start is called before the first frame update
    void Start()
    {
        // Set the alpha of all the intro text to 0.
        foreach (TextMeshProUGUI text in introText) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
        // Also disable the continue text.
        ToggleContinueText(false);

        // Fade in the first intro text segment.
        StartCoroutine(WaitAndLoadText(textIndex));
        ++textIndex;
    }

    private void Update()
    {
        if (InputManager.Instance.dialogue && waitingForInput)
        {
            // Disable input handling until we've loaded the next text segment.
            waitingForInput = false;
            // If we've reached the end of the intro text, load the next scene.
            if (textIndex == introText.Length) {
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
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        // Loop until the alpha is 1.
        while (text.color.a < 1.0f) {
            // Increase the alpha by 0.01.
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.01f);
            // Wait for 0.01 seconds.
            yield return new WaitForSeconds(0.01f);
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
        if (index > 0) {
            StartCoroutine(FadeOut(introText[index - 1]));
            ToggleContinueText(false);
            // Delay for a little...
            yield return new WaitForSeconds(fadeDelay);
        }
        // ...then load the next text segment.
        StartCoroutine(FadeIn(introText[index]));
        // Delay for a little...
        yield return new WaitForSeconds(fadeDelay);
        // ...then wait for input.
        ToggleContinueText(true);
        waitingForInput = true;
    }

    IEnumerator WaitAndLoadScene(int index) {
        // Fade out the current text segment.
        StartCoroutine(FadeOut(introText[index - 1]));
        ToggleContinueText(false);
        // Delay for a little...
        yield return new WaitForSeconds(fadeDelay);
        // ...then load the next scene.
        // Make sure the next scene is positioned after the current one in the build settings!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

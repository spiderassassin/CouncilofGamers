using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public TextMeshProUGUI continueText;

    // Start is called before the first frame update
    void Start()
    {
        // Fade in the intro text.
        StartCoroutine(FadeIn(introText));
        StartCoroutine(FadeIn(continueText));
    }

    public void Play() {
        // Fade out the intro text.
        StartCoroutine(FadeOut(introText));
        StartCoroutine(FadeOut(continueText));
        // Wait until it fades out completely before moving on.
        StartCoroutine(WaitAndLoad());
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

    IEnumerator WaitAndLoad() {
        // Delay for a little...
        yield return new WaitForSeconds(2);
        // ...then load the next scene.
        // Make sure the next scene is positioned after the current one in the build settings!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class TextBlock
{
    public string role;
    public string[] names;
}

public class Ending : MonoBehaviour
{
    private int textIndex = 0;
    private int fadeDelay = 2;
    private bool waitingForText = false;
    private bool initialDelay = true;
    public TextBlock[] textBlocks;
    public TextMeshProUGUI roleText;
    public TextMeshProUGUI nameText;
    public int introDelay = 2;
    public int textDuration = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Set the alpha of all the text to 0.
        roleText.color = new Color(roleText.color.r, roleText.color.g, roleText.color.b, 0);
        nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 0);
    }

    private void Update()
    {
        if (GameManager.Instance.gameStage == GameManager.GameStage.Ending && !waitingForText)
        {
            if (initialDelay) {
                // First time, wait a bit before starting the credits.
                waitingForText = true;
                StartCoroutine(WaitForText(introDelay));
            } else if (textIndex != textBlocks.Length) {
                waitingForText = true;
                StartCoroutine(WaitAndLoadText(textIndex));
                ++textIndex;
            }
        }
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
            StartCoroutine(FadeOut(roleText));
            StartCoroutine(FadeOut(nameText));
            // Delay for a little...
            yield return new WaitForSeconds(fadeDelay);
        }
        // ...then load the next text segment.
        roleText.text = textBlocks[index].role;
        // Add newlines manually to the names.
        nameText.text = "";
        for (int i = 0; i < textBlocks[index].names.Length; ++i) {
            nameText.text += textBlocks[index].names[i] + "\n";
        }
        StartCoroutine(FadeIn(roleText));
        StartCoroutine(FadeIn(nameText));
        // Delay for a little...
        yield return new WaitForSeconds(fadeDelay);

        StartCoroutine(WaitForText(textDuration));

        if (index == textBlocks.Length - 1) {
            // Increment the completion counter.
            ++GameManager.Instance.gameEndCompletion;
        }
    }

    IEnumerator WaitForText(int seconds) {
        // Keep the text on the screen for a bit.
        yield return new WaitForSeconds(seconds);
        waitingForText = false;
        initialDelay = false;
    }
}

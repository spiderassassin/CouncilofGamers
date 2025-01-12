using UnityEngine;
using UnityEngine.UI; // Include this if you use a UI Text element

public class FPSDebug : MonoBehaviour
{
    public Text fpsText; // Assign a UI Text element in the Inspector

    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (fpsText == null)
        {
            // Fallback: Display FPS using OnGUI if no UI Text is assigned
            int width = Screen.width, height = Screen.height;
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(250, 250, width, height * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = height * 2 / 100;
            style.normal.textColor = Color.white;

            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.} FPS", fps);
            GUI.Label(rect, text, style);
        }
        else
        {
            // Update the UI Text element with the FPS value
            float fps = 1.0f / deltaTime;
            fpsText.text = string.Format("{0:0.} FPS", fps);
        }
    }
}

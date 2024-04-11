using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpritePlayer : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public Sprite[] sprites;
    public float delay = 0.25f;
    public bool faceCamera = false;

    int i = 0;
    float timer = 0;
    Camera c;

    private void OnEnable()
    {
        renderer.sprite = sprites[i];
        if (faceCamera) c = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            i = (i + 1) % sprites.Length;
            renderer.sprite = sprites[i];
            timer = 0;
        }

        if (faceCamera)
        {
            transform.LookAt(c.transform);
            transform.Rotate(0, 180, 0);
            transform.Rotate(0, 0, Time.timeSinceLevelLoad * 100f, Space.Self);
        }
    }
}

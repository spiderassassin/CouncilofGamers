using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpritePlayer : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public Sprite[] sprites;
    public float delay = 0.25f;

    int i = 0;
    float timer = 0;

    private void OnEnable()
    {
        renderer.sprite = sprites[i];
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
    }
}

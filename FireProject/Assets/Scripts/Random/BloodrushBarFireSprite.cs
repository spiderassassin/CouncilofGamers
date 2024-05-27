using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodrushBarFireSprite : MonoBehaviour
{
    //public new SpriteRenderer renderer;
    public Sprite[] sprites;
    public Image image;
    public float delay = 0.25f;
    public bool faceCamera = false;
    public Controller controller;

    int i = 0;
    float timer = 0;
    Camera c;

    private void Start()
    {
        image.sprite = sprites[i];
        if (faceCamera) c = Camera.main;
    }

    private void Update()
    {
       if (GameManager.Instance.AdrenalinePercent>=1f)
        {
            
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                i = (i + 1) % sprites.Length;
                image.sprite = sprites[i];
                timer = 0;
            }

            if (faceCamera)
            {
                transform.LookAt(c.transform);
                transform.Rotate(0, 180, 0);
                transform.Rotate(0, 0, Time.timeSinceLevelLoad * 100f, Space.Self);
            }
            Color newColor = new Color32(0, 255, 255, 255);
            image.color = newColor;
        }
        else
        {
            Color newColor = new Color32(0, 255, 255, 0);
            image.color = newColor;
            timer = 0;
            i = 0;
        }
        
    }
}

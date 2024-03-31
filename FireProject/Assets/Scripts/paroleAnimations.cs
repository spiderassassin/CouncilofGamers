using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paroleAnimations : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.gameStage == GameManager.GameStage.Wave3)
        {
            betray();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  hide()
    {
        animator.SetBool("wavemode", true);
    }

    public void unhide()
    {
        animator.SetBool("wavemode", false);
    }

    public void betray()
    {
        animator.SetBool("betrayed", true);
    }

    public void inquisitive()
    {
        animator.SetBool("inquisitive", true);
    }


    public void notinquisitive()
    {
        animator.SetBool("inquisitive", false);
    }
}

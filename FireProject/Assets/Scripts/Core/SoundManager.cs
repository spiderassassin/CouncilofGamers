using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip playerwalk;
    public AudioClip playerrun;
    public AudioClip jump;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Play(AudioClip clip, AudioSource source)
    {

        if (!(source.clip == clip && source.isPlaying == true))
        {
            print(clip);
            source.Stop();
            source.clip = clip;
            source.Play();
        }
    }
    
}

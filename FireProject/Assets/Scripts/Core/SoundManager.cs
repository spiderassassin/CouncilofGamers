using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public AudioClip jump;

    public AudioSource music;
    public AudioClip paroleDialogue;

    public AudioClip playerwalk;
    public AudioClip playerrun;
    public GameObject audioSource;

    //Event Instances
    public EventInstance wave0;
    public EventInstance wave1;
    public EventInstance wave2;
    public EventInstance wave3;

    public EventInstance hello;
    public EventInstance hhhh;
    public EventInstance downtime1;
    public EventInstance downtime2;
    public EventInstance betrayal;



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

    private void Start()
    {
        wave0 = CreateInstance(FMODEvents.Instance.wave0);
        wave1 = CreateInstance(FMODEvents.Instance.wave1);
        wave2 = CreateInstance(FMODEvents.Instance.wave2);
        wave3 = CreateInstance(FMODEvents.Instance.wave3);

        hello = CreateInstance(FMODEvents.Instance.hello);
        hhhh  = CreateInstance(FMODEvents.Instance.hhhh);

        downtime1 = CreateInstance(FMODEvents.Instance.downtime1);
        downtime2 = CreateInstance(FMODEvents.Instance.downtime2);
        betrayal = CreateInstance(FMODEvents.Instance.betrayal);

    }

    private void Update()
    {

        
    }

    public void DowntimeMusicPlay()
    {
        switch (GameManager.Instance.gameStage)
        {
            case GameManager.GameStage.Downtime1:
                downtime1.start();
                break;
            case GameManager.GameStage.Downtime2:
                downtime2.start();
                break;

        }
    }

    public void DowntimeMusicStop()
    {
        switch (GameManager.Instance.gameStage)
        {
            case GameManager.GameStage.Downtime1:
                downtime1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case GameManager.GameStage.Downtime2:
                downtime2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;

        }
    }


    public void WaveMusicPlay()
    {
        switch (GameManager.Instance.gameStage)
        {
            case GameManager.GameStage.Wave1:
                wave1.start();
                break;

            case GameManager.GameStage.Wave2:
                wave2.start();
                break;

            case GameManager.GameStage.Wave3:
                wave3.start();
                break;

        }
    }
    public void WaveMusicStop()
    {
        switch (GameManager.Instance.gameStage)
        {
            case GameManager.GameStage.Wave1:
                wave1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case GameManager.GameStage.Wave2:
                wave2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case GameManager.GameStage.Wave3:
                wave3.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
        }
    }



    public GameObject PlaySoundloop(AudioClip clip, Transform parent)
    {
        GameObject obj = Instantiate(audioSource);
        obj.transform.position = parent.position;
        obj.transform.SetParent(parent);
        obj.GetComponent<AudioSource>().loop = true;
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().Play();
        return obj;
        
    }

    public void PlaySoundOnce(AudioClip clip, Transform parent)
    {
        if (clip == null) return;
        GameObject obj = Instantiate(audioSource);
        obj.transform.position = parent.position;
        obj.transform.SetParent(parent);
        obj.GetComponent<AudioSource>().loop = false;
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().Play();
        StartCoroutine(Buffer(obj, clip.length));
    }
    public void PlaySoundOnce(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;
        GameObject obj = Instantiate(audioSource);
        obj.transform.position = position;
        obj.GetComponent<AudioSource>().loop = false;
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().Play();
        StartCoroutine(Buffer(obj, clip.length));
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }



    public IEnumerator Buffer(GameObject obj, float len)
    {
        yield return new WaitForSeconds(len);
        StopSoundEffect(obj);


    }

    public void StopSoundEffect(GameObject obj)
    {
        Destroy(obj);
    }

    public void MusicChange(AudioClip clip)
    {
        music.Stop();
        music.clip = clip;
        music.Play();
    }

    public void MusicStop()
    {
        music.Pause(); 
    }
    public void MusicPlay()
    {
        music.Play();
    }
   



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public AudioClip jump;

    public AudioSource music;
    public AudioClip paroleDialogue;

    public AudioClip playerwalk;
    public AudioClip playerrun;
    public GameObject audioSource;



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

    private void Update()
    {

        
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

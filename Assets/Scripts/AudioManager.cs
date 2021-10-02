using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource audioSourceHitSound;
    public AudioSource audioSourceMusic;

    //REMOVEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
    public float test;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayHitSound()
    {
        audioSourceHitSound.Play();
    }

    public void StopHitSound()
    {
        audioSourceHitSound.Stop();
    }

    public void PlayMusic()
    {
        audioSourceMusic.Play();
    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }
}

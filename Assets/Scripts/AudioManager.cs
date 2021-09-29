using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}

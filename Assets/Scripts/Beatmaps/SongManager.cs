using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SongManager : Singleton<SongManager>
{
    public AudioSource music;
    public AudioSource hitSound;

    public float firstBeatOffset;
    public float songBPM;

    private new void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayHitSound()
    {
        hitSound.Play();
    }

    public void StopHitSound()
    {
        hitSound.Stop();
    }

    public void PlayMusic()
    {
        music.Play();
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void ResetMusic()
    {
        music.time = 0;
    }
}

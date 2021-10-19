using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource mapMusic;
    public AudioSource hitSound;

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
        mapMusic.Play();
    }

    public void StopMusic()
    {
        mapMusic.Stop();
    }
}

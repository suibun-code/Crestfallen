using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource audioSourceHitSound;

    public void PlayHitSound()
    {
        audioSourceHitSound.Play();
    }

    public void StopHitSound()
    {
        audioSourceHitSound.Stop();
    }
}

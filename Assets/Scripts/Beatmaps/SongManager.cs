using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SongManager : Singleton<SongManager>
{
    public BeatmapCell currentBeatmap;

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
        if (hitSound != null)
            hitSound.Play();
    }

    public void StopHitSound()
    {
        if (hitSound != null)
            hitSound.Stop();
    }

    public void PlayMusic()
    {
        if (music != null)
            music.Play();
    }

    public void StopMusic()
    {
        if (music != null)
            music.Stop();
    }

    public void ResetMusic()
    {
        if (music != null)
            music.time = 0;
    }

    public float GetPitch()
    {
        if (music != null)
        {
            return music.pitch;
        }
        else
        {
            Debug.Log("No music!");
            return 0;
        }
    }
}

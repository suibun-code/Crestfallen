using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SongManager : Singleton<SongManager>
{
    public delegate void NewSong();
    public static event NewSong OnNewSong;

    public Beatmap beatmap;

    public AudioSource music;
    public AudioSource hitSound;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    //Start music from a random loaded beatmap. Called after all beatmaps loaded.
    public void SelectRandomSongAndPlay()
    {
        //return if no beatmaps to select from
        if (TrackLoader.instance.beatmaps.Count <= 0) return;

        int r = Random.Range(0, TrackLoader.instance.beatmaps.Count);

        beatmap = TrackLoader.instance.beatmaps[r];

        PlayMusic();
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
        music.clip = beatmap.clip;

        if (OnNewSong != null)
            OnNewSong();

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

    public void PauseMusic()
    {
        if (music != null)
            music.Pause();
    }

    public void ResumeMusic()
    {
        if (music != null)
            music.Play();
    }    

    public void SetMusic(AudioClip newClip)
    {
        if (music != null)
        {
            music.clip = newClip;
            ResetMusic();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SongManager : Singleton<SongManager>
{
    public delegate void NewSong();
    public static event NewSong OnNewSong;

    public AudioSource music;
    public AudioSource hitSound;

    public float firstBeatOffset;
    public float songBPM;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    //Start music from a random loaded beatmap. Called after all beatmaps loaded.
    public void SelectRandomSongAndPlay()
    {
        int r = Random.Range(0, TrackLoader.instance.beatmaps.Count);
        music.clip = TrackLoader.instance.beatmaps[r].music;
        music.clip.name = TrackLoader.instance.beatmaps[r].audioFileName;
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
        if (music != null)
            music.Play();

        if (OnNewSong != null)
            OnNewSong();
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

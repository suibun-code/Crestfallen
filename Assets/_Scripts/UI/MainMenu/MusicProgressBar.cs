using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicProgressBar : MonoBehaviour
{
    Slider songProgressSlider;

    private void OnEnable()
    {
        SongManager.OnNewSong += SetMusicTime;
    }

    private void OnDisable()
    {
        SongManager.OnNewSong -= SetMusicTime;
    }

    private void Start()
    {
        songProgressSlider = GetComponent<Slider>();

        if (SongManager.instance.music.clip != null)
        {
            songProgressSlider.minValue = 0;
            songProgressSlider.maxValue = SongManager.instance.music.clip.length;
        }
    }

    private void Update()
    {
        if (SongManager.instance.music.clip != null)
            songProgressSlider.value = SongManager.instance.music.time;
    }

    public void SetNewMusicTime(float newTime)
    {
        //if (SongManager.instance.music.clip == null)
        //    return;

        //Debug.Log("NEW MUSIC TIME");

        //if (SongManager.instance.music.isPlaying)
        //    SongManager.instance.music.time = 
        //        Mathf.Min(newTime, SongManager.instance.music.clip.length);
    }

    public void SetMusicTime()
    {
        if (SongManager.instance.music.clip == null)
            return;

        Debug.Log("MUSIC TIME");

        songProgressSlider.maxValue = SongManager.instance.music.clip.length;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private bool sliderDrag = false;
    private float newTime;

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
        slider = GetComponent<Slider>();

        if (SongManager.instance.music.clip != null)
        {
            slider.minValue = 0;
            slider.maxValue = SongManager.instance.music.clip.length;
        }
    }

    private void Update()
    {
        /*
        Move the slider forward according to audio's current time.
        When the slider is dragged, this pauses so the user can drag the slider.
        */
        if (sliderDrag == false)
            if (SongManager.instance.music.clip != null)
                slider.value = SongManager.instance.music.time;
    }

    public void SetMusicTime()
    {
        if (SongManager.instance.music.clip == null)
            return;

        Debug.Log("MUSIC TIME");

        slider.maxValue = SongManager.instance.music.clip.length;
    }

    public void OnBeginDrag()
    {
        sliderDrag = true;
    }

    public void OnEndDrag()
    {
        sliderDrag = false;

        SongManager.instance.music.time = slider.value;
    }

    public void OnPointerDown()
    {
        newTime = slider.value;
    }

    public void OnPointerUp()
    {
        SongManager.instance.music.time = newTime;
    }
}
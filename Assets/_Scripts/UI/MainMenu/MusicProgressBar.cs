using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicProgressBar : MonoBehaviour
{
    Slider songProgressSlider;

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
        if (SongManager.instance.music.clip == null)
            return;

        if (SongManager.instance.music.isPlaying)
            SongManager.instance.music.time = Mathf.Min(newTime, SongManager.instance.music.clip.length);
    }
}

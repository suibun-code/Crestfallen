using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public Beatmap beatmap;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        var songText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        var artistText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        var difficultyText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        if (beatmap == null)
        {
            Debug.Log("No beatmap!");
            return;
        }

        image.sprite = beatmap.artwork;
        songText.SetText(beatmap.songName);
        artistText.SetText(beatmap.songArtist);

        if (beatmap.difficulty < 10)
            difficultyText.SetText("0" + beatmap.difficulty.ToString());
        else
            difficultyText.SetText(beatmap.difficulty.ToString());

        if (beatmap.difficulty <= 3)
            difficultyText.color = new Color(0f, 0.6980392f, 0f);
        else if (beatmap.difficulty <= 6 && beatmap.difficulty > 3)
            difficultyText.color = new Color(0f, 0.6980392f, 1f);
        else if (beatmap.difficulty <= 9 && beatmap.difficulty > 6)
            difficultyText.color = new Color(0.6980392f, 0.6980392f, 0f);
        else if (beatmap.difficulty <= 12 && beatmap.difficulty > 9)
            difficultyText.color = new Color(0.6980392f, 0f, 0f);
    }

    public void PreviewSong()
    {
        if (beatmap == null)
            return;

        SongPreview.instance.music.clip = beatmap.music;
        SongPreview.instance.music.time = beatmap.previewTimeStart;
        SongPreview.instance.music.Play();
    }
}
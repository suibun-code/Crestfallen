using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BeatmapCell : MonoBehaviour
{
    public Beatmap beatmap;
    Image image;

    private TextEffect textEffect;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        if (beatmap == null)
        {
            Debug.Log("No beatmap!");
            return;
        }

        //After the first child (which is the difficulty bg) is song text, then artist text, then finally difficulty text.
        var songText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        var artistText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        var difficultyText = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textEffect = difficultyText.gameObject.GetComponent<TextEffect>();

        //Set parameters from beatmap scriptable object to this gameobject.
        //image.sprite = beatmap.artwork;

        //StartCoroutine(LoadFile.LoadImage(image.sprite.texture, beatmap.relativePath + beatmap.artName));
        songText.SetText(beatmap.songName);
        artistText.SetText(beatmap.songArtist);

        //Add a 0 before the difficulty if the difficulty is a single digit
        if (beatmap.difficulty < 10)
            difficultyText.SetText("0" + beatmap.difficulty.ToString());
        else
            difficultyText.SetText(beatmap.difficulty.ToString());

        //Coloring the difficulty text depending on the difficulty
        if (beatmap.difficulty <= 3)
            difficultyText.color = SongSelectManager.instance.Tier0;

        else if (beatmap.difficulty <= 7 && beatmap.difficulty > 3)
            difficultyText.color = SongSelectManager.instance.Tier1;

        else if (beatmap.difficulty <= 11 && beatmap.difficulty > 7)
            difficultyText.color = SongSelectManager.instance.Tier2;

        else if (beatmap.difficulty <= 15 && beatmap.difficulty > 11)
            difficultyText.color = SongSelectManager.instance.Tier3;

        else if (beatmap.difficulty <= 19)
            difficultyText.color = SongSelectManager.instance.Tier4;

        else
            textEffect.enabled = true;
    }

    public void PreviewSong()
    {
        if (beatmap == null)
            return;
        
        SongManager songManager = SongManager.instance;

        //Check if the same cel has been clicked twice. If it has, change to gameplay scene
        if (songManager.currentBeatmap == this)
        {
            songManager.songBPM = beatmap.songBPM;
            songManager.firstBeatOffset = beatmap.firstBeatOffset;

            SceneManager.instance.ChangeScene("Gameplay");
            songManager.StopMusic();
            songManager.PlayHitSound();
            return;
        }
        else
        {
            //Use to track if the same song has been clicked twice
            SongManager.instance.currentBeatmap = this;
        }

        SongSelectManager.instance.SetBigArt(beatmap.art);

        songManager.music.clip = beatmap.music;
        songManager.music.time = beatmap.previewStartTime;
        songManager.PlayMusic();
    }
}
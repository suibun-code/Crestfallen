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

    public Color Tier0 = new Color(0f, 0.6980392f, 0f);
    public Color Tier1 = new Color(0f, 0.6980392f, 1f);
    public Color Tier2 = new Color(0.6980392f, 0.6980392f, 0f);
    public Color Tier3 = new Color(0.6980392f, 0f, 0f);
    public Color Tier4 = new Color(0.6980392f, 0f, 0.6980392f);

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
            difficultyText.color = Tier0;
        else if (beatmap.difficulty <= 7 && beatmap.difficulty > 3)
            difficultyText.color = Tier1;
        else if (beatmap.difficulty <= 11 && beatmap.difficulty > 7)
            difficultyText.color = Tier2;
        else if (beatmap.difficulty <= 15 && beatmap.difficulty > 11)
            difficultyText.color = Tier3;
        else if (beatmap.difficulty <= 19)
            difficultyText.color = Tier4;
        else
            textEffect.enabled = true;
    }

    public void PreviewSong()
    {
        if (beatmap == null)
            return;

        SongSelectManager.instance.SetBigArt(beatmap.art);

        var songManager = SongManager.instance;
        songManager.songBPM = beatmap.songBPM;
        songManager.firstBeatOffset = beatmap.firstBeatOffset;
        songManager.music.clip = beatmap.music;
        songManager.music.time = beatmap.previewStartTime;
        songManager.music.Play();

        //Check if the same cel has been clicked twice. If it has, change to gameplay scene
        if (SongManager.instance.currentBeatmap == this)
        {
            SceneManager.instance.ChangeSceneToGameplay();
            return;
        }
        else
        {
            //Use to track if the same song has been clicked twice
            SongManager.instance.currentBeatmap = this;
        }
    }
}
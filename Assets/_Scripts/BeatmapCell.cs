using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BeatmapCell : MonoBehaviour
{
    public delegate void ClickAction(BeatmapCell beatmapCell);
    public static event ClickAction OnClick;

    public Beatmap beatmap;
    public TextMeshProUGUI songText;
    public TextMeshProUGUI artistText;
    public TextMeshProUGUI difficultyText;
    public TextEffect textEffect;

    void Start()
    {
        if (beatmap == null)
        {
            Debug.Log("No beatmap!");
            return;
        }

        //Set parameters from beatmap scriptable object to this gameobject.
        songText.SetText(beatmap.songName);
        artistText.SetText(beatmap.songArtist);
        SetDifficultyTextAndColor();
    }

    private void SetDifficultyTextAndColor()
    {
        //Add a 0 before the difficulty if the difficulty is a single digit
        if (beatmap.difficulty < 10)
            difficultyText.SetText("0" + beatmap.difficulty.ToString());
        else
            difficultyText.SetText(beatmap.difficulty.ToString());

        //Coloring the difficulty text depending on the difficulty
        if (beatmap.difficulty <= 3)
            difficultyText.color = GameColors.instance.Tier0;

        else if (beatmap.difficulty <= 7 && beatmap.difficulty > 3)
            difficultyText.color = GameColors.instance.Tier1;

        else if (beatmap.difficulty <= 11 && beatmap.difficulty > 7)
            difficultyText.color = GameColors.instance.Tier2;

        else if (beatmap.difficulty <= 15 && beatmap.difficulty > 11)
            difficultyText.color = GameColors.instance.Tier3;

        else if (beatmap.difficulty <= 19)
            difficultyText.color = GameColors.instance.Tier4;

        else
            textEffect.enabled = true;
    }

    public void OnPreviewSong()
    {
        if (OnClick != null)
            OnClick(this);
    }
}
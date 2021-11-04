using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatmapCell : MonoBehaviour
{
    public Beatmap beatmap;
    Image image;

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
        var difficultyText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        //Set parameters from beatmap scriptable object to this gameobject.
        image.sprite = beatmap.artwork;
        songText.SetText(beatmap.songName);
        artistText.SetText(beatmap.songArtist);

        //Add a 0 before the difficulty if the difficulty is a single digit
        if (beatmap.difficulty < 10)
            difficultyText.SetText("0" + beatmap.difficulty.ToString());
        else
            difficultyText.SetText(beatmap.difficulty.ToString());

        //Coloring the difficulty text depending on the difficulty
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

        var songManager = SongManager.instance;
        songManager.songBPM = beatmap.songBPM;
        songManager.firstBeatOffset = beatmap.firstBeatOffset;
        songManager.music.clip = beatmap.music;
        songManager.music.time = beatmap.previewTimeStart;
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
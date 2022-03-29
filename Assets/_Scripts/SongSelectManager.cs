using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SongSelectManager : MonoBehaviour
{
    public int cellsPerHorizontal;
    private int cellCount = 0;

    public Transform parent;
    public GameObject pf_horizontalPanel;
    public GameObject pf_beatmapCell;
    public GameObject currentHorizontalPanel;
    
    public RawImage bigArt;
    public TextMeshProUGUI beatmapName;
    public TextMeshProUGUI beatmapBPM;

    private void OnEnable() 
    {
        TrackLoader.onLoadedNewFile += LoadBeatmaps;
        BeatmapCell.OnClick += PreviewSong;
    }

    private void OnDisable()
    {
        TrackLoader.onLoadedNewFile -= LoadBeatmaps;
        BeatmapCell.OnClick -= PreviewSong;
    }

    void Start()
    {
        LoadBeatmaps();
    }

    public void LoadBeatmaps()
    {
        foreach (Beatmap beatmap in TrackLoader.instance.beatmaps)
        {
            if (cellCount == cellsPerHorizontal)
            {
                currentHorizontalPanel = Instantiate(pf_horizontalPanel);
                currentHorizontalPanel.transform.SetParent(parent, false);
                cellCount = 0;
            }

            var beatmapCell = Instantiate(pf_beatmapCell);

            beatmapCell.GetComponent<BeatmapCell>().beatmap = beatmap;
            beatmapCell.transform.SetParent(currentHorizontalPanel.transform, false);
            beatmapCell.GetComponent<RawImage>().texture = beatmap.art;

            cellCount++;
        }

        TrackLoader.instance.beatmaps.Clear();
    }

    public void PreviewSong(BeatmapCell beatmapCell)
    {
        if (beatmapCell.beatmap == null)
            return;

        SongManager songManager = SongManager.instance;
        Beatmap beatmap = beatmapCell.beatmap;

        //Check if the same cel has been clicked twice. If it has, change to gameplay scene
        if (songManager.currentBeatmapCell == beatmapCell)
        {
            songManager.songBPM = beatmap.songBPM;
            songManager.firstBeatOffset = beatmap.firstBeatOffset;

            SceneManager.instance.ChangeScene("Gameplay");
            songManager.StopMusic();

            //PLAY SOME SOUND EFFECT HERE INSTEAD OF HITSOUND
            songManager.PlayHitSound();
            return;
        }
        else
        {
            //Use to track if the same song has been clicked twice
            SongManager.instance.currentBeatmapCell = beatmapCell;
        }

        bigArt.texture = beatmap.art;
        beatmapName.SetText(beatmap.songName);
        beatmapBPM.SetText("BPM: " + beatmap.songBPM.ToString());

        //Set the song to the beatmap's song, and start it at the preview start time.
        songManager.music.clip = beatmap.music;
        songManager.music.time = beatmap.previewStartTime;
        songManager.PlayMusic();
    }

    public void OnEscape(InputValue value)
    {
        SceneManager.instance.ChangeScene("MainMenu");
    }
}

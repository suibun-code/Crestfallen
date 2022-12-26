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

    [SerializeField] private BeatmapCell currentBeatmapCell;

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
        Debug.Log("LOADING BEATMAPS");

        foreach (Beatmap beatmap in TrackLoader.instance.beatmaps)
        {
            //Create new row of songs when the horizontal size is reached
            //Set new row to current horizontal panel
            if (cellCount == cellsPerHorizontal)
            {
                currentHorizontalPanel = Instantiate(pf_horizontalPanel);
                currentHorizontalPanel.transform.SetParent(parent, false);
                cellCount = 0;
            }

            //Create new beatmap UI item, assign properties to reflect what the beatmap should have
            GameObject beatmapCell = Instantiate(pf_beatmapCell);
            beatmapCell.GetComponent<BeatmapCell>().beatmap = beatmap;
            beatmapCell.transform.SetParent(currentHorizontalPanel.transform, false);
            beatmapCell.GetComponent<RawImage>().texture = beatmap.art;

            cellCount++;
        }

        //TrackLoader.instance.beatmaps.Clear();
    }

    public void PreviewSong(BeatmapCell beatmapCell)
    {
        if (beatmapCell.beatmap == null)
        {
            Debug.LogError("No beatmap found!");
            return;
        }

        SongManager songManager = SongManager.instance;
        Beatmap beatmap = beatmapCell.beatmap;

        songManager.beatmap = beatmap;

        //Check if the same cel has been clicked twice. If it has, change to gameplay scene
        if (currentBeatmapCell == beatmapCell)
        {
            SceneManager.instance.ChangeScene("Gameplay");
            songManager.StopMusic();

            //PLAY SOME SOUND EFFECT HERE INSTEAD OF HITSOUND
            songManager.PlayHitSound();
            return;
        }
        else
        {
            //Use to track if the same song has been clicked twice
            currentBeatmapCell = beatmapCell;
        }

        //set the song select screen UI info to match the clicked beatmap
        bigArt.texture = beatmap.art;
        beatmapName.SetText(beatmap.songName);
        beatmapBPM.SetText("BPM: " + beatmap.songBPM.ToString());

        songManager.PlayMusic();
    }

    public void OnEscape(InputValue value)
    {
        //Switch menu, print debug error if scene from string not found
        MenuManager.instance.SwitchMenu("///MainMenu (Scene)");
    }
}

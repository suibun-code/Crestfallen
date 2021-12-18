using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SongSelectManager : Singleton<SongSelectManager>
{
    public int cellsPerHorizontal;
    private int cellCount = 0;

    public Transform parent;
    public GameObject pf_horizontalPanel;
    public GameObject pf_beatmapCell;
    public GameObject currentHorizontalPanel;

    public Color Tier0;
    public Color Tier1;
    public Color Tier2;
    public Color Tier3;
    public Color Tier4;
    
    public RawImage bigArt;

    void OnEnable() 
    {
        TrackLoader.onLoadedNewFile += LoadBeatmaps;
    }

    void Start()
    {
        LoadBeatmaps();
    }

    public void SetBigArt(Texture texture)
    {
        bigArt.texture = texture;
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

    public void OnEscape(InputValue value)
    {
        SceneManager.instance.ChangeScene("MainMenu");
    }
}

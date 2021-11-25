using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectManager : Singleton<SongSelectManager>
{
    public int cellsPerHorizontal;
    private int cellCount = 0;

    public Transform parent;
    public GameObject pf_horizontalPanel;
    public GameObject pf_beatmapCell;
    public GameObject currentHorizontalPanel;
    
    public RawImage bigArt;

    void Start()
    {
        LoadBeatmaps();
        TrackLoader.instance.LoadTracks(true);
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
}

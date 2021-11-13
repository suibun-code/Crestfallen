using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectManager : Singleton<SongSelectManager>
{
    public Transform parent;
    public GameObject pf_horizontalPanel;
    public GameObject pf_beatmapCell;

    private GameObject currentHorizontalPanel;
    private int cellCount;

    public void LoadBeatmaps()
    {
        cellCount = 0;
        var currentHorizontalPanel = Instantiate(pf_horizontalPanel);
        currentHorizontalPanel.transform.SetParent(parent, false);

        Debug.Log(TrackLoader.instance.beatmaps.Count);

        foreach (Beatmap beatmap in TrackLoader.instance.beatmaps)
        {
            if (cellCount == 4)
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
    }
}

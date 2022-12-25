using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject pf_timelineBeat;

    public void InitTimeline()
    {
        float realSongDuration = SongManager.instance.music.clip.length /
            SongManager.instance.songBPM;

        for (int i = 0; i < SongManager.instance.songBPM; i++)
        {
            GameObject go = Instantiate(pf_timelineBeat);
            go.transform.SetParent(parent, false);
        }
    }
}

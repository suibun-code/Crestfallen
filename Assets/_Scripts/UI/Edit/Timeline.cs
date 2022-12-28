using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject pf_timelineBeat;

    public void InitTimeline()
    {
        float crotchet = 60f / SongManager.instance.beatmap.songBPM;

        float realSongDuration = SongManager.instance.music.clip.length /
            crotchet;

        Debug.Log("BPM: " + SongManager.instance.beatmap.songBPM);
        Debug.Log("SONG LENGTH: " + SongManager.instance.beatmap.clip.length);
        Debug.Log("CROTCHET: " + crotchet);
        Debug.Log("BEATS IN SONG: " + realSongDuration);

        for (int i = 0; i < realSongDuration; i++)
        {
            GameObject go = Instantiate(pf_timelineBeat);
            go.transform.SetParent(parent, false);
        }
    }
}

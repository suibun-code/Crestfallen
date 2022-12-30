using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject pf_timelineBeat;
    [SerializeField] private GameObject pf_timelineHalfBeat;
    [SerializeField] private List<GameObject> beats = new List<GameObject>();

    private float beatcount;

    public void InitTimeline()
    {
        float crotchet = 60f / SongManager.instance.beatmap.songBPM;

        float totalBeats = SongManager.instance.music.clip.length /
            crotchet;

        //Debug.Log("BPM: " + SongManager.instance.beatmap.songBPM);
        //Debug.Log("SONG LENGTH: " + SongManager.instance.beatmap.clip.length);
        //Debug.Log("CROTCHET: " + crotchet);
        //Debug.Log("BEATS IN SONG: " + realSongDuration);

        for (int i = 0; i < totalBeats; i++)
        {
            GameObject beat = Instantiate(pf_timelineBeat);
            beat.transform.SetParent(parent, false);
            beat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = beatcount.ToString();
            beats.Add(beat);

            for (int j = 0; j < 3; j++)
            {
                beatcount += 0.25f;
                GameObject halfBeat = Instantiate(pf_timelineHalfBeat);
                halfBeat.transform.SetParent(parent, false);
                halfBeat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = beatcount.ToString();
                beats.Add(beat);
            }

            beatcount += 0.25f;
        }
    }

    public void DestroyTimeline()
    {
        foreach (GameObject beat in beats)
        {
            Destroy(beat);
        }

        beats.Clear();
        beatcount = 0;
    }
}

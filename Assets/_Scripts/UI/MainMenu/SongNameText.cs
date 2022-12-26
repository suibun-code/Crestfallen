using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongNameText : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    private void OnEnable()
    {
        SongManager.OnNewSong += SetSongNameText;
    }

    private void OnDisable()
    {
        SongManager.OnNewSong -= SetSongNameText;
    }

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        if (SongManager.instance.music.clip != null)
        {
            textMesh.text = SongManager.instance.music.clip.name;
        }
        else
        {
            textMesh.text = "ERROR: music clip does not exist.";
        }
    }

    public void SetSongNameText()
    {
        Debug.Log("Song name: " + SongManager.instance.beatmap.songName);
        textMesh.text = SongManager.instance.beatmap.songName;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class BeatmapInfo : Singleton<BeatmapInfo>
{
    public string artPath;
    public string artName;
    public RawImage beatmapArt;
    public TMP_InputField input_name;
    public TMP_InputField input_description;
    public TMP_InputField input_songArtist;
    public TMP_InputField input_mapperName;
    public TMP_InputField input_difficulty;
    public TMP_InputField input_songBPM;
    public TMP_InputField input_firstBeatOffset;

    [ReadOnly] public string beatmapName;
    [ReadOnly] public string beatmapDescription;
    [ReadOnly] public string beatmapSongArtist;
    [ReadOnly] public string beatmapMapperName;
    [ReadOnly] public float beatmapDifficulty;
    [ReadOnly] public float beatmapBPM;
    [ReadOnly] public float beatmapFirstBeatOffset;

    public void SetArt()
    {
        artPath = FileDialogPlugin.OpenFileDialog("Upload an image file to use as your avatar.", "Upload an image file.", "*.png;*.jpg");
        artName = FileDialogPlugin.GetFileName();

        MapMakerManager.instance.artPath = artPath;
        MapMakerManager.instance.artName = artName;

        StartCoroutine(LoadFile.LoadImage(beatmapArt.texture, artPath));
    }

    public void SetName()
    {
        beatmapName = input_name.text;
        Debug.Log(beatmapName);

        MapMakerManager.instance.beatmapName = beatmapName;
    }

    public void SetDescription()
    {
        beatmapDescription = input_description.text;
        Debug.Log(beatmapDescription);

        MapMakerManager.instance.beatmapDescription = beatmapDescription;
    }

    public void SetSongArtist()
    {
        beatmapSongArtist = input_songArtist.text;
        Debug.Log(beatmapSongArtist);

        MapMakerManager.instance.songArtist = beatmapSongArtist;
    }

    public void SetMapperName()
    {
        beatmapMapperName = input_mapperName.text;
        Debug.Log(beatmapMapperName);

        MapMakerManager.instance.mapperName = beatmapMapperName;
    }

    public void SetDifficulty()
    {
        float.TryParse(input_difficulty.text, out beatmapDifficulty);
        Debug.Log(beatmapDifficulty);

        MapMakerManager.instance.difficulty = beatmapDifficulty;
    }

    public void SetBPM()
    {
        float.TryParse(input_songBPM.text, out beatmapBPM);
        Debug.Log(beatmapBPM);

        MapMakerManager.instance.songBPM = beatmapBPM;
    }

    public void SetFirstBeatOffset()
    {
        float.TryParse(input_firstBeatOffset.text, out beatmapFirstBeatOffset);
        Debug.Log(beatmapFirstBeatOffset);

        MapMakerManager.instance.firstBeatOffset = beatmapFirstBeatOffset;
    }
}
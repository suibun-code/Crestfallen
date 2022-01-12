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
    public TMP_InputField input_previewStartTime;

    [ReadOnly] public string beatmapName;
    [ReadOnly] public string beatmapDescription;
    [ReadOnly] public string beatmapSongArtist;
    [ReadOnly] public string beatmapMapperName;
    [ReadOnly] public float beatmapDifficulty;
    [ReadOnly] public float beatmapBPM;
    [ReadOnly] public float beatmapFirstBeatOffset;
    [ReadOnly] public float beatmapPreviewStartTime;

    public void SetArt()
    {
        artPath = FileDialogPlugin.OpenFileDialog("Upload an image file to use as your avatar.", "Upload an image file.", "*.png;*.jpg");
        artName = FileDialogPlugin.GetFileName();

        MapMakerManager.instance.artPath = artPath;
        MapMakerManager.instance.artName = artName;

        StartCoroutine(LoadImage(artPath));
    }

    public void SetName()
    {
        beatmapName = input_name.text;
        MapMakerManager.instance.beatmapName = beatmapName;
    }

    public void SetDescription()
    {
        beatmapDescription = input_description.text;
        MapMakerManager.instance.beatmapDescription = beatmapDescription;
    }

    public void SetSongArtist()
    {
        beatmapSongArtist = input_songArtist.text;
        MapMakerManager.instance.songArtist = beatmapSongArtist;
    }

    public void SetMapperName()
    {
        beatmapMapperName = input_mapperName.text;
        MapMakerManager.instance.mapperName = beatmapMapperName;
    }

    public void SetDifficulty()
    {
        float.TryParse(input_difficulty.text, out beatmapDifficulty);
        MapMakerManager.instance.difficulty = beatmapDifficulty;
    }

    public void SetBPM()
    {
        float.TryParse(input_songBPM.text, out beatmapBPM);
        MapMakerManager.instance.songBPM = beatmapBPM;
    }

    public void SetFirstBeatOffset()
    {
        float.TryParse(input_firstBeatOffset.text, out beatmapFirstBeatOffset);
        MapMakerManager.instance.firstBeatOffset = beatmapFirstBeatOffset;
    }

    public void SetPreviewStartTime()
    {
        float.TryParse(input_previewStartTime.text, out beatmapPreviewStartTime);
        MapMakerManager.instance.previewStartTime = beatmapPreviewStartTime;
    }

    public IEnumerator LoadImage(string path) // Loads *.mp3's
    {
        //Load audio from the chosen *.mp3 file
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        /*If there was an error loading the audio file, 
        log the error. Otherwise, set it to the audioSource*/
        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
            beatmapArt.texture = DownloadHandlerTexture.GetContent(www);
    }
}

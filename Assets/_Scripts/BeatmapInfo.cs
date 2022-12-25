using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.InputSystem;

public class BeatmapInfo : MonoBehaviour
{
    public string artPath;
    public string artName;
    public RawImage beatmapArt;
    public TMP_InputField inputName;
    public TMP_InputField inputDescription;
    public TMP_InputField inputSongArtist;
    public TMP_InputField inputMapperName;
    public TMP_InputField inputDifficulty;
    public TMP_InputField inputSongBPM;
    public TMP_InputField inputFirstBeatOffset;
    public TMP_InputField inputPreviewStartTime;

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
        beatmapName = inputName.text;
        MapMakerManager.instance.beatmapName = beatmapName;
    }

    public void SetDescription()
    {
        beatmapDescription = inputDescription.text;
        MapMakerManager.instance.beatmapDescription = beatmapDescription;
    }

    public void SetSongArtist()
    {
        beatmapSongArtist = inputSongArtist.text;
        MapMakerManager.instance.songArtist = beatmapSongArtist;
    }

    public void SetMapperName()
    {
        beatmapMapperName = inputMapperName.text;
        MapMakerManager.instance.mapperName = beatmapMapperName;
    }

    public void SetDifficulty()
    {
        float.TryParse(inputDifficulty.text, out beatmapDifficulty);
        MapMakerManager.instance.difficulty = beatmapDifficulty;
    }

    public void SetBPM()
    {
        float.TryParse(inputSongBPM.text, out beatmapBPM);
        MapMakerManager.instance.songBPM = beatmapBPM;
    }

    public void SetFirstBeatOffset()
    {
        float.TryParse(inputFirstBeatOffset.text, out beatmapFirstBeatOffset);
        MapMakerManager.instance.firstBeatOffset = beatmapFirstBeatOffset;
    }

    public void SetPreviewStartTime()
    {
        float.TryParse(inputPreviewStartTime.text, out beatmapPreviewStartTime);
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

    public void OnEscape(InputValue value)
    {
        CreateTrackTweens.instance.StopAnimatingUI();

        //Switch menu, print debug error if scene from string not found
        MenuManager.instance.SwitchMenu("///MainMenu (Scene)");

        inputName.text = "";
        inputDescription.text = "";
        inputSongArtist.text = "";
        inputMapperName.text = "";
        inputDifficulty.text = "";
        inputSongBPM.text = "";
        inputFirstBeatOffset.text = "";
        inputPreviewStartTime.text = "";
    }
}
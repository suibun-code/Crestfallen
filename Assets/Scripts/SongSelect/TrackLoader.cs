using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;

public class TrackLoader : Singleton<TrackLoader>
{
    public string tracksPath;
    public string folderPath;
    public Beatmap beatmap;
    public List<Beatmap> beatmaps;

    void Start()
    {
        tracksPath = MapMakerManager.instance.TracksPath;
        StartCoroutine(ScanDirectoryOfBeatmaps());
    }

    public IEnumerator ScanDirectoryOfBeatmaps()
    {
        var info = new DirectoryInfo(tracksPath);
        DirectoryInfo[] trackDirectories = info.GetDirectories();

        foreach (DirectoryInfo trackDirectory in trackDirectories)//************************************************************************************************************************************************
        {
            folderPath = Path.Combine(tracksPath, trackDirectory.Name); //USE THIS FOR THE NOSOUNDSFOUND TEXT IN SONGSELECT. FIND OUT HOW TO MAKE IT DISPLAY IF THERE ARE NO TRACKS TO BE FOUND.

            string jsonDataPath = ScanFilesOfDirectoryForGSTFile(folderPath); //Scan the files in the track folder to find the .gst
            string jsonData = ReadGSTFile(jsonDataPath);

            yield return StartCoroutine(LoadTrack(jsonData));
            SongSelectManager.instance.LoadBeatmaps();
        }

    }

    public string ScanFilesOfDirectoryForGSTFile(string trackFolderPath)
    {
        var info = new DirectoryInfo(trackFolderPath);
        var fileInfo = info.GetFiles("*.gst", SearchOption.TopDirectoryOnly);

        foreach (var file in fileInfo)
        {
            return System.IO.Path.Combine(trackFolderPath, file.Name);
        }

        Debug.Log("No .gst files found in directory.");
        return "";
    }

    public string ReadGSTFile(string jsonDataPath)
    {
        return System.IO.File.ReadAllText(jsonDataPath);
    }

    public IEnumerator LoadTrack(string jsonData)
    {
        beatmap = ScriptableObject.CreateInstance<Beatmap>();
        JsonUtility.FromJsonOverwrite(jsonData, beatmap);

        if (beatmap.music != null)
        {
            beatmap.music.UnloadAudioData();
            DestroyImmediate(beatmap.music, false);
        }
        if (beatmap.art != null)
        {
            DestroyImmediate(beatmap.art, false);
        }

        yield return StartCoroutine(LoadImage(Path.Combine(folderPath, beatmap.artName)));

        DateTime before = DateTime.Now;

        yield return StartCoroutine(LoadAudioFile(Path.Combine(folderPath, beatmap.musicName)));

        //Debug.Log(Path.Combine(folderPath, beatmap.artName));
        //Debug.Log(Path.Combine(folderPath, beatmap.musicName));

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        //Debug.Log("Function took " + duration.Milliseconds + "ms.");

        beatmaps.Add(beatmap);
        //Debug.Log("Beatmap added");
    }

    public IEnumerator LoadAudioFile(string path) // Loads *.mp3's
    {
        //Load audio from the chosen *.mp3 file
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;

        www.SendWebRequest();

        while (www.result != UnityWebRequest.Result.ConnectionError && www.downloadedBytes < 1024)
            yield return null;

        beatmap.music = ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;

    }

    public IEnumerator LoadImage(string path) // Loads *.mp3's
    {
        //Load audio from the chosen *.mp3 file
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        /*If there was an error loading the audio file, 
        log the error. Otherwise, set it to the audioSource*/
        if (www.result == UnityWebRequest.Result.Success)
            beatmap.art = DownloadHandlerTexture.GetContent(www);
        else
            Debug.Log(www.error);
    }
}

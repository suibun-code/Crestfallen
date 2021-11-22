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
        ScanDirectoryOfBeatmaps();
    }

    public async void ScanDirectoryOfBeatmaps()
    {
        var info = new DirectoryInfo(tracksPath);
        DirectoryInfo[] trackDirectories = info.GetDirectories();

        foreach (DirectoryInfo trackDirectory in trackDirectories)
        {
            folderPath = Path.Combine(tracksPath, trackDirectory.Name);

            string jsonDataPath = ScanFilesOfDirectoryForGSTFile(folderPath); //Scan the files in the track folder to find the .gst
            string jsonData = ReadGSTFile(jsonDataPath);

            await LoadTrack(jsonData);

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

    public async Task LoadTrack(string jsonData)
    {
        beatmap = ScriptableObject.CreateInstance<Beatmap>();
        JsonUtility.FromJsonOverwrite(jsonData, beatmap);

        if (beatmap.art == null)
            beatmap.art = await LoadFile.instance.LoadImage(Path.Combine(folderPath, beatmap.artName));

        DateTime before = DateTime.Now;

        if (beatmap.music == null)
            beatmap.music = await LoadFile.instance.LoadAudioFile(Path.Combine(folderPath, beatmap.musicName));

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("Function took " + duration.Milliseconds + "ms.");

        beatmaps.Add(beatmap);
    }
}

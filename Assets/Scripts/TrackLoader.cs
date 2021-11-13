using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrackLoader : Singleton<TrackLoader>
{
    public string directoryPath;
    List<Beatmap> beatmaps;

    void Start()
    {
        directoryPath = MapMakerManager.instance.TracksPath;
        ScanDirectoryOfBeatmaps();
    }

    public void LoadTrack(string jsonPath)
    {
        Beatmap beatmap;

        //JsonUtility.FromJsonOverwrite()
    }

    public void ScanDirectoryOfBeatmaps()
    {
        var info = new DirectoryInfo(directoryPath);
        DirectoryInfo[] trackDirectories = info.GetDirectories();

        foreach (DirectoryInfo trackDirectory in trackDirectories)
        {
            string trackFolderPath = System.IO.Path.Combine(directoryPath, trackDirectory.Name);
            string gstFilePath = ScanFilesOfDirectory(trackFolderPath); //Scan the files in the track folder to find the .gst

            Debug.Log(gstFilePath);
            //LoadTrack(gstFilePath);
        }
    }

    public string ScanFilesOfDirectory(string directoryPath)
    {
        var info = new DirectoryInfo(directoryPath);
        var fileInfo = info.GetFiles("*.gst", SearchOption.TopDirectoryOnly);

        foreach (var file in fileInfo)
        {
            return file.FullName;
        }

        Debug.Log("No .gst files found in directory.");
        return "";
    }
}

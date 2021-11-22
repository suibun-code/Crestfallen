using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MapMakerManager : Singleton<MapMakerManager>
{
    public string g_TracksPath;

    public string TracksPath
    {
        get => g_TracksPath;
    }

    [ReadOnly] public string musicPath;
    [ReadOnly] public string musicName;

    [ReadOnly] public string artPath;
    [ReadOnly] public string artName;

    [ReadOnly] public string beatmapName;
    [ReadOnly] public string beatmapDescription;
    [ReadOnly] public string songArtist;
    [ReadOnly] public string mapperName;

    [ReadOnly] public float songBPM;
    [ReadOnly] public float difficulty;
    [ReadOnly] public float firstBeatOffset;
    [ReadOnly] public float previewStartTime;

    new private void Awake()
    {
        g_TracksPath = System.IO.Path.Combine(Application.streamingAssetsPath, "tracks");
        if (!Directory.Exists(g_TracksPath))
            Directory.CreateDirectory(g_TracksPath);
    }

    public void Save()
    {
        string beatmapPath = System.IO.Path.Combine(g_TracksPath, beatmapName);

        if (!Directory.Exists(beatmapPath))
        {
            Directory.CreateDirectory(beatmapPath);
        }
        else
        {
            while (Directory.Exists(beatmapPath))
                beatmapPath = System.IO.Path.Combine(g_TracksPath, beatmapName + Random.Range(0, 10000));

            Directory.CreateDirectory(beatmapPath);
        }

        Beatmap beatmap = ScriptableObject.CreateInstance<Beatmap>();
        beatmap.folderPath = beatmapPath;
        beatmap.songName = beatmapName;
        beatmap.description = beatmapDescription;
        beatmap.songArtist = songArtist;
        beatmap.mapperName = mapperName;
        beatmap.difficulty = difficulty;
        beatmap.songBPM = songBPM;
        beatmap.firstBeatOffset = firstBeatOffset;
        beatmap.previewStartTime = previewStartTime;
        beatmap.artName = artName;
        beatmap.musicName = musicName;

        /*Creates a json from the object and saves it to the beatmap 
        name's directory. Also saves audio and image file to same path*/
        var json = JsonUtility.ToJson(beatmap);

        string gstFullName = beatmapName + ".gst";
        string gstFullPath = System.IO.Path.Combine(beatmapPath, gstFullName);
        System.IO.File.WriteAllText(gstFullPath, json);

        string musicCopyPath = Path.Combine(beatmapPath, musicName);
        string artCopyPath = Path.Combine(beatmapPath, artName);

        if (File.Exists(musicPath) && !File.Exists(musicCopyPath))
            File.Copy(musicPath, musicCopyPath, true);
        else
            Debug.Log("MusicPath doesn't exist, or destination path already exists");

        if (File.Exists(artPath) && !File.Exists(artCopyPath))
            File.Copy(artPath, artCopyPath, true);
        else
            Debug.Log("ArtPath doesn't exist, or destination path already exists");

        Debug.Log("Saved!");
    }
}

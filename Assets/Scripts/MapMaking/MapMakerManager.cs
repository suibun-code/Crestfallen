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

    new private void Awake()
    {
        g_TracksPath = Application.persistentDataPath + "/tracks/";
        if (!Directory.Exists(g_TracksPath))
            Directory.CreateDirectory(g_TracksPath);
    }

    public void Save()
    {
        string beatmapPath = g_TracksPath + "/" + beatmapName + "/";

        if (!Directory.Exists(beatmapPath))
        {
            Directory.CreateDirectory(beatmapPath);
        }
        else
        {
            while (Directory.Exists(beatmapPath))
                beatmapPath = g_TracksPath + "/" + beatmapName + Random.Range(0, 10000) + "/";

            Directory.CreateDirectory(beatmapPath);
        }


        //MAKE BEATMAP SCRIPTABLE OBJECT HERE
        Beatmap beatmap = ScriptableObject.CreateInstance<Beatmap>();
        beatmap.relativePath = beatmapPath;
        beatmap.songName = beatmapName;
        beatmap.description = beatmapDescription;
        beatmap.songArtist = songArtist;
        beatmap.mapperName = mapperName;
        beatmap.difficulty = difficulty;
        beatmap.songBPM = songBPM;
        beatmap.firstBeatOffset = firstBeatOffset;
        beatmap.artName = artName;
        beatmap.musicName = musicName;

        /*Creates a json from the object and saves it to the beatmap 
        name's directory. Also saves audio file to same path*/
        var json = JsonUtility.ToJson(beatmap);
        System.IO.File.WriteAllText(beatmapPath + beatmapName + ".gst", json);

        if (File.Exists(musicPath) && !File.Exists(beatmapPath + musicName))
        {
            File.Copy(musicPath, beatmapPath + musicName, true);
        }
        else
        {
            Debug.Log("MusicPath doesn't exist, or destination path already exists");
        }

        if (File.Exists(artPath) && !File.Exists(artPath + artName))
        {
            File.Copy(artPath, beatmapPath + artName, true);
        }
        else
        {
            Debug.Log("ArtPath doesn't exist, or destination path already exists");
        }

        Debug.Log("Saved!");
    }
}

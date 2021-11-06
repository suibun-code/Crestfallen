using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrackToJson : MonoBehaviour
{
    string savePath;

    private void Awake()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/tracks/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/tracks/");

        savePath = Application.persistentDataPath + "/tracks/";
    }

    public void Save(Beatmap beatmap)
    {
        var json = JsonUtility.ToJson(beatmap);
        System.IO.File.WriteAllText(savePath + beatmap.name + ".gst", json);
    }
}

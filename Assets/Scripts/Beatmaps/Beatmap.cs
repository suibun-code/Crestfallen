using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Beatmap", menuName = "Beatmap")]
public class Beatmap : ScriptableObject
{
    public float previewTimeStart; //REMEMBER TO DO THIS
    
    public string relativePath;
    public string songName;
    public string description;
    public string songArtist;
    public string mapperName;
    public float difficulty;
    public float songBPM;
    public float firstBeatOffset;

    public string artName;
    public string musicName;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Beatmap", menuName = "Beatmap")]
public class Beatmap : ScriptableObject
{
    public float previewTimeStart;

    public AudioClip music;

    public Sprite artwork;
    public string songName;
    public string songArtist;
    public string mapMapper;
    public float songBPM;
    public float firstBeatOffset;
    public float difficulty;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Beatmap", menuName = "Beatmap")]
public class Beatmap : ScriptableObject
{
    public Sprite artwork;
    public string songName;
    public string songArtist;
    public string mapMapper;
    public float songBPM;
    public float difficulty;


}

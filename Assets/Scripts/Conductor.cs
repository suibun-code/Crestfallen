using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : Singleton<Conductor>
{
    //Music Metadata
    public float howOftenToSpawn; //How often to spawn notes. 1 = once per beat
    public float songBPM; //Beats per minute of the song
    public float firstBeatOffset; //First beat offset for the music, as there is silence or otherwise noise before the first beat of the song
    public float beatsBeforeArrive; //"Note speed", how many beats go by before the hitline reaches the playerline. Lower = faster
    [ReadOnly] public float crotchet; //How long 1 beat is
    [ReadOnly] public float songPosition;
    [ReadOnly] public float songPosInBeats;
    [ReadOnly] public float dspSongTime;

    //Hitline properties
    public GameObject hitLinePrefab;
    private int currentColor;
    [System.NonSerialized] public float[] notes = new float[15000]; //Stores which beat to spawn hitlines on
    int nextIndex = 0; //Tracks which hitline is next

    //Misc
    public bool autoHit = false; //Automatically hit the notes without player input?

    void Start()
    {
        //Set values of the song and reset the music time
        songBPM = GameManager.instance.songBPM;
        firstBeatOffset = GameManager.instance.firstBeatOffset;
        GameManager.instance.ResetMusic();

        firstBeatOffset += (crotchet / 10f); //I don't know why
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) * GameManager.instance.music.pitch - firstBeatOffset; //Determine how many seconds since the song started
        crotchet = 60f / songBPM; //Calculate the number of seconds in each beat

        dspSongTime = (float)AudioSettings.dspTime; //Record the time when the music starts
        GameManager.instance.PlayMusic(); //Start the music

        //How often to spawn notes -- feature for testing
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] = i * howOftenToSpawn;
        }
    }

    void Update()
    {
        
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) * GameManager.instance.music.pitch - firstBeatOffset; //Determine how many seconds since the song started
        songPosInBeats = songPosition / crotchet; //Determine how many beats since the song started

        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsBeforeArrive)
        {
            var hitLine = Instantiate(hitLinePrefab);

            hitLine.transform.parent = transform;

            currentColor = Random.Range(0, (int)LineColorEnum.COUNT);

            var hitLineBehaviour = hitLine.GetComponent<HitLine>();
            hitLineBehaviour.beat = notes[nextIndex];
            hitLineBehaviour.SetColor((LineColorEnum)currentColor);

            nextIndex++;
        }
    }
}

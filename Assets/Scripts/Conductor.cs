using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : Singleton<Conductor>
{
    //Music Metadata
    public bool autoHit = false; //Automatically hit the notes without player input?
    public float howOftenToSpawn; //How often to spawn notes. 1 = once per beat
    public float songBpm; //Beats per minute of the song
    public float firstBeatOffset; //First beat offset for the music, as there is silence or otherwise noise before the first beat of the song
    public float beatsBeforeArrive; //"Note speed", how many beats go by before the hitline reaches the playerline. Lower = faster
    [ReadOnly] public float crotchet; //How long 1 beat is
    [ReadOnly] public float songPosition;
    [ReadOnly] public float songPosInBeats;
    [ReadOnly] public float dspSongTime;

    //Hitline
    public GameObject hitLinePrefab;
    private int currentColor;
    [System.NonSerialized] public float[] notes = new float[15000]; //Stores which beat to spawn hitlines on
    int nextIndex = 0; //Tracks which hitline is next

    void Start()
    {
        AudioManager.instance.mapMusic = SongSelector.instance.music;
        firstBeatOffset = SongSelector.instance.currentCell.beatmap.firstBeatOffset;
        songBpm = SongSelector.instance.currentCell.beatmap.songBPM;

        //Calculate the number of seconds in each beat
        crotchet = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        AudioManager.instance.PlayMusic();

        //I don't know why
        firstBeatOffset += (crotchet / 10f);

        //Determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) * AudioManager.instance.mapMusic.pitch - firstBeatOffset;

        //How often to spawn notes -- feature for testing
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] = i * howOftenToSpawn;
        }
    }

    void Update()
    {
        //Determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) * AudioManager.instance.mapMusic.pitch - firstBeatOffset;

        //Determine how many beats since the song started
        songPosInBeats = songPosition / crotchet;

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

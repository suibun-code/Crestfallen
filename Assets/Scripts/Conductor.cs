using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : Singleton<Conductor>
{
    //Music
    public AudioSource musicSource;

    //Music Metadata
    public int howOftenToSpawn;
    public float songBpm;
    public float firstBeatOffset;
    public float beatsShownInAdvance;
    [ReadOnly] public float crotchet;
    [ReadOnly] public float songPosition;
    [ReadOnly] public float songPosInBeats;
    [ReadOnly] public float dspSongTime;

    //Hitline
    public GameObject hitLinePrefab;
    private int currentColor;
    [System.NonSerialized] public float[] notes = new float[150];
    int nextIndex = 0;

    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        crotchet = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();

        //Determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) * musicSource.pitch - firstBeatOffset;

        for (int i = 0; i < 150; i++)
        {
            notes[i] = i * howOftenToSpawn;
        }
    }

    void Update()
    {
        //Determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) * musicSource.pitch - firstBeatOffset;

        //Determine how many beats since the song started
        songPosInBeats = songPosition / crotchet;

        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            var hitLine = Instantiate(hitLinePrefab);

            //hitLine.transform.parent = transform;

            currentColor = Random.Range(0, (int)LineColorEnum.COUNT);

            var hitLineBehaviour = hitLine.GetComponent<HitLine>();
            hitLineBehaviour.beat = notes[nextIndex];
            hitLineBehaviour.SetColor((LineColorEnum)currentColor);

            nextIndex++;
        }
    }
}

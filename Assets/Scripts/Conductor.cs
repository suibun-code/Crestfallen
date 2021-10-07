using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : Singleton<Conductor>
{
    //Music
    public AudioSource musicSource;

    //Music Metadata
    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPosInBeats;
    public float dspSongTime;
    public float firstBeatOffset;
    public float beatsShownInAdvance = 4f;

    //Hitline
    public GameObject hitLinePrefab;
    private int currentColor;
    public float[] notes;
    int nextIndex = 0;

    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }

    void Update()
    {
        //Determine how many beats since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //Determine how many beats since the song started
        songPosInBeats = songPosition / secPerBeat;

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

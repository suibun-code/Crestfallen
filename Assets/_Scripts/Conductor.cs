using System.Collections.Generic;
using UnityEngine;

public class Conductor : Singleton<Conductor>
{
    bool flipflop = false;
    bool flipflop2 = false;
    bool flipflop3 = false;

    public List<Hitline> leftHitlines;
    public List<Hitline> rightHitlines;

    //Hitline properties
    private int currentColor;
    [System.NonSerialized] public float[] notes = new float[15000]; //Stores which beat to spawn hitlines on
    int nextIndex = 0; //Tracks which hitline is next

    //Misc
    public bool autoHit = true; //Automatically hit the notes without player input?

    //Music Metadata
    /*Fields*/
    [SerializeField] private float howOftenToSpawn; //How often to spawn notes. 1 = once per beat
    [SerializeField] private float beatsBeforeArrive; //"Note speed", how many beats go by before the hitline reaches the playerline. Lower = faster
    [SerializeField] private float songBPM; //Beats per minute of the song
    [SerializeField] private float firstBeatOffset; //First beat offset for the music, as there is silence or otherwise noise before the first beat of the song
    [SerializeField] private float crotchet; //How long 1 beat is
    [SerializeField] private float songPosition;
    [SerializeField] private float songPosInBeats;
    [SerializeField] private float dspSongTime;

    /*Properties*/
    public float BeatsBeforeArrive { get { return beatsBeforeArrive; } private set { beatsBeforeArrive = value; } }
    public float SongBPM           { get { return songBPM;           } private set { songBPM = value;           } }
    public float FirstBeatOffset   { get { return firstBeatOffset;   } private set { firstBeatOffset = value;   } }
    public float Crotchet          { get { return crotchet;          } private set { crotchet = value;          } }
    public float SongPosition      { get { return songPosition;      } private set { songPosition = value;      } }
    public float SongPosInBeats    { get { return songPosInBeats;    } private set { songPosInBeats = value;    } }
    public float DspSongTime       { get { return dspSongTime;       } private set { dspSongTime = value;       } }

    void Start()
    {
        //Set values of the song and reset the music time
        SongBPM = SongManager.instance.songBPM;
        FirstBeatOffset = SongManager.instance.firstBeatOffset;
        SongManager.instance.ResetMusic();

        //firstBeatOffset += (crotchet / 10f); //I don't know why
        DspSongTime = (float)AudioSettings.dspTime; //Record the time when the music starts
        SongPosition = (float)(AudioSettings.dspTime - DspSongTime) * SongManager.instance.GetPitch() - FirstBeatOffset; //Determine how many seconds since the song started
        Crotchet = 60f / SongBPM; //Calculate the number of seconds in each beat

        SongManager.instance.PlayMusic(); //Start the music

        //How often to spawn notes -- TESTING FEATURE
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] = i * howOftenToSpawn;
        }
    }

    void Update()
    {
        SongPosition = (float)(AudioSettings.dspTime - DspSongTime) * SongManager.instance.GetPitch() - FirstBeatOffset; //Determine how many seconds since the song started
        SongPosInBeats = SongPosition / Crotchet; //Determine how many beats since the song started

        if (nextIndex < notes.Length && notes[nextIndex] < SongPosInBeats + BeatsBeforeArrive) //If there are notes to spawn, and it is time to spawn one
        {
            //Instantiate hitline with conductor gameobject as parent
            GameObject go_hitline;

            flipflop3 = (Random.value > 0.15f);
            if (flipflop3)
                go_hitline = HitlineFactory.instance.GetHitline(HitlineType.SMALL, transform, true);
            else
                go_hitline = HitlineFactory.instance.GetHitline(HitlineType.BIG, transform, true);

            Hitline hitline = go_hitline.GetComponent<Hitline>();

            /*******************************************THIS MUST BE CHANGED. FEATURE ONLY FOR TESTING*******************************************/
            hitline.Beat = notes[nextIndex]; //Set the beat the hitline should arrive on
            hitline.PosInSeconds = hitline.Beat * Crotchet;

            //Flip flop between left and right lanes. for testing purposes.
            flipflop = (Random.value > 0.5f);

            if (flipflop)
            {
                hitline.Lane = 0;

                if (hitline.hitlineType == HitlineType.SMALL)
                {
                    flipflop2 = (Random.value > 0.5f);

                    if (flipflop2)
                        hitline.Sublane = 0;
                    else if (!flipflop2)
                        hitline.Sublane = 1;
                }
            }
            else if (!flipflop)
            {
                hitline.Lane = 1;

                if (hitline.hitlineType == HitlineType.SMALL)
                {
                    flipflop2 = (Random.value > 0.5f);

                    if (flipflop2)
                        hitline.Sublane = 0;
                    else if (!flipflop2)
                        hitline.Sublane = 1;
                }
            }
            /*******************************************THIS MUST BE CHANGED. FEATURE ONLY FOR TESTING*******************************************/

            AddHitlineToList(hitline);

            nextIndex++; //Go to the next hitline
        }
    }

    public List<Hitline> GetHitlines(int lane)
    {
        if (lane == 0)
            return leftHitlines;
        else
            return rightHitlines;
    }

    private void AddHitlineToList(Hitline hitline)
    {
        if (hitline.Lane == 0)
            leftHitlines.Add(hitline.GetComponent<Hitline>());
        else if (hitline.Lane == 1)
            rightHitlines.Add(hitline.GetComponent<Hitline>());
    }
}

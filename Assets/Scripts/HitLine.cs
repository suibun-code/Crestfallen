using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitLine : MonoBehaviour
{
    [ReadOnly] public Vector3 spawnPos = new Vector3(0.0f, 0.0f, 80.0f);
    [ReadOnly] public Vector3 endPos = new Vector3(0.0f, 0.0f, 5f);
    [ReadOnly] public float removePos = 1.4f;
    [ReadOnly] public float beat = 0f;
    [ReadOnly] public float lastBeat = 0f;

    //Current color
    [ReadOnly] public LineColorEnum hitLineColor = LineColorEnum.RED;

    //Components
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (transform.position.z <= removePos)
        {
            ScoreTracker.instance.ResetCombo();
            ScoreTracker.instance.HitMiss();
            Destroy(gameObject);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, spawnPos.z + (endPos.z - spawnPos.z) * (1f - (beat - Conductor.instance.songPosInBeats) / Conductor.instance.beatsBeforeArrive));

        //transform.position = Vector3.Lerp(
        //    spawnPos,
        //    endPos,
        //    (Conductor.instance.beatsShownInAdvance - (beat - Conductor.instance.songPosInBeats)) / Conductor.instance.beatsShownInAdvance
        //    );

        var test = (Conductor.instance.beatsBeforeArrive - (beat - Conductor.instance.songPosInBeats)) / Conductor.instance.beatsBeforeArrive;

        //Debug.Log(test);

        //Debug.Log("SONG POS IN BEATS: " + Conductor.instance.songPosInBeats);
        //Debug.Log("BEAT: " + beat);
        //Debug.Log("CROTCHET: " + Conductor.instance.crotchet);
        //Debug.Log("ADVANCE: " + Conductor.instance.beatsShownInAdvance);
        //Debug.Log("BEAT + CROTCHET: " + (beat + Conductor.instance.crotchet));
        if (Conductor.instance.songPosInBeats >= beat)
        {
            Debug.Log("SONG POS IN BEATS: " + Conductor.instance.songPosInBeats);
            Debug.Log("BEAT: " + beat);
            AudioManager.instance.PlayHitSound();
            Destroy(gameObject);

            lastBeat += Conductor.instance.crotchet;
        }

        ////Autohit
        //if (transform.position.z == 5f)
        //{
        //    AudioManager.instance.PlayHitSound();
        //    Destroy(gameObject);
        //}
    }

    public void SetColor(LineColorEnum color)
    {
        switch (color)
        {
            case LineColorEnum.RED:
                _renderer.material.SetColor("_BaseColor", HitLineColor.red);
                break;

            case LineColorEnum.GREEN:
                _renderer.material.SetColor("_BaseColor", HitLineColor.green);
                break;

            case LineColorEnum.BLUE:
                _renderer.material.SetColor("_BaseColor", HitLineColor.blue);
                break;

            case LineColorEnum.YELLOW:
                _renderer.material.SetColor("_BaseColor", HitLineColor.yellow);
                break;
        }

        hitLineColor = color;
    }
}

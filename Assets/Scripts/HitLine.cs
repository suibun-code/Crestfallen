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
        //Remove hitline if past the playerline, no longer on the screen
        if (transform.position.z <= removePos)
        {
            ScoreTracker.instance.ResetCombo();
            ScoreTracker.instance.HitMiss();
            Destroy(gameObject);
        }

        //Move the hitlines down based on the audio
        transform.position = new Vector3(transform.position.x, 
            transform.position.y,
            spawnPos.z + (endPos.z - spawnPos.z) * (1f - (beat - Conductor.instance.songPosInBeats) / Conductor.instance.beatsBeforeArrive));


        //Autohit
        if (Conductor.instance.songPosInBeats >= beat && Conductor.instance.autoHit == true)
        {
            ScoreTracker.instance.HitPerfect();
            ScoreTracker.instance.UpdateTexts();
            AudioManager.instance.PlayHitSound();
            Destroy(gameObject);
        }
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

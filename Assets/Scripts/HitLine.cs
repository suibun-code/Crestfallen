using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitLine : MonoBehaviour
{
    [ReadOnly] private Vector3 spawnPos = new Vector3(-1.85f, -12.275f, 80.0f);
    [ReadOnly] private Vector3 endPos = new Vector3(-1.85f, 1.15f, 4f);
    [ReadOnly] private float removePos = 1.4f;
    [ReadOnly] private float offsetAmount;

    [ReadOnly] public float beat = 0f; //What beat it will arrive on

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

        offsetAmount = (1f - (beat - Conductor.instance.songPosInBeats) / Conductor.instance.beatsBeforeArrive);

        //Move the hitlines down based on the audio
        transform.position = new Vector3(spawnPos.x, spawnPos.y + (endPos.y - spawnPos.y) * offsetAmount, spawnPos.z + (endPos.z - spawnPos.z) * offsetAmount);


        //Autohit
        if (Conductor.instance.songPosInBeats >= beat && Conductor.instance.autoHit == true)
        {
            ScoreTracker.instance.HitPerfect();
            ScoreTracker.instance.UpdateTexts();
            SongManager.instance.PlayHitSound();
            HitDetector.instance.playerLineHit.Play(0);
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

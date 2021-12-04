using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitLine : MonoBehaviour
{
    [ReadOnly] private Vector3 spawnPos1 = new Vector3(-1.85f, -12.275f, 80.0f);
    [ReadOnly] private Vector3 endPos1 = new Vector3(-1.85f, 1.15f, 4f);

    [ReadOnly] private Vector3 spawnPos2 = new Vector3(1.85f, -12.275f, 80.0f);
    [ReadOnly] private Vector3 endPos2 = new Vector3(1.85f, 1.15f, 4f);

    [ReadOnly] private float removePos = 1.4f;
    [ReadOnly] private float offsetAmount;

    [ReadOnly] public float beat = 0f; //What beat it will arrive on
    [ReadOnly] public float positionInSeconds;
    [ReadOnly] public int lane;

    //Current color
    [ReadOnly] public int hitLineColor = 0;

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
            ScoreTracker.instance.HitMiss();
            RemoveFromList();
            Destroy(gameObject);
        }

        offsetAmount = (1f - (beat - Conductor.instance.songPosInBeats) / Conductor.instance.beatsBeforeArrive);

        //Move the hitlines down based on the audio
        if (lane == 1)
            transform.position = new Vector3(spawnPos1.x, spawnPos1.y + (endPos1.y - spawnPos1.y) * offsetAmount, spawnPos1.z + (endPos1.z - spawnPos1.z) * offsetAmount);
        else if (lane == 2)
            transform.position = new Vector3(spawnPos2.x, spawnPos2.y + (endPos2.y - spawnPos2.y) * offsetAmount, spawnPos2.z + (endPos2.z - spawnPos2.z) * offsetAmount);

        //Autohit
        if (Conductor.instance.songPosition >= positionInSeconds && Conductor.instance.autoHit == true)
        {
            if (lane == 1)
            {
                PlayerLineInput.instance.SetLeftToNextColor();
                PlayerLineInput.instance.StrumLeft();
            }
            else if (lane == 2)
            {
                PlayerLineInput.instance.SetRightToNextColor();
                PlayerLineInput.instance.StrumRight();
            }
        }
    }

    public void SetColor(int color)
    {
        if (lane == 1)
        {
            switch (color)
            {
                case 0:
                    _renderer.material.SetColor("_BaseColor", HitLineColor.red);
                    break;

                case 1:
                    _renderer.material.SetColor("_BaseColor", HitLineColor.green);
                    break;
            }
        }

        if (lane == 2)
        {
            switch (color)
            {
                case 0:
                    _renderer.material.SetColor("_BaseColor", HitLineColor.blue);
                    break;

                case 1:
                    _renderer.material.SetColor("_BaseColor", HitLineColor.yellow);
                    break;
            }
        }

        hitLineColor = color;
    }

    public void RemoveFromList()
    {
        if (lane == 1)
        {
            Conductor.instance.leftHitlines.Remove(this);
        }
        if (lane == 2)
        {
            Conductor.instance.rightHitlines.Remove(this);
        }

        Conductor.instance.leftHitlines.TrimExcess();
    }
}

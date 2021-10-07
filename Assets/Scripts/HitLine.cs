using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitLine : MonoBehaviour
{
    [NonSerialized] public Vector3 spawnPos = new Vector3(0.0f, 0.0f, 80.0f);
    [NonSerialized] public Vector3 endPos = new Vector3(0.0f, 0.0f, 5f);
    [NonSerialized] public Vector3 removePos = new Vector3(0.0f, 0.0f, 1.4f);
    [NonSerialized] public float beat;

    //Current color
    public LineColorEnum hitLineColor = LineColorEnum.RED;

    //Components
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (transform.position.z <= 3.4f)
        {
            ScoreTracker.instance.ResetCombo();
            ScoreTracker.instance.HitMiss();
            Destroy(gameObject);
        }

        transform.position = Vector3.Lerp(
            spawnPos, 
            endPos,
            (Conductor.instance.beatsShownInAdvance - (beat - Conductor.instance.songPosInBeats)) / Conductor.instance.beatsShownInAdvance
            );

        if (transform.position.z == 5f)
        {
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

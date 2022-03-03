using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum HitlineType
{
    BIG,
    SMALL
}

public class Hitline : MonoBehaviour
{
    [SerializeField] private HitlineType hitlineType;

    /*Fields*/
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 endPos;
    [ReadOnly] private float removePos = 1.4f;
    [ReadOnly] private float offsetAmount;

    [ReadOnly] private float beat; //What beat it will arrive on
    [ReadOnly] private float posInSeconds; //What time in seconds it will arrive on

    //Hitline lane, sublane & color
    [SerializeField] private int lane;
    [SerializeField] private int sublane;
    [SerializeField] private int hitlineColor;

    //Components
    protected Renderer _renderer;

    /*Properties*/
    public float Beat         { get { return beat; }         set { beat = value; } }
    public float PosInSeconds { get { return posInSeconds; } set { posInSeconds = value; } }

    public int Lane         { get { return lane; }         set { lane = value; } }
    public int Sublane      { get { return sublane; }      set { sublane = value; } }
    public int HitlineColor { get { return hitlineColor; } set { hitlineColor = value; } }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        SetLaneStartPosition();
        SetColor();
    }

    private void Update()
    {
        if (Conductor.instance.autoHit)
            Autohit();

        if (transform.position.z <= removePos)
        {
            DestroyHitline();
            ScoreTracker.instance.HitMiss();
        }

        MoveHitLine();
    }

    private void SetLaneStartPosition()
    {
        //Set the proper x position if hitline is in the second lane. No need to set the position of the first lane as it is the default.
        if (lane == 1)
        {
            spawnPos.x = 1.6f;
            endPos.x = 1.6f;
        }
    }

    private void MoveHitLine()
    {
        offsetAmount = (1f - (Beat - Conductor.instance.SongPosInBeats) / Conductor.instance.BeatsBeforeArrive);
        transform.position = new Vector3(spawnPos.x, spawnPos.y + (endPos.y - spawnPos.y) * offsetAmount, spawnPos.z + (endPos.z - spawnPos.z) * offsetAmount);
    }

    public void SetColor()
    {
        if (lane == 0)
        {
            if (HitlineColor == 0)
            {
                _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorOne);
            }
            else if (HitlineColor == 1)
            {
                _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorTwo);
            }
        }
        else if (lane == 1)
        {
            if (HitlineColor == 0)
            {
                _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorThree);
            }
            else if (HitlineColor == 1)
            {
                _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorFour);
            }
        }
    }

    private void Autohit()
    {
        if (Conductor.instance.SongPosition >= PosInSeconds)
        {
            if (Lane == 0)
            {
                PlayerLineInput.instance.SetLaneToNextColor(0);
                PlayerLineInput.instance.StrumLeft();
            }
            else if (Lane == 1)
            {
                PlayerLineInput.instance.SetLaneToNextColor(1);
                PlayerLineInput.instance.StrumRight();
            }
        }
    }

    public void DestroyHitline()
    {
        if (Lane == 0)
        {
            Conductor.instance.leftHitlines.Remove(this);
        }
        if (Lane == 1)
        {
            Conductor.instance.rightHitlines.Remove(this);
        }

        Conductor.instance.leftHitlines.TrimExcess();

        Destroy(this.gameObject);
    }
}

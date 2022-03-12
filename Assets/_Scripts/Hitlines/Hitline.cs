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
    [SerializeField] public HitlineType hitlineType;

    /*Fields*/
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 endPos;
    [ReadOnly] private float removePos = -4f;
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
    public float Beat { get { return beat; } set { beat = value; } }
    public float PosInSeconds { get { return posInSeconds; } set { posInSeconds = value; } }

    public int Lane { get { return lane; } set { lane = value; } }
    public int Sublane { get { return sublane; } set { sublane = value; } }
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
        if (hitlineType == HitlineType.SMALL)
        {
            if (lane == 0)
            {
                if (sublane == 0)
                {
                    //no change. use default spawn value
                }

                if (sublane == 1)
                {
                    spawnPos.x = -0.85f;
                    endPos.x = -0.85f;
                }
            }

            else if (lane == 1)
            {
                if (sublane == 0)
                {
                    spawnPos.x = 0.85f;
                    endPos.x = 0.85f;
                }

                else if (sublane == 1)
                {
                    spawnPos.x = 2.35f;
                    endPos.x = 2.35f;
                }
            }
        }

        else if (hitlineType == HitlineType.BIG)
        {
            //Set the proper x position if hitline is in the second lane. No need to set the position of the first lane as it is the default.
            if (lane == 1)
            {
                spawnPos.x = 1.6f;
                endPos.x = 1.6f;
            }
        }
    }

    private void MoveHitLine()
    {
        offsetAmount = (1f - (Beat - Conductor.instance.SongPosInBeats) / Conductor.instance.BeatsBeforeArrive);
        transform.position = new Vector3(spawnPos.x, spawnPos.y + (endPos.y - spawnPos.y) * offsetAmount, spawnPos.z + (endPos.z - spawnPos.z) * offsetAmount);
    }

    public void SetColor()
    {
        //Set BIG hitline colors
        if (hitlineType == HitlineType.BIG)
        {
            if (lane == 0)
            {
                if (HitlineColor == 0)
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorOne);

                else if (HitlineColor == 1)
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorTwo);
            }

            else if (lane == 1)
            {
                if (HitlineColor == 0)
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorThree);

                else if (HitlineColor == 1)
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorFour);
            }
        }

        //Set SMALL hitline colors
        else if (hitlineType == HitlineType.SMALL)
        {
            if (lane == 0)
            {
                if (sublane == 0)
                {
                    hitlineColor = 0;
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorOne);
                }

                else if (sublane == 1)
                {
                    hitlineColor = 1;
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorTwo);
                }

            }
            else if (lane == 1)
            {
                if (sublane == 0)
                {
                    hitlineColor = 0;
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorThree);
                }

                else if (sublane == 1)
                {
                    hitlineColor = 1;
                    _renderer.material.SetColor("_BaseColor", GameColors.instance.hitlineColorFour);
                }
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

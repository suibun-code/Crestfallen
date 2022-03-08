using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public struct StrumLane
{
    public string name;

    public List<Hitline> activeHitLines;
    public Renderer renderer;
    public Animator animator;
    public int color;
}

public class PlayerLineInput : Singleton<PlayerLineInput>
{
    [SerializeField] private float perfectAccuracy;
    [SerializeField] private float greatAccuracy;
    [SerializeField] private float badAccuracy;
    [SerializeField] private float minimumAccuracyToHit;

    private float accuracy;
    private Hitline nextHitline;

    //Lanes
    public StrumLane leftLane;
    public StrumLane rightLane;

    //Components
    [SerializeField] private Renderer leftLineRenderer;
    [SerializeField] private Renderer rightLineRenderer;
    [SerializeField] private Animator leftAnimator;
    [SerializeField] private Animator rightAnimator;

    void Start()
    {
        leftLane.name = "LeftLane";
        leftLane.activeHitLines = Conductor.instance.leftHitlines;
        leftLane.renderer = leftLineRenderer;
        leftLane.animator = leftAnimator;
        leftLane.color = 0;

        rightLane.name = "RightLane";
        rightLane.activeHitLines = Conductor.instance.rightHitlines;
        rightLane.renderer = rightLineRenderer;
        rightLane.animator = rightAnimator;
        rightLane.color = 0;

        leftLane.renderer.material.SetColor("PlayerLineColor", GameColors.instance.laneColorOne);
        rightLane.renderer.material.SetColor("PlayerLineColor", GameColors.instance.laneColorThree);
    }

    public void StrumLeft()
    {
        Strum(leftLane);
    }
    public void StrumRight()
    {
        Strum(rightLane);
    }

    public void Strum(StrumLane lane)
    {
        SongManager.instance.PlayHitSound();
        lane.animator.Play(0);

        if (lane.activeHitLines.Count != 0) //Make sure there are hitlines in the lane
            nextHitline = lane.activeHitLines.First(); //Assign the next hitline in line to be hit
        else
            return;

        if (lane.color != nextHitline.HitlineColor)//if the colors don't match
        {
            Missed();
            return;
        }

        CalculateAccuracy();
    }

    private void Missed()
    {
        nextHitline.DestroyHitline();
        ScoreTracker.instance.HitMiss();
    }

    private void CalculateAccuracy()
    {
        accuracy = Mathf.Abs(nextHitline.PosInSeconds - Conductor.instance.SongPosition);
        //Debug.Log("Accuracy: " + accuracy);

        if (accuracy >= minimumAccuracyToHit) //Accuracy not close enough to count the hit. Do nothing
        {
            //Debug.Log("ok");
            return;
        }

        if (accuracy >= badAccuracy)//early or late by half a beat
            ScoreTracker.instance.HitBad();
        else if (accuracy >= greatAccuracy)//early or late by half a beat
            ScoreTracker.instance.HitGreat();
        else if (accuracy >= perfectAccuracy)//early or late by half a beat
            ScoreTracker.instance.HitPerfect();

        nextHitline.DestroyHitline();
    }
    private void SetLaneColor(int lane, int color)
    {
        if (lane == 0)
        {
            if (color == 0)
            {
                leftLane.color = 0;
                leftLineRenderer.material.SetColor("PlayerLineColor", GameColors.instance.laneColorOne);
            }
            else if (color == 1)
            {
                leftLane.color = 1;
                leftLineRenderer.material.SetColor("PlayerLineColor", GameColors.instance.laneColorTwo);
            }
        }
        else if (lane == 1)
        {
            if (color == 0)
            {
                rightLane.color = 0;
                rightLineRenderer.material.SetColor("PlayerLineColor", GameColors.instance.laneColorThree);
            }
            else if (color == 1)
            {
                rightLane.color = 1;
                rightLineRenderer.material.SetColor("PlayerLineColor", GameColors.instance.laneColorFour);
            }
        }
    }

    public void SetLaneToNextColor(int lane) //Call when the next hitline to be hit is in the left lane
    {
        if (Conductor.instance.GetHitlines(lane).Count != 0)
        {
            switch (Conductor.instance.GetHitlines(lane).First().HitlineColor)
            {
                case 0:
                    SetLaneColor(lane, 0);
                    break;

                case 1:
                    SetLaneColor(lane, 1);
                    break;

                default:
                    SetLaneColor(lane, 0);
                    Debug.Log("Default case triggered in SetLaneToNextColor");
                    break;
            }
        }
        else
        {
            Debug.Log("hitlines lists empty!");
            return;
        }
    }

    /*Input system methods*/
    public void OnStrumLeft(InputValue value)
    {
        StrumLeft();
    }
    public void OnStrumRight(InputValue value)
    {
        StrumRight();
    }
    public void OnChangeColorLeftRed(InputValue value)
    {
        SetLaneColor(0, 0);
    }
    public void OnChangeColorLeftGreen(InputValue value)
    {
        SetLaneColor(0, 1);
    }
    public void OnChangeColorRightBlue(InputValue value)
    {
        SetLaneColor(1, 0);
    }
    public void OnChangeColorRightYellow(InputValue value)
    {
        SetLaneColor(1, 1);
    }
}

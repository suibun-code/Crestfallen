using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public struct StrumLane
{
    public string name;

    public List<HitLine> activeHitLines;
    public Renderer renderer;
    public Animator animator;
    public float color;
}

public class PlayerLineInput : Singleton<PlayerLineInput>
{
    private float accuracy;
    private HitLine nextHitline;

    //Lanes
    public StrumLane leftLane;
    public StrumLane rightLane;

    //Components
    public Renderer leftLineRenderer;
    public Renderer rightLineRenderer;
    public Animator leftAnimator;
    public Animator rightAnimator;

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

        leftLane.renderer.material.SetColor("PlayerLineColor", PlayerLineColor.red);
        rightLane.renderer.material.SetColor("PlayerLineColor", PlayerLineColor.blue);
    }

    public void OnChangeColorLeftRed(InputValue value)
    {
        ChangeColorLeftRed();
    }
    public void ChangeColorLeftRed()
    {
        leftLane.color = 0;
        leftLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.red);
    }

    public void OnChangeColorLeftGreen(InputValue value)
    {
        ChangeColorLeftGreen();
    }
    public void ChangeColorLeftGreen()
    {
        leftLane.color = 1;
        leftLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.green);
    }

    public void OnChangeColorRightBlue(InputValue value)
    {
        ChangeColorRightBlue();
    }
    public void ChangeColorRightBlue()
    {
        rightLane.color = 0;
        rightLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.blue);
    }

    public void OnChangeColorRightYellow(InputValue value)
    {
        ChangeColorRightYellow();
    }
    public void ChangeColorRightYellow()
    {
        rightLane.color = 1;
        rightLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.yellow);
    }

    public void OnStrumLeft(InputValue value)
    {
        StrumLeft();
    }
    public void OnStrumRight(InputValue value)
    {
        StrumRight();
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
        {
            Debug.Log("No more " + lane.name + " hitlines!");
            return;
        }

        accuracy = Mathf.Abs(nextHitline.positionInSeconds - Conductor.instance.songPosition);
        Debug.Log("Accuracy: " + accuracy);

        if (accuracy >= 0.25f) //Accuracy not close enough to count the hit. Do nothing
            return;

        if (lane.color != nextHitline.hitLineColor)//if the colors don't match
        {
            Missed();
            return;
        }

        if (accuracy >= 0.15f)//early or late by half a beat
            ScoreTracker.instance.HitBad();

        else if (accuracy >= 0.06f)//early or late by half a beat
            ScoreTracker.instance.HitGreat();

        else if (accuracy >= 0f)//early or late by half a beat
            ScoreTracker.instance.HitPerfect();

        RemoveHitline(nextHitline);
    }

    public void Missed()
    {
        ScoreTracker.instance.HitMiss();
        RemoveHitline(nextHitline);
    }

    public void SetLeftToNextColor()
    {
        if (Conductor.instance.leftHitlines.Count == 0)
        {
            Debug.Log("Left hitlines lists empty!");
            return;
        }

        switch (Conductor.instance.leftHitlines.First().hitLineColor)
        {
            case 0:
                ChangeColorLeftRed();
                break;

            case 1:
                ChangeColorLeftGreen();
                break;
        }
    }
    public void SetRightToNextColor()
    {
        if (Conductor.instance.rightHitlines.Count == 0)
        {
            Debug.Log("Right hitlines lists empty!");
            return;
        }

        switch (Conductor.instance.rightHitlines.First().hitLineColor)
        {
            case 0:
                ChangeColorRightBlue();
                break;

            case 1:
                ChangeColorRightYellow();
                break;
        }
    }

    public void RemoveHitline(HitLine hitline) //Destroy the hitline and remove it from its respective array
    {
        nextHitline.RemoveFromList();
        Destroy(nextHitline.gameObject);
    }
}

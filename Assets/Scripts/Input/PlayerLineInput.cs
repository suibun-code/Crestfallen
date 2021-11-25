using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerLineInput : Singleton<PlayerLineInput>
{
    //Current color
    public static float leftLineColor = 0;
    public static float rightLineColor = 0;

    //Components
    public Renderer leftLineRenderer;
    public Renderer rightLineRenderer;

    public Animator leftAnimator;
    public Animator rightAnimator;

    private float accuracy;
    private HitLine nextHitline;

    void Start()
    {
        leftLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.red);
        rightLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.blue);
    }

    public void OnChangeColorLeftRed(InputValue value)
    {
        ChangeColorLeftRed();
    }

    public void OnChangeColorLeftGreen(InputValue value)
    {
        ChangeColorLeftGreen();
    }

    public void OnChangeColorRightBlue(InputValue value)
    {
        ChangeColorRightBlue();
    }

    public void OnChangeColorRightYellow(InputValue value)
    {
        ChangeColorRightYellow();
    }

    public void OnStrumLeft(InputValue value)
    {
        StrumLeft();
    }

    public void OnStrumRight(InputValue value)
    {
        StrumRight();
    }

    public void ChangeColorLeftRed()
    {
        leftLineColor = 0;
        leftLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.red);
    }

    public void ChangeColorLeftGreen()
    {
        leftLineColor = 1;
        leftLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.green);
    }

    public void ChangeColorRightBlue()
    {
        rightLineColor = 0;
        rightLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.blue);
    }

    public void ChangeColorRightYellow()
    {
        rightLineColor = 1;
        rightLineRenderer.material.SetColor("PlayerLineColor", PlayerLineColor.yellow);
    }

    public void StrumLeft()
    {
        SongManager.instance.PlayHitSound();
        leftAnimator.Play(0);

        if (Conductor.instance.leftHitlines.Count != 0) //Confirm the hitlines array is not empty
        {
            nextHitline = Conductor.instance.leftHitlines.First();
        }
        else
        {
            Debug.Log("No more left side hitlines!");
            return;
        }

        accuracy = nextHitline.beat - Conductor.instance.songPosInBeats;
        Debug.Log("Accuracy: " + accuracy);

        if (accuracy >= 0.25f) //Accuracy not close enough to count the hit. Do nothing
            return;

        if (leftLineColor != nextHitline.hitLineColor)//if the colors don't match
        {
            Missed();
            return;
        }

        if (Mathf.Abs(accuracy) >= 0.15f)//early or late by half a beat
        {
            ScoreTracker.instance.HitBad();
        }
        else if (Mathf.Abs(accuracy) >= 0.05f)//early or late by half a beat
        {
            ScoreTracker.instance.HitGreat();
        }
        else if (Mathf.Abs(accuracy) >= 0f)//early or late by half a beat
        {
            ScoreTracker.instance.HitPerfect();
        }

        nextHitline.RemoveFromList();
        Destroy(nextHitline.gameObject);
    }

    public void StrumRight()
    {
        SongManager.instance.PlayHitSound();
        rightAnimator.Play(0);

        if (Conductor.instance.rightHitlines.Count != 0)
        {
            nextHitline = Conductor.instance.rightHitlines.First();
        }
        else
        {
            Debug.Log("No more right side hitlines!");
            return;
        }

        accuracy = nextHitline.beat - Conductor.instance.songPosInBeats;
        Debug.Log("Accuracy: " + accuracy);

        if (accuracy >= 0.25f) //Accuracy not close enough to count the hit. Do nothing
            return;

        if (rightLineColor != nextHitline.hitLineColor)//if the colors don't match
        {
            Missed();
            return;
        }

        if (Mathf.Abs(accuracy) >= 0.15f)//early or late by half a beat
        {
            ScoreTracker.instance.HitBad();
        }
        else if (Mathf.Abs(accuracy) >= 0.06f)//early or late by half a beat
        {
            ScoreTracker.instance.HitGreat();
        }
        else if (Mathf.Abs(accuracy) >= 0f)//early or late by half a beat
        {
            ScoreTracker.instance.HitPerfect();
        }

        nextHitline.RemoveFromList();
        Destroy(nextHitline.gameObject);
    }

    public void Missed()
    {
        ScoreTracker.instance.HitMiss();
        nextHitline.RemoveFromList();
        Destroy(nextHitline.gameObject);
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
}

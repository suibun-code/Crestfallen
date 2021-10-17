using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitDetector : MonoBehaviour
{
    public int accuracy = 0;

    public Animator playerLineHit;

    List<GameObject> currentCollisions = new List<GameObject>();

    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitLine")
            currentCollisions.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        currentCollisions.Remove(other.gameObject);
    }

    public void OnStrum(InputValue value)
    {
        AudioManager.instance.PlayHitSound();
        playerLineHit.Play(0);

        foreach (GameObject gObject in currentCollisions)
        {
            if (gObject == null)
                continue;

            var hitLine = gObject.GetComponent<HitLine>();

            if (hitLine.hitLineColor == Player.playerLineColor)
            {
                switch(accuracy)
                {
                    case 0:
                        ScoreTracker.instance.HitPerfect();
                        break;
                    case 1:
                        ScoreTracker.instance.HitGreat();
                        break;
                    case 2:
                        ScoreTracker.instance.HitBad();
                        break;
                }

                Destroy(gObject);
            }
            else
            {
                ScoreTracker.instance.ResetCombo();
                ScoreTracker.instance.HitMiss();
                Destroy(gObject);
            }

            ScoreTracker.instance.UpdateTexts();
        }
    }
}
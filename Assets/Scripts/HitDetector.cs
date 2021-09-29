using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitDetector : MonoBehaviour
{
    private ScoreTracker scoreTracker;
    private BoxCollider hitCollider;

    public Animator playerLineHit;

    List<GameObject> currentCollisions = new List<GameObject>();

    void Start()
    {
        hitCollider = GetComponent<BoxCollider>();
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
        AudioManager.instance.Play();
        playerLineHit.Play(0);

        foreach (GameObject gObject in currentCollisions)
        {
            if (gObject == null)
                continue;

            var hitLine = gObject.GetComponent<HitLineBehaviour>();

            if (hitLine.hitLineColor == Player.playerLineColor)
            {
                ScoreTracker.instance.combo += 1;
                ScoreTracker.instance.scoreMultiplier = 1 + ScoreTracker.instance.combo / 9.0f;
                ScoreTracker.instance.score += 1 * ScoreTracker.instance.scoreMultiplier;

                Destroy(gObject);
            }
            else
            {
                ScoreTracker.instance.ResetCombo();
            }

            ScoreTracker.instance.UpdateTexts();
        }
    }
}
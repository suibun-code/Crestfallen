using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitDetector : MonoBehaviour
{
    private ScoreTracker scoreTracker;

    private BoxCollider hitCollider;

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
        foreach (GameObject gObject in currentCollisions)
        {
            if (gObject == null)
                continue;

            var hitLine = gObject.GetComponent<HitLineBehaviour>();

            if (hitLine.hitLineColor == Player.playerLineColor)
            {
                Debug.Log("Hit");
                Destroy(gObject);
            }
            else
            {
                Debug.Log("Wrong Color");
            }
        }
    }
}

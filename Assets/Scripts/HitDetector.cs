using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    private ScoreTracker scoreTracker;

    public BoxCollider hitCollider;

    void Start()
    {
        hitCollider = GetComponent<BoxCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {

    }
}

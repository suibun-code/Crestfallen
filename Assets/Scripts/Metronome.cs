using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Metronome : MonoBehaviour
{
    public int accuracy = 0;

    public Animator playerLineHit;

    List<GameObject> currentCollisions = new List<GameObject>();

    public void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "HitLine")
        //    AudioManager.instance.PlayHitSound();
    }
}
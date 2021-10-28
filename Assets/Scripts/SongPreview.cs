using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPreview : Singleton<SongPreview>
{
    public AudioSource music;

    new private void Awake()
    {
        music = GetComponent<AudioSource>();
    }
}

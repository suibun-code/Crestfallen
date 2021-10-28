using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelector : Singleton<SongSelector>
{
    public Cell currentCell;
    public AudioSource music;

    new private void Awake()
    {
        music = GetComponent<AudioSource>();

        DontDestroyOnLoad(this.gameObject);
    }
}

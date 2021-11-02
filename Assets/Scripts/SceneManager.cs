using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public void ChangeSceneToGameplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void ChangeSceneToSongSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SongSelect");
    }
}

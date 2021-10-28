using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    public void ChangeSceneToGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ChangeSceneToSongSelect()
    {
        SceneManager.LoadScene("SongSelect");
    }
}

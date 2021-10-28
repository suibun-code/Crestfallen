using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
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

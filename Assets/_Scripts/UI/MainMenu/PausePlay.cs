using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePlay : MonoBehaviour
{
    private Image _image;
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    private bool isPaused = false;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void TogglePlayPauseTexture()
    {
        if (isPaused)
        {
            SongManager.instance.PlayMusic();
            _image.sprite = pauseSprite;
            isPaused = false;
        }
        else
        {
            SongManager.instance.PauseMusic();
            _image.sprite = playSprite;
            isPaused = true;
        }

        Debug.Log("TogglePlayPause");
    }
}

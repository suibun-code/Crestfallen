using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject eventSystem;

    private GameObject currentMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject uploadMusic;
    [SerializeField] private GameObject beatmapInfo;
    [SerializeField] private GameObject songSelect;

    void Start()
    {
        currentMenu = mainMenu;
    }

    public void SwitchMenu(string newMenu)
    {
        eventSystem.SetActive(true);
        currentMenu.SetActive(false);

        switch (newMenu)
        {
            case "MainMenu":
                currentMenu = mainMenu;
                break;

            case "UploadMusic":
                currentMenu = uploadMusic;
                break;

            case "BeatmapInfo":
                currentMenu = beatmapInfo;
                break;

            case "SongSelect":
                currentMenu = songSelect;
                break;

            default:
                Debug.Log("Menu '" + newMenu + "' not found.");
                break;
        }

        currentMenu.SetActive(true);
    }
}

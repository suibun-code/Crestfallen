using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject eventSystem;

    //current menu gameobject
    private GameObject currentMenu;

    [SerializeField] private List<GameObject> menus;

    void Start()
    {
        //initialize current menu to the main menu
        currentMenu = menus[0];

    }

    public void SwitchMenu(string newMenu)
    {
        //enable event system
        eventSystem.SetActive(true);

        //disable current menu gameobject
        currentMenu.SetActive(false);

        //assign new menu gameobject to current menu
        foreach (var menu in menus)
        {
            if (menu.name == newMenu)
            {
                currentMenu = menu;
                currentMenu.SetActive(true);
                return;
            }
        }

        //If nothing is found and the function doesn't return, a menu was not found
        Debug.LogWarning("Menu not found!");
    }
}

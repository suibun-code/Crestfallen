using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Autohit : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void ToggleAutoHit()
    {
        if (Conductor.instance.autoHit == false)
        {
            Conductor.instance.autoHit = true;
            ColorBlock colors = button.colors;
            colors.normalColor = Color.green;
            colors.highlightedColor = Color.green;
            colors.pressedColor = Color.green;
            colors.selectedColor = Color.green;
            button.colors = colors;
            text.SetText("AutoHit ON");
        }
        else
        {
            Conductor.instance.autoHit = false;
            ColorBlock colors = button.colors;
            colors.normalColor = Color.red;
            colors.highlightedColor = Color.red;
            colors.pressedColor = Color.red;
            colors.selectedColor = Color.red;
            button.colors = colors;
            text.SetText("AutoHit OFF");

        }
    }
}

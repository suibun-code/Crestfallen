using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Autohit : MonoBehaviour
{
    public Color enabledColor;
    public Color disabledColor;

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
            colors.normalColor = enabledColor;
            colors.highlightedColor = enabledColor;
            colors.pressedColor = enabledColor;
            colors.selectedColor = enabledColor;

            button.colors = colors;

            text.SetText("AUTOHIT ON");
        }
        else
        {
            Conductor.instance.autoHit = false;

            ColorBlock colors = button.colors;
            colors.normalColor = disabledColor;
            colors.highlightedColor = disabledColor;
            colors.pressedColor = disabledColor;
            colors.selectedColor = disabledColor;

            button.colors = colors;

            text.SetText("AUTOHIT OFF");

        }
    }
}

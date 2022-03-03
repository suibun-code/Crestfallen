using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SwitchCanvas : MonoBehaviour
{
    private Canvas toSwitch;
    public Camera cameraRef;
    public Color backgroundColorAfterSwitch;

    private void Start()
    {
        toSwitch = GetComponent<Canvas>();
    }

    public void Switch(Canvas switchTo)
    {
        toSwitch.enabled = false;
        switchTo.enabled = true;
    }

    public void ChangeBackgroundColor()
    {
        cameraRef.backgroundColor = backgroundColorAfterSwitch;
    }
}

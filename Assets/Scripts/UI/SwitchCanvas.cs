using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCanvas : MonoBehaviour
{
    public Canvas toSwitch;
    public Canvas switchTo;

    public void Switch()
    {
        toSwitch.enabled = false;
        switchTo.enabled = true;
        SelectMusic.instance.StopClip();
    }
}

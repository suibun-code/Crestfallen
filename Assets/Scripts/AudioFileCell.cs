using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFileCell : MonoBehaviour
{
    public delegate void ClickAction(AudioFileCell audioFileCell);
    public static event ClickAction OnClick;

    [ReadOnly] public string fileName;

    public void OnSetCurrentCell()
    {
        var audioFileCell = this;

        if (OnClick != null)
            OnClick(audioFileCell);
    }
}

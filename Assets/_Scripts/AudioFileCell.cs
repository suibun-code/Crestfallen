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
        if (OnClick != null)
            OnClick(this);
    }
}

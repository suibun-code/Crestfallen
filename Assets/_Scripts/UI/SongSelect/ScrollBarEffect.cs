using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarEffect : MonoBehaviour
{
    public Material material;
    public Scrollbar scrollBar;

    public void SetScrollValue()
    {
        material.SetFloat("ScrollValue", scrollBar.value);
    }
}

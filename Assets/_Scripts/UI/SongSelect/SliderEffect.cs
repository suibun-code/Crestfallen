using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderEffect : MonoBehaviour
{
    public Material material;
    public Slider slider;

    public void SetScrollValue()
    {
        material.SetFloat("ScrollValue", slider.normalizedValue);
    }
}

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
        Debug.Log(slider.normalizedValue);
        material.SetFloat("ScrollValue", slider.normalizedValue);
    }
}

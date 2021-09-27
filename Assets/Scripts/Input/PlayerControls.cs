using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //Current Color
    public CurrentColor currentColor = CurrentColor.RED;

    //Components
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        
    }

    public void OnChangeColorRed(InputValue value)
    {
        currentColor = CurrentColor.RED;
        renderer.material.SetColor("_BaseColor", new Color32(184, 15, 10, 255));
        Debug.Log(renderer.material.GetColor("_Color"));
        Debug.Log("Pressed Red");
    }
    
        public void OnChangeColorGreen(InputValue value)
    {
        currentColor = CurrentColor.GREEN;
        renderer.material.SetColor("_BaseColor", new Color32(50, 205, 50, 255));
        Debug.Log("Pressed Green");
    }

    public void OnChangeColorBlue(InputValue value)
    {
        currentColor = CurrentColor.BLUE;
        renderer.material.SetColor("_BaseColor", new Color32(65, 105, 225, 255));
        Debug.Log("Pressed Blue");
    }

    public void OnChangeColorPurple(InputValue value)
    {
        currentColor = CurrentColor.PURPLE;
        renderer.material.SetColor("_BaseColor", new Color32(102, 51, 153, 255));
        Debug.Log("Pressed Purple");
    }
}

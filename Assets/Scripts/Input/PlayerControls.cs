using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //Current color
    public HitLineEnum hitLineColor = HitLineEnum.RED;

    //Components
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void OnChangeColorRed(InputValue value)
    {
        hitLineColor = HitLineEnum.RED;
        _renderer.material.SetColor("_BaseColor", HitLineColor.red);
    }
    
        public void OnChangeColorGreen(InputValue value)
    {
        hitLineColor = HitLineEnum.GREEN;
        _renderer.material.SetColor("_BaseColor", HitLineColor.green);
    }

    public void OnChangeColorBlue(InputValue value)
    {
        hitLineColor = HitLineEnum.BLUE;
        _renderer.material.SetColor("_BaseColor", HitLineColor.blue);
    }

    public void OnChangeColorYellow(InputValue value)
    {
        hitLineColor = HitLineEnum.YELLOW;
        _renderer.material.SetColor("_BaseColor", HitLineColor.yellow);
    }
}

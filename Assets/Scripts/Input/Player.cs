using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Current color
    public static LineColorEnum playerLineColor = LineColorEnum.RED;

    //Components
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

   void Start()
    {
        _renderer.material.SetColor("PlayerLineColor", PlayerLineColor.red);
    }

    public void OnChangeColorRed(InputValue value)
    {
        playerLineColor = LineColorEnum.RED;
        _renderer.material.SetColor("PlayerLineColor", PlayerLineColor.red);
    }
    
        public void OnChangeColorGreen(InputValue value)
    {
        playerLineColor = LineColorEnum.GREEN;
        _renderer.material.SetColor("PlayerLineColor", PlayerLineColor.green);
    }

    public void OnChangeColorBlue(InputValue value)
    {
        playerLineColor = LineColorEnum.BLUE;
        _renderer.material.SetColor("PlayerLineColor", PlayerLineColor.blue);
    }

    public void OnChangeColorYellow(InputValue value)
    {
        playerLineColor = LineColorEnum.YELLOW;
        _renderer.material.SetColor("PlayerLineColor", PlayerLineColor.yellow);
    }
}

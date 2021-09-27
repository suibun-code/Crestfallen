using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMonoBehaviour : MonoBehaviour
{
    private GameInputActions inputActions;
    protected void Awake()
    {
        inputActions = new GameInputActions();
    }

    protected void OnEnable()
    {
        inputActions.Enable();
    }

    protected void OnDisable()
    {
        inputActions.Disable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMonoBehaviour : MonoBehaviour
{
    public static GameInputActions inputActions;
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

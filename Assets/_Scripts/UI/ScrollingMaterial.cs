using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingMaterial : MonoBehaviour
{
    [SerializeField] private float x, y;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _renderer.material.mainTextureOffset = new Vector2(Time.time * x, Time.time * y);
    }
}

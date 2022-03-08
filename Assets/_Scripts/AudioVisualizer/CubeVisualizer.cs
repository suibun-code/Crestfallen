using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    public int _band;

    public float _startScale;
    public float _scaleMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (AudioSpectrum._freqBand[_band] * _scaleMultiplier) + _startScale);
    }
}

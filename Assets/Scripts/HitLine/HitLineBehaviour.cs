using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLineBehaviour : MonoBehaviour
{
    //Hitline speed
    public float hitLineSpeed = 20.0f;

    //Current color
    public LineColorEnum hitLineColor = LineColorEnum.RED;

    //Components
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (transform.position.z <= 3.4f)
        {
            ScoreTracker.instance.ResetCombo();
            ScoreTracker.instance.HitMiss();
            Destroy(gameObject);
        }

        transform.Translate(0.0f, 0.0f, -hitLineSpeed * Time.deltaTime);
    }

    public void SetColor(LineColorEnum color)
    {
        switch (color)
        {
            case LineColorEnum.RED:
                _renderer.material.SetColor("_BaseColor", HitLineColor.red);
                break;

            case LineColorEnum.GREEN:
                _renderer.material.SetColor("_BaseColor", HitLineColor.green);
                break;

            case LineColorEnum.BLUE:
                _renderer.material.SetColor("_BaseColor", HitLineColor.blue);
                break;

            case LineColorEnum.YELLOW:
                _renderer.material.SetColor("_BaseColor", HitLineColor.yellow);
                break;
        }

        hitLineColor = color;
    }
}

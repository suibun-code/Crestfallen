using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLineBehaviour : MonoBehaviour
{
    //Hitline speed
    public float hitLineSpeed = 20.0f;

    //Current color
    public HitLineEnum hitLineColor = HitLineEnum.RED;

    //Components
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (transform.position.z <= 0.0f)
            Destroy(gameObject);

        transform.Translate(0.0f, 0.0f, -hitLineSpeed * Time.deltaTime);
    }

    public void SetColor(HitLineEnum color)
    {
        switch (color)
        {
            case HitLineEnum.RED:
                _renderer.material.SetColor("_BaseColor", HitLineColor.red);
                break;

            case HitLineEnum.GREEN:
                _renderer.material.SetColor("_BaseColor", HitLineColor.green);
                break;

            case HitLineEnum.BLUE:
                _renderer.material.SetColor("_BaseColor", HitLineColor.blue);
                break;

            case HitLineEnum.YELLOW:
                _renderer.material.SetColor("_BaseColor", HitLineColor.yellow);
                break;
        }

        hitLineColor = color;
    }
}

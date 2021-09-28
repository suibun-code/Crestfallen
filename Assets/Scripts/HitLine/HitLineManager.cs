using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLineManager : MonoBehaviour
{
    public bool spawning = true;
    public float spawnRateInSeconds = 1.5f;
    public float hitLineSpeed = 20.0f;

    public GameObject prefab;

    private int lastColor;
    private int currentColor;

    void Start()
    {
        StartCoroutine(SpawnerCoroutine());
    }

    IEnumerator SpawnerCoroutine()
    {
        while (spawning)
        {
            GameObject hitLine = Instantiate(prefab);
            hitLine.transform.parent = transform;

            currentColor = Random.Range(0, (int)LineColorEnum.COUNT);
            while (currentColor == lastColor)
                currentColor = Random.Range(0, (int)LineColorEnum.COUNT);

            var hitLineBehaviour = hitLine.GetComponent<HitLineBehaviour>();
            hitLineBehaviour.SetColor((LineColorEnum)currentColor);
            hitLineBehaviour.hitLineSpeed = hitLineSpeed;

            lastColor = currentColor;

            yield return new WaitForSeconds(spawnRateInSeconds);
        }
    }
}

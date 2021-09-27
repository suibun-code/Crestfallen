using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLineManager : MonoBehaviour
{
    public bool spawning = true;
    public float spawnRateInSeconds = 0.7f;
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

            currentColor = Random.Range(0, (int)HitLineEnum.COUNT);
            while (currentColor == lastColor)
                currentColor = Random.Range(0, (int)HitLineEnum.COUNT);

            var hitLineBehaviour = hitLine.GetComponent<HitLineBehaviour>();
            hitLineBehaviour.SetColor((HitLineEnum)currentColor);
            hitLineBehaviour.hitLineSpeed = hitLineSpeed;

            lastColor = currentColor;

            yield return new WaitForSeconds(spawnRateInSeconds);
        }
    }
}

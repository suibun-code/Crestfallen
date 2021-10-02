using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLineManager : MonoBehaviour
{
    public float musicOffset;

    public bool spawning = true;
    public float spawnRateInSeconds = 1.5f;
    public float hitLineSpeed = 40.0f;
    public float timeToHit = 0.0f;

    public GameObject prefab;

    //private int lastColor;
    private int currentColor;

    void Start()
    {
        StartCoroutine(MusicOffset());

        hitLineSpeed = 7.5f * (200.0f / 60.0f);
        timeToHit = 75f / hitLineSpeed;
        Debug.Log("Hitline speed: " + hitLineSpeed);
        Debug.Log("Hitline time to hit: " + timeToHit);
    }

    IEnumerator MusicOffset()
    {
        if (musicOffset < 0.0f)
        {
            AudioManager.instance.PlayMusic();
            StartCoroutine(SpawnerCoroutine(musicOffset));
        }
        else
        {
            StartCoroutine(SpawnerCoroutine(0));
            yield return new WaitForSeconds(musicOffset);
            AudioManager.instance.PlayMusic();
        }

        AudioManager.instance.test = Time.time; //yotyodfijgfdklghjnsdfjklgjhfdsklgjfdsklgdfjgk;lfdgjfdkl;j
    }

    IEnumerator SpawnerCoroutine(float waitFor)
    {
        yield return new WaitForSeconds(Mathf.Abs(waitFor));

        while (spawning)
        {
            GameObject hitLine = Instantiate(prefab);
            hitLine.transform.parent = transform;

            currentColor = Random.Range(0, (int)LineColorEnum.COUNT);
            //while (currentColor == lastColor)
            //currentColor = Random.Range(0, (int)LineColorEnum.COUNT);

            var hitLineBehaviour = hitLine.GetComponent<HitLineBehaviour>();
            hitLineBehaviour.SetColor((LineColorEnum)currentColor);

            hitLineBehaviour.hitLineSpeed = hitLineSpeed;

            //lastColor = currentColor;

            AudioManager.instance.PlayHitSound();

            yield return new WaitForSeconds(spawnRateInSeconds);
        }
    }
}

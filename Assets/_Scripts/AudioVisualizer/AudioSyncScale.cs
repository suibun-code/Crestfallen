using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    private Coroutine scaleCoroutine;

    //public Vector3 beatScale;
    private Vector3 test;
    public Vector3 restScale;

    public int _band;
    public float _startScale;
    public float _scaleMultiplier;

    private void Start()
    {
        //test = new Vector3(transform.localScale.x, transform.localScale.y, (AudioSpectrum._freqBand[_band] * _scaleMultiplier));
    }

    public override void OnBeat()
    {
        base.OnBeat();

        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(MoveToScale(test));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }

    private IEnumerator MoveToScale(Vector3 beatScale)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 initialScale = currentScale;

        float timer = 0;

        while (currentScale != beatScale)
        {
            test = new Vector3(transform.localScale.x, transform.localScale.y, (AudioSpectrum._freqBand[_band] * _scaleMultiplier));
            currentScale = Vector3.Lerp(initialScale, test, timer / timeToBeat);

            //currentScale = Vector3.Lerp(initialScale, beatScale, timer / timeToBeat);

            timer += Time.deltaTime; //TRY CHANGING THIS TO SONG POSITION

            transform.localScale = currentScale;

            yield return null;
        }

        m_isBeat = false;
    }
}

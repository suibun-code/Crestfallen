using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    private Coroutine scaleCoroutine;

    public Vector3 beatScale;
    public Vector3 restScale;

    private void Start()
    {

    }

    public override void OnBeat()
    {
        base.OnBeat();

        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(MoveToScale(beatScale));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }

    private IEnumerator MoveToScale(Vector3 target)
    {
        Vector3 curr = transform.localScale;
        Vector3 initial = curr;
        float timer = 0;

        while (curr != target)
        {
            curr = Vector3.Lerp(initial, target, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = curr;

            yield return null;
        }

        m_isBeat = false;
    }
}

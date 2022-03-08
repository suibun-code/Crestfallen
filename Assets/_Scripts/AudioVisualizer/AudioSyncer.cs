using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    private float m_timer = 0;
    //private float lastbeat = 0f;
    //private float m_audioValue;
    //private float m_previousAudioValue;

    protected bool m_isBeat;

    //public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;

    public virtual void OnBeat()
    {
        //Debug.Log("Beat");
        m_timer = 0;
        m_isBeat = true;
    }

    public virtual void OnUpdate()
    {
        //m_previousAudioValue = m_audioValue;
        //m_audioValue = AudioSpectrum.spectrumValue;

        //if (m_previousAudioValue > bias && m_audioValue <= bias)
        //{
        //    if (m_timer > timeStep)
        //        OnBeat();
        //}

        //if (m_previousAudioValue <= bias && m_audioValue > bias)
        //{
        //    if (m_timer > timeStep)
        //        OnBeat();
        //}

        //if (Conductor.instance.SongPosition > lastbeat + Conductor.instance.Crotchet)
        //{
        //    OnBeat();
        //    lastbeat += Conductor.instance.Crotchet;
        //}

        if (m_timer > timeStep)
            OnBeat();

        m_timer += Time.deltaTime;
    }

    void Update()
    {
        OnUpdate();
    }
}

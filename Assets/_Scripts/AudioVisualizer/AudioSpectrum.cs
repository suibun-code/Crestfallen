using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    public AudioSource _audioSource;

    public static float spectrumValue { get; private set; }

    public static float[] m_samples;
    public static float[] _freqBand;

    // Start is called before the first frame update
    void Start()
    {
        m_samples = new float[512];
        _freqBand = new float[8];
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
    }

    public void GetSpectrumAudioSource()
    {
        //AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);
        _audioSource.GetSpectrumData(m_samples, 0, FFTWindow.Blackman);

        if (m_samples != null && m_samples.Length > 0)
        {
            spectrumValue = m_samples[0] * 100;
        }
    }

    public void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
                sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                average += m_samples[count] * (count + 1);
                count++;
            }

            average /= sampleCount;

            _freqBand[i] = average * 10;
        }
    }
}

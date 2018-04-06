using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource m_AudioSource;

    public static float[] m_Samples = new float[512];

	void Start ()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

	void Update ()
    {
        GetSpectrumAudioSource();
    }

    void GetSpectrumAudioSource()
    {
        // Get all the samples from the audio source
        m_AudioSource.GetSpectrumData(m_Samples,0, FFTWindow.Blackman);
    }
}

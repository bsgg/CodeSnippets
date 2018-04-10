using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource m_AudioSource;

    public static float[] m_Samples = new float[512];

    public static float[] m_FrequenceBands = new float[8];

	void Start ()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

	void Update ()
    {
        GetSpectrumAudioSource();
        MakeFrequenceBands();
    }

    void GetSpectrumAudioSource()
    {
        // Get all the samples from the audio source
        m_AudioSource.GetSpectrumData(m_Samples,0, FFTWindow.Blackman);
    }

    void MakeFrequenceBands()
    {
        // Current song 22050Hz / 512 samples = 43hertz per sample
        // Bands
        // 60 - 250 Hertz
        // 250 - 500 Hertz
        // 500 - 2000 Hertz
        // 2000 - 4000 Hertz
        // 4000 - 6000 Hertz
        // 6000 - 20000 Hertz

        // Create frequence bands (8 in total)
        // 0. - 2 Samples = 86 Hertzs 
        // 1. - 4 Samples = 172 Hertzs  - Range 87 - 258
        // 2. - 8 Samples = 344 Hertzs  - Range 259 - 602
        // 3. - 16 Samples = 688 Hertzs  - Range 605 - 1290
        // 4. - 32 Samples = 1376 Hertzs  - Range 1291 - 2666
        // 5. - 64 Samples = 2752 Hertzs  - Range 2667 - 5418
        // 6. - 128 Samples = 5504 Hertzs  - Range 5419 - 10922
        // 7. - 256 Samples = 11008 Hertzs  - Range 10923 - 21930
        // Total 510

        // Current sample
        int count = 0;
        for (int i=0; i<8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            
        }

    }
}

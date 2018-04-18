using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioVisualizer
{
    [RequireComponent(typeof(Light))]
    public class LightOnAudio : MonoBehaviour
    {

        [SerializeField] private int m_Band;
        [SerializeField] private float m_MinIntensity;
        [SerializeField] private float m_MaxIntensity;
        private Light m_Light;

        private void Start()
        {
            m_Light = GetComponent<Light>();
        }

        private void Update()
        {
            m_Light.intensity = (AudioPeer.AudioBandBuffer[m_Band] * (m_MaxIntensity - m_MinIntensity)) + m_MinIntensity;
        }
    }
}

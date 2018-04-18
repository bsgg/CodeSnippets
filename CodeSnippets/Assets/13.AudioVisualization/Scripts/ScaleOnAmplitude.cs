using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AudioVisualizer
{
    public class ScaleOnAmplitude : MonoBehaviour
    {
        [SerializeField] private float m_StartScale;
        [SerializeField] private float m_MaxScale;
        [SerializeField] private bool m_UseBuffer;

        private Material m_Material;
        [SerializeField] private float m_Red;
        [SerializeField] private float m_Green;
        [SerializeField] private float m_Blue;


        private void Awake()
        {
            m_Material = GetComponent<MeshRenderer>().materials[0];
        }

        
        // Update is called once per frame
        void Update()
        {
            float amplitudeScale = m_StartScale;
            float amplitudeColor = 1;
            if (!m_UseBuffer)
            {  
                if (!float.IsNaN(AudioPeer.AmplitudeBuffer))
                {
                    amplitudeScale = (AudioPeer.AmplitudeBuffer * m_MaxScale) + m_StartScale;
                    amplitudeColor = AudioPeer.AmplitudeBuffer;
                }
            }
            else
            {
                if(!float.IsNaN(AudioPeer.Amplitude))
                {
                    amplitudeScale = (AudioPeer.Amplitude * m_MaxScale) + m_StartScale;
                    amplitudeColor = AudioPeer.Amplitude;
                }
            }

            transform.localScale = new Vector3(
                   amplitudeScale,
                   amplitudeScale,
                   amplitudeScale);
            Color c = new Color(
                   m_Red * amplitudeColor,
                   m_Green * amplitudeColor,
                   m_Blue * amplitudeColor);
            m_Material.SetColor("_EmissionColor", c);
        }
    }
}

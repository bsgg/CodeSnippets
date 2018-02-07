using UnityEngine;
using System.Collections;

namespace MisCode
{
    public class FloatingEffect : Effect
    {
        [Header("Amplitude and Speed for floating")]
        [SerializeField] private float  m_Amplitude = 0.5f;
        [SerializeField] private float  m_Speed     = 1.5f;

        [SerializeField] private bool   m_XEffect;
        [SerializeField] private bool   m_YEffect;
        [SerializeField] private bool   m_ZEffect;

        /// <summary>
        /// Steps time and current y of object
        /// </summary>
        private float                   m_Step = 0.0f;
        private float                   m_OrigX = 0;
        private float                   m_OrigY = 0;
        private float                   m_OrigZ = 0;

        protected override void DoStart()
        {
            m_OrigX = transform.position.x;
            m_OrigY = transform.position.y;
            m_OrigZ = transform.position.z;
            base.DoStart();
        }

        protected override void DoUpdate()
        {
            base.DoUpdate();

            // Update y position
            m_Step += Time.deltaTime;
            float stepY = 0;

            float stepX = 0;
            if (m_XEffect)
            {
                stepX = Mathf.Abs(m_Amplitude * Mathf.Sin(m_Speed * m_Step));
            }

            if (m_YEffect)
            {
                stepY = Mathf.Abs(m_Amplitude * Mathf.Sin(m_Speed * m_Step));
            }
            
            float stepZ = 0;
            if (m_ZEffect)
            {
                stepZ = Mathf.Abs(m_Amplitude * Mathf.Sin(m_Speed * m_Step));
            }

            transform.position = new Vector3(m_OrigX + stepX, m_OrigY + stepY, m_OrigZ + stepZ);
        }

    }
}

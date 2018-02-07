using UnityEngine;
using System.Collections;

namespace Effects
{
    public class SpinEffect : Effect
    {
        [Header("Spin Settings")]
        [SerializeField] private float  m_SpinSpeed = 100.0f;
        [SerializeField] private bool   m_XSpin;
        [SerializeField] private bool   m_YSpin;
        [SerializeField] private bool   m_ZSpin;

        private float                   m_OrigXRot = 0.0f;
        private float                   m_OrigYRot = 0.0f;
        private float                   m_OrigZRot = 0.0f;

        protected override void DoStart()
        {
            m_OrigXRot = 0.0f;
            m_OrigYRot = 0.0f;
            m_OrigZRot = 0.0f;
            base.DoStart();
        }

        protected override void DoUpdate()
        {
            base.DoUpdate();

            // Rotation
            if (m_XSpin) m_OrigXRot += Time.deltaTime * m_SpinSpeed;
            if (m_YSpin) m_OrigYRot += Time.deltaTime * m_SpinSpeed;
            if (m_ZSpin) m_OrigZRot += Time.deltaTime * m_SpinSpeed;
            transform.localRotation = Quaternion.Euler(new Vector3(m_OrigXRot, m_OrigYRot, m_OrigZRot));
        }        
    }
}

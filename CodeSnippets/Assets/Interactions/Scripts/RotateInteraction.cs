using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets.Interactions
{
    public class RotateInteraction : BaseInteraction
    {
        [Header("Rotation settings")]
        [SerializeField] private float m_RotationSpeed = 1.0f;
        [SerializeField] private float m_TimeToResetRotation = 1.0f;

        private Quaternion m_OriginalRotation;
        private bool m_ResetRotation = false;
        private float m_ResetRotTime = 0.0f;      


        protected override void DoStart()
        {
            base.DoStart();
            m_OriginalRotation = transform.localRotation;
        }

        protected override void DoUpdate()
        {
            base.DoUpdate();

            // Do the rotation in Start and Touch phase
            if ((m_PhaseInteraction == EPHASEINTERACTION.START) || (m_PhaseInteraction == EPHASEINTERACTION.TOUCH))
            {
                // Rotation
                Vector3 vertRotAxis = transform.InverseTransformDirection(m_InteractionCamera.transform.TransformDirection(Vector3.right)).normalized;
                Vector3 hortRotAxis = transform.InverseTransformDirection(m_InteractionCamera.transform.TransformDirection(Vector3.up)).normalized;

                float horizontalRot = -(Input.mousePosition.x - m_LastTouch.x) * m_RotationSpeed;
                float verticalRot = (Input.mousePosition.y - m_LastTouch.y) * m_RotationSpeed;

                transform.Rotate(vertRotAxis, verticalRot);
                transform.Rotate(hortRotAxis, horizontalRot);

                m_LastTouch = Input.mousePosition;
            }

            // Reset rotation
            if (m_ResetRotation)
            {
                if (m_ResetRotTime < m_TimeToResetRotation)
                {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, m_OriginalRotation, m_ResetRotTime / m_TimeToResetRotation);
                    m_ResetRotTime += Time.deltaTime;
                }else
                {
                    m_ResetRotation = false;
                }
            }
        }

        public void InstantReset()
        {
            transform.localRotation = m_OriginalRotation;
        }

        public void SmoothResetRotation()
        {
            m_ResetRotation = true;
            m_ResetRotTime = 0.0f;
        }   
    }
}

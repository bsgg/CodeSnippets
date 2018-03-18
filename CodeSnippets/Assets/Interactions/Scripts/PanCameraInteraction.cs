using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class PanCameraInteraction : BaseInteraction
    {
        [SerializeField]
        private float m_SpeedPan = 10.0f; 

        private bool m_IsPannig = false;
        private Vector3 m_CurrentEuler = Vector3.zero;
        private Quaternion m_NextQuaternion;

        [Header("Angles of Pan Clamping camera")]
        public float m_UpperClamp = -50.0f;
        public float m_BottomClamp = 50.0f;
        public float m_LeftClamp = -25.0f;
        public float m_RigthClamp = 7.0f;


        protected override void DoUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_LastTouch = Input.mousePosition;
                m_IsPannig = true;
            }

            if (m_IsPannig)
            {
                m_CurrentTouch = Input.mousePosition;
                float xDelta = m_CurrentTouch.x - m_LastTouch.x;
                float yDelta = m_CurrentTouch.y - m_LastTouch.y;
                float speedMovement = 10.0f;

                float nextAngleHoriz = xDelta * speedMovement * Time.deltaTime;
                m_CurrentEuler.y = Mathf.Clamp(m_CurrentEuler.y - nextAngleHoriz, m_LeftClamp, m_RigthClamp);


                float nextAngleVert = yDelta * speedMovement * Time.deltaTime;
                m_CurrentEuler.x = Mathf.Clamp(m_CurrentEuler.x + nextAngleVert, m_UpperClamp, m_BottomClamp);
                m_NextQuaternion = Quaternion.Euler(m_CurrentEuler);

                // Get new position  and apply to camera the new rotation      
                transform.localRotation = Quaternion.Lerp(transform.localRotation, m_NextQuaternion, Time.time * m_SpeedPan);
                m_LastTouch = m_CurrentTouch;
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_IsPannig = false;
            }

        }
        

    }
}

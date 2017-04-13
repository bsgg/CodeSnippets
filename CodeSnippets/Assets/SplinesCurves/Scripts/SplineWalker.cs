using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityCurves
{
    public class SplineWalker : MonoBehaviour
    {
        [SerializeField] private BezierSpline m_Spline;

        [SerializeField]
        private float m_Duration;
        
        private float m_Progress;

        [SerializeField]
        private bool m_LookForward;

        public enum SplineWalkerMode
        {
            Once,
            Loop,
            PingPong
        }

        [SerializeField]
        private SplineWalkerMode m_Mode;
        private bool m_GoingForward = true;

        private void Update()
        {
            if (m_GoingForward)
            {
                m_Progress += Time.deltaTime / m_Duration;
                if (m_Progress > 1.0f)
                {
                    if (m_Mode == SplineWalkerMode.Once)
                    {
                        m_Progress = 1.0f;
                    }
                    else if (m_Mode == SplineWalkerMode.Loop)
                    {
                        m_Progress -= 1.0f;
                    }
                    else
                    {
                        m_Progress = 2.0f - m_Progress;
                        m_GoingForward = false;
                    }
                }

            }
            else
            {
                m_Progress -= Time.deltaTime / m_Duration;
                if (m_Progress < 0.0f)
                {
                    m_Progress = -m_Progress;
                    m_GoingForward = true;
                }

            }

            Vector3 position = m_Spline.GetPoint(m_Progress);
            transform.localPosition = position;

            if (m_LookForward)
            {
                transform.LookAt(position + m_Spline.GetDirection(m_Progress));
            }
            
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UtilityCurves
{
    public class SplineWalker : MonoBehaviour
    {
        public enum SplineWalkerMode
        {
            Once,
            Loop,
            PingPong
        }

        [Header("UI")]
        [SerializeField] private Text m_Messages;


        [Header("Spline Settings")]
        [SerializeField] private BezierSpline m_Spline;
        [SerializeField] private bool m_GeneratePoints = true;
        [SerializeField] private GameObject m_ReferenceObjectPrefab;

        [Header("Movement Settings")]
        [SerializeField] private SplineWalkerMode m_Mode;
        [SerializeField] private float m_Duration = 5.0f;
        [SerializeField] private bool m_LookForward;

        private float m_Progress;
        private bool m_Move = false;
        private bool m_GoingForward = true;

        public void Play()
        {
            m_Progress = 0;
            m_Move = true;
            m_GoingForward = true;

            m_Messages.text = "Traveller Progress: " + m_Progress;

            if (m_GeneratePoints)
            {
                int nPoints = 5;
                float progress = 0.0f;
                float section = m_Duration / nPoints;
                for (int i=0; i< nPoints; i++)
                {
                    Vector3 position = m_Spline.GetPoint(progress);

                    GameObject obj = Instantiate(m_ReferenceObjectPrefab);
                    obj.transform.parent = m_Spline.transform;
                    obj.transform.localPosition = position;

                    progress += section;
                }

            }
        }

        private void Update()
        {
            if (!m_Move) return;

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

            m_Messages.text = "Traveller Progress: " + m_Progress;

            Vector3 position = m_Spline.GetPoint(m_Progress);
            transform.localPosition = position;

            if (m_LookForward)
            {
                transform.LookAt(position + m_Spline.GetDirection(m_Progress));
            }            
        }
    }
}

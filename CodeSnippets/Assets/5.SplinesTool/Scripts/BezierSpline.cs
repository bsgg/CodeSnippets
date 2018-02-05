using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityCurves
{
    public class BezierSpline : MonoBehaviour
    {
        [Header("Points")]
        [SerializeField]
        private Vector3[] m_Points;

        public Vector3[] Points
        {
            get
            {
                return m_Points;
            }
        }

        [SerializeField]
        private BezierControlPointMode[] m_Modes;

        [SerializeField]
        private bool m_Loop;

        [SerializeField] private List<GameObject> m_ReferencePoints = new List<GameObject>();

        public bool Loop
        {
            get
            {
                return m_Loop;
            }
            set
            {
                m_Loop = value;
                if (value == true)
                {
                    m_Modes[m_Modes.Length - 1] = m_Modes[0];
                    SetControlPoint(0, m_Points[0]);
                }
            }
        }


        public int ControlPointCount
        {
            get
            {
                return m_Points.Length;
            }
        }

        public void Reset()
        {
            m_Points = new Vector3[] 
            {
                new Vector3(1.0f, 0.0f, 0.0f),
                new Vector3(2.0f, 0.0f, 0.0f),
                new Vector3(3.0f, 0.0f, 0.0f),
                new Vector3(4.0f, 0.0f, 0.0f)
            };


            m_Modes = new BezierControlPointMode[] {
                BezierControlPointMode.Free,
                BezierControlPointMode.Free
            };
        }

        public Vector3 GetControlPoint(int index)
        {
            return m_Points[index];
        }

        public void SetControlPoint(int index, Vector3 point)
        {
            
            if (index % 3 == 0)
            {
                Vector3 delta = point - m_Points[index];
                if (m_Loop)
                {
                    if (index == 0)
                    {
                        m_Points[1] += delta;
                        m_Points[m_Points.Length - 2] += delta;
                        m_Points[m_Points.Length - 1] = point;
                    }
                    else if (index == m_Points.Length - 1)
                    {
                        m_Points[0] = point;
                        m_Points[1] += delta;
                        m_Points[index - 1] += delta;
                    }
                    else
                    {
                        m_Points[index - 1] += delta;
                        m_Points[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        m_Points[index - 1] += delta;
                    }
                    if (index + 1 < m_Points.Length)
                    {
                        m_Points[index + 1] += delta;
                    }
                }
            }

            m_Points[index] = point;
            EnforceMode(index);
        }

        public int CurveCount
        {
            get
            {
                return (m_Points.Length - 1) / 3;
            }
        }


        public BezierControlPointMode GetControlPointMode(int index)
        {
            return m_Modes[(index + 1) / 3];
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            m_Modes[modeIndex] = mode;
            if (m_Loop)
            {
                if (modeIndex == 0)
                {
                    m_Modes[m_Modes.Length - 1] = mode;
                }
                else if (modeIndex == m_Modes.Length - 1)
                {
                    m_Modes[0] = mode;
                }
            }
            EnforceMode(index);
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = m_Modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !m_Loop && (modeIndex == 0 || modeIndex == m_Modes.Length - 1))
            {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = m_Points.Length - 2;
                }
                enforcedIndex = middleIndex + 1;

                if (enforcedIndex >= m_Points.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= m_Points.Length)
                {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = m_Points.Length - 2;
                }
            }

            Vector3 middle = m_Points[middleIndex];
            Vector3 enforcedTangent = middle - m_Points[fixedIndex];


            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, m_Points[enforcedIndex]);
            }

            m_Points[enforcedIndex] = middle + enforcedTangent;

        }

        public Vector3 GetPoint(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = m_Points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int) t;
                t -= i;
                i *= 3;
            }

            return transform.TransformPoint(Bezier.GetPoint(m_Points[i], m_Points[i + 1], m_Points[i + 2], m_Points[i + 3], t));

        }

        /// <summary>
        /// Gets velocity according to a t param. It produces a velocity vector and not a point, 
        /// it should not be affected by the position of the curve, so we subtract that after transforming.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = m_Points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int) t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetFirstDerivative(m_Points[i], m_Points[i+1], m_Points[i+2], m_Points[i+3], t)) -
                transform.position;
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }


        public void AddCurve()
        {
            Vector3 point = m_Points[m_Points.Length - 1];
            Array.Resize(ref m_Points, m_Points.Length + 3);
            point.x += 1f;
            m_Points[m_Points.Length - 3] = point;
            point.x += 1f;
            m_Points[m_Points.Length - 2] = point;
            point.x += 1f;
            m_Points[m_Points.Length - 1] = point;


            Array.Resize(ref m_Modes, m_Modes.Length + 1);
            m_Modes[m_Modes.Length - 1] = m_Modes[m_Modes.Length - 2];
            EnforceMode(m_Points.Length - 4);

            if (m_Loop)
            {
                m_Points[m_Points.Length - 1] = m_Points[0];
                m_Modes[m_Modes.Length - 1] = m_Modes[0];
                EnforceMode(0);
            }
        }


        public void RemoveReferecePoints()
        {
            if (m_ReferencePoints.Count > 0)
            {
                for (int i = (m_ReferencePoints.Count - 1); i >= 0; i--)
                {
                    if (Application.isEditor)
                    {
                        DestroyImmediate(m_ReferencePoints[i]);
                    }
                    else
                    {
                        Destroy(m_ReferencePoints[i]);
                    }
                }
            }

            m_ReferencePoints = new List<GameObject>();
        }
        public void AddReferencePoints()
        {
            RemoveReferecePoints();

            for (int i = 0; i < m_Points.Length; i++)
            {
                if ((i % 3) == 0) // Only multiple of 3 are points, the rest are control points
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.name = "ReferencePoint_" + i;
                    go.transform.parent = transform;
                    go.transform.localPosition = m_Points[i];
                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    m_ReferencePoints.Add(go);
                }
            }
        }
    }

}

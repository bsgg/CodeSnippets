using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityCurves
{
    public class BezierCurve : MonoBehaviour
    {
        [Header("Points")]
        [SerializeField]
        private Vector3[] m_Points;
        public Vector3[] Points
        {
            get { return m_Points; }
            set { m_Points = value; }
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
        }

        public Vector3 GetPoint(float t)
        {
            return transform.TransformPoint(Bezier.GetPoint(m_Points[0], m_Points[1], m_Points[2], m_Points[3], t));

        }

        /// <summary>
        /// Gets velocity according to a t param. It produces a velocity vector and not a point, 
        /// it should not be affected by the position of the curve, so we subtract that after transforming.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetVelocity(float t)
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(m_Points[0], m_Points[1], m_Points[2], m_Points[3], t)) -
                transform.position;
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IKController
{
    public class Segment : MonoBehaviour
    {
        private LineRenderer m_line;

        [SerializeField] private Transform m_startPoint;
        public Transform StartPoint
        {
            get { return m_startPoint; }
            set { m_startPoint = value; }
        }

        [SerializeField] private Transform m_endPoint;

        private float m_angle;

        private float m_lenght;

        private Segment parent = null;
        public Segment Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private Segment child = null;
        public Segment Child
        {
            get { return child; }
            set { child = value; }
        }

        private void Awake()
        {
            m_line = GetComponent<LineRenderer>();
        }

        public void InitializeSegment(Vector2 startPoint, float a_len)
        {
            m_startPoint.localPosition = new Vector3(startPoint.x, startPoint.y, 0.0f);

            m_lenght = a_len;

            CalculateEndPoint();       
        }

        public void InitializeSegment(Segment a_parent, float a_len)
        {
            if (a_parent == null) return;

            parent = a_parent;

            // Set the start point of this segmet, on the end point of the parent
            m_startPoint.localPosition = new Vector3(a_parent.m_endPoint.localPosition.x, a_parent.m_endPoint.localPosition.y, 0.0f);

            m_lenght = a_len;
            CalculateEndPoint();
        }

         private void CalculateEndPoint()
         {
            float dx = m_lenght * Mathf.Cos(m_angle);
            float dy = m_lenght * Mathf.Sin(m_angle);

            m_endPoint.localPosition = new Vector3(m_startPoint.position.x + dx, m_startPoint.localPosition.y + dy, 0.0f);
        }

        public void Follow()
        {
            float targetX = child.m_startPoint.localPosition.x;
            float targetY = child.m_startPoint.localPosition.y;

            Follow(targetX, targetY);
        }

        public void Follow(float targetX, float targetY)
        {
            Vector3 target = new Vector3(targetX, targetY, 0.0f);
            m_angle = Mathf.Atan2(target.y - m_startPoint.localPosition.y, target.x - m_startPoint.localPosition.x);

            // Get direction between target and a, and make it 1
            Vector3 dir = target - m_startPoint.localPosition;
            dir.Normalize();

            // Get the lenth of direction equal to a b vector
            // Or move the vector dir where b is located at the moment
            dir = dir * m_lenght;

            // Change dir's direction to the opposite direction,
            dir *= -1;

            // Make a start from the end of dir (or B in this case)
            m_startPoint.localPosition = target + dir;
        }

        public void UpdateSegment()
        {
            CalculateEndPoint();

            m_line.SetPosition(0, m_startPoint.position);
            m_line.SetPosition(1, m_endPoint.position);
        }
    }
}
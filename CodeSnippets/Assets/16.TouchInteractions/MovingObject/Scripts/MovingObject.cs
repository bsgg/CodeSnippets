using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets.MovingObject
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField] private float m_speed = 1.0f;

        private CharacterController m_controller;

        private Vector3 m_initialPosition;
        private Vector3 m_targetPoint;

        private Vector3 m_startPoint;
        private Vector3 m_currentDirection;
        private float m_totalDistance = 0;
        private bool m_isMoving;

        void Start()
        {
            m_controller = GetComponent<CharacterController>();
            m_initialPosition = transform.localPosition;
            m_isMoving = false;
        }

        public void SetTargetPosition(Vector3 a_targetPoint)
        {
            m_targetPoint = a_targetPoint;
            m_startPoint = transform.position;

            // Set direction
            m_currentDirection = m_targetPoint - transform.position;
            m_totalDistance = m_currentDirection.sqrMagnitude;
            m_currentDirection.Normalize();
            m_currentDirection.y = 0.0f;
            m_isMoving = true;
        }

        public void Move()
        {
            if (!m_isMoving) return;

            float distanceTravelled = (m_startPoint - transform.position).sqrMagnitude;
            if (distanceTravelled < m_totalDistance)
            {
                m_controller.Move(m_currentDirection * m_speed * Time.deltaTime);
            }else
            {
                transform.position = new Vector3(m_targetPoint.x, m_initialPosition.y, m_targetPoint.z);
                m_isMoving = false;
            }
        }		
	}
}

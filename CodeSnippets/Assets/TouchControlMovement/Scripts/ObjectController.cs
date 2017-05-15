using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets
{
    public class ObjectController : MonoBehaviour
    {
        [SerializeField]
        private float m_SpeedMovement = 1.0f;

        private CharacterController m_Controller;
        private Vector3 m_InitPosition;
        private Vector3 m_TargetPoint;
        private Vector3 m_OrigPoint;
        private Vector3 m_CurrentDirection;
        private float m_TotalDistance = 0;
        private bool m_Move;

        void Start()
        {
            m_Controller = GetComponent<CharacterController>();
            m_InitPosition = transform.localPosition;
            m_Move = false;
        }

        public void Move(Vector3 targetPoint)
        {
            m_TargetPoint = targetPoint;
            m_OrigPoint = transform.localPosition;

            // Get Direction
            Vector3 dir = m_TargetPoint - transform.localPosition;           

            m_TotalDistance = dir.magnitude;
            dir.Normalize();
            dir.y = 0.0f;

            m_CurrentDirection = dir;
            m_Move = true;
        }

        void Update()
        {
            if (m_Move)
            {
                Vector3 auxV = m_OrigPoint -  transform.localPosition;
                float currentDist = auxV.magnitude;
                if (currentDist < m_TotalDistance)
                {
                    m_Controller.Move(m_CurrentDirection * m_SpeedMovement * Time.deltaTime);
                }else
                {
                    transform.localPosition = new Vector3(m_TargetPoint.x, m_InitPosition.y, m_TargetPoint.z);
                    m_Move = false;
                }
            }
        }
		
	}
}

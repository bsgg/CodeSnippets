using UnityEngine;
using System.Collections;

namespace CodeSnippets
{
    public class Simulation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float m_Angle = 30;
        [SerializeField]
        private float m_InitVelocity = 50;

        [SerializeField]
        private Vector2 m_CurrentVelocity;

        private Vector3 m_InitPosition;
        private float m_Time;

        void Start()
        {
            m_InitPosition = transform.localPosition;
        }

        void Update()
        {
            // Check if object reachs the end position
            if (transform.localPosition.y >= m_InitPosition.y)
            {
                m_Time += Time.deltaTime;

                Vector2 position = MiscPhysics.GetPosition(m_InitVelocity,m_Angle, m_Time);
                transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);

                m_CurrentVelocity = MiscPhysics.GetVelocity(m_InitVelocity, m_Angle, m_Time);
            }            
        }
    }
}

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
        private Vector2 m_CurrentVelocity = Vector3.zero;
        public Vector2 CurrentVelocity
        {
            get { return m_CurrentVelocity; }
        }

        private Vector3 m_InitPosition = Vector3.zero;
        private float m_Time;


        [SerializeField] private MiscPhysics.ETYPEPHYSICS TypePhysics = MiscPhysics.ETYPEPHYSICS.PARABOLIC;

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

                // Parabolic movement
                m_CurrentVelocity = MiscPhysics.GetVelocity(TypePhysics, m_InitVelocity, m_Angle, m_Time);

                Vector2 position = MiscPhysics.GetPosition(TypePhysics, m_InitVelocity, m_Angle, m_Time);
                transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);


                // Free fall
                m_CurrentVelocity = MiscPhysics.GetVelocity(TypePhysics, m_InitVelocity, m_Angle, m_Time);
            }            
        }
    }
}

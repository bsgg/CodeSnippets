using UnityEngine;
using System.Collections;
using System;

namespace Effects
{
    public class DebrisEffect : Effect
    {
        [SerializeField] private float          m_ExplosionForce;
        [SerializeField] private float          m_ExplosionRadius;        
        [SerializeField] private Vector2       m_RangeMass;

        private Rigidbody[]                     m_ObjectRigidbodies;

        protected override void DoStart()
        {
            // Get all rigidbodies under this object
            m_ObjectRigidbodies = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < m_ObjectRigidbodies.Length; i++)
            {
                m_ObjectRigidbodies[i].isKinematic = true;
                m_ObjectRigidbodies[i].mass = 0.0f;
                m_ObjectRigidbodies[i].useGravity = true;
            }

            base.DoStart();
        }
       
        /// <summary>
        /// Show the effect to explode
        /// </summary>
        public void Play()
        {
            for (int i = 0; i < m_ObjectRigidbodies.Length; i++)
            {
                m_ObjectRigidbodies[i].isKinematic = false;
                m_ObjectRigidbodies[i].mass = UnityEngine.Random.Range(m_RangeMass.x, m_RangeMass.y);
                m_ObjectRigidbodies[i].AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);                
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class DissolveEffect : Effect
    {
        public enum DIRECTION { LEFT_RIGHT, RIGHT_LEFT };
        [SerializeField] private DIRECTION m_Direction = DIRECTION.LEFT_RIGHT;
        [SerializeField] private float m_Speed = 1.0f;

        private Material m_MeshMaterial;
        private float m_DissolveAmount = 0.0f; 
        

        protected override void DoAwake()
        {
            base.DoAwake();

            m_MeshMaterial = GetComponent<MeshRenderer>().materials[0];
        }

        protected override void DoStart()
        {
            if (m_MeshMaterial == null) return;

            base.DoStart();           

            if (m_Direction == DIRECTION.LEFT_RIGHT)
            {
                m_DissolveAmount = 0.0f;
            }else if (m_Direction == DIRECTION.RIGHT_LEFT)
            {
                m_DissolveAmount = 1.0f;
            }
                

            m_MeshMaterial.SetFloat("_SliceAmount", m_DissolveAmount);
            
        }

        protected override void DoUpdate()
        {
            if (m_MeshMaterial == null) return;

            base.DoUpdate();
            
            if (m_Direction == DIRECTION.LEFT_RIGHT)
            {
                if (m_DissolveAmount < 1.0f)
                {
                    m_DissolveAmount += (m_Speed * Time.deltaTime);
                }
                else
                {
                    m_DissolveAmount = 1.0f;
                    m_Direction = DIRECTION.RIGHT_LEFT;
                }

            }else if (m_Direction == DIRECTION.RIGHT_LEFT)
            {
                if (m_DissolveAmount > 0.0f)
                {
                    m_DissolveAmount -= (m_Speed * Time.deltaTime);
                }
                else
                {
                    m_DissolveAmount = 0.0f;
                    m_Direction = DIRECTION.LEFT_RIGHT;
                }
            }

            m_MeshMaterial.SetFloat("_SliceAmount", m_DissolveAmount);           

        }
    }
}

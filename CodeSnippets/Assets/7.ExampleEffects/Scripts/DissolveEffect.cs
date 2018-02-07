using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisCode
{
    public class DissolveEffect : Effect
    {

        [SerializeField] private MeshRenderer m_MeshRenderer;
        private                  Material m_MeshMaterial;

        private float m_SliceAmount = 0.0f;
        [SerializeField] private float m_SpeedSliceAmount = 1.0f;

        [SerializeField]
        private bool m_LeftToRight = true;

        protected override void DoStart()
        {
            base.DoStart();

            if (m_MeshRenderer != null)
            {
                m_MeshMaterial = m_MeshRenderer.materials[0];

                if (m_LeftToRight)
                {
                    m_SliceAmount = 0.0f;
                }else
                {
                    m_SliceAmount = 1.0f;
                }
                

                m_MeshMaterial.SetFloat("_SliceAmount", m_SliceAmount);
            }
        }

        protected override void DoUpdate()
        {
            base.DoUpdate();

            if (m_MeshMaterial != null)
            {
                if (m_LeftToRight)
                {
                    if (m_SliceAmount < 1.0f)
                    {
                        m_SliceAmount += (m_SpeedSliceAmount * Time.deltaTime);
                    }
                    else
                    {
                        m_SliceAmount = 1.0f;
                    }
                }else
                {
                    if (m_SliceAmount > 0.0f)
                    {
                        m_SliceAmount -= (m_SpeedSliceAmount * Time.deltaTime);
                    }
                    else
                    {
                        m_SliceAmount = 0.0f;
                    }
                }

                m_MeshMaterial.SetFloat("_SliceAmount", m_SliceAmount);
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{ 
    public class BaseInteraction : MonoBehaviour
    {
        public delegate void OnObjectAction(GameObject objectAction);
        public OnObjectAction OnTouchObjectEvent;
        public OnObjectAction OnEndTouchObjectEvent;

        public enum EPHASEINTERACTION { NONE, START, TOUCH, END };
        [SerializeField]
        protected EPHASEINTERACTION m_PhaseInteraction = EPHASEINTERACTION.NONE;


        [Header("Camera for interaction")]
        [SerializeField] protected Camera m_InteractionCamera;

        [Header("Tag name for the object")]
        [SerializeField] private string m_ObjectTag;

        protected Vector3 m_CurrentTouch;
        protected Vector3 m_LastTouch;           

        private GameObject m_CurrentObjectTouched;


        private void Start()
        {
            DoStart();
        }

        protected virtual void DoStart()
        {
            m_CurrentTouch = Vector3.zero;
            m_LastTouch = Vector3.zero;

            m_PhaseInteraction = EPHASEINTERACTION.NONE;
        }

        private void Update()
        {
            DoUpdate();
        }

        protected virtual void DoUpdate()
        {
            // Check phases
            if (m_PhaseInteraction == EPHASEINTERACTION.START)
            {
                m_PhaseInteraction = EPHASEINTERACTION.TOUCH;
            }
            else if (m_PhaseInteraction == EPHASEINTERACTION.END)
            {
                m_PhaseInteraction = EPHASEINTERACTION.NONE;

            }

            m_CurrentTouch = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                m_CurrentObjectTouched = GetHitObject(m_CurrentTouch);
                if (m_CurrentObjectTouched != null)
                {
                    Debug.LogFormat("[BaseInteraction] OnHitObject Name: {0} - Tag: {1} ", m_CurrentObjectTouched.name, m_CurrentObjectTouched.tag);
                    if (m_PhaseInteraction == EPHASEINTERACTION.NONE)
                    {
                        m_PhaseInteraction = EPHASEINTERACTION.START;
                        // Event when touch on the object
                        if (OnTouchObjectEvent != null)
                        {
                            OnTouchObjectEvent(m_CurrentObjectTouched);
                        }

                    }
                }
                else
                {
                    m_PhaseInteraction = EPHASEINTERACTION.END;
                }

                m_LastTouch = m_CurrentTouch;
            }

            if (Input.GetMouseButtonUp(0))
            {

                m_PhaseInteraction = EPHASEINTERACTION.END;
               
                if (OnEndTouchObjectEvent != null)
                {
                    OnEndTouchObjectEvent(m_CurrentObjectTouched);
                }                
            }
        }

        protected GameObject GetHitObject(Vector3 pos)
        {
            if (m_InteractionCamera != null)
            {
                Ray auxRay = m_InteractionCamera.ScreenPointToRay(pos);
                RaycastHit auxRayCast;

                if (Physics.Raycast(auxRay, out auxRayCast))
                {
                    if (auxRayCast.collider.gameObject.tag == m_ObjectTag)
                    {
                        return auxRayCast.collider.gameObject;
                    }
                }
            }
            return null;
        }

    }
}

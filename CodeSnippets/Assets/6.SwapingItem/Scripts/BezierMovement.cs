using UnityEngine;
using System.Collections;

namespace SwapItems
{
    public class BezierMovement : MonoBehaviour
    {
        public delegate void DelegateFinishMove(BezierMovement obj);
        public DelegateFinishMove OnFinishMove;

        // Start, end and control point       
        private Vector3                     m_StartPosition;
        private Vector3                     m_EndPosition;
        private Vector3                     m_ControlPosition;
        // Control movement
        private bool                        m_Animate = false;
        private float                       m_TranslationTime = 0.6f;
        private float                       m_Timer = 0.0f;

        /// <summary>
        /// Start a swap movement
        /// </summary>
        /// <param name="endPosition">End position</param>
        /// <param name="speed">Movement Speed</param>
        public void StartBezierAnimation(Vector3 endPosition, float speed = 0.6f)
        {
            m_Timer = 0.0f;
            m_TranslationTime = speed;
            m_StartPosition = this.transform.localPosition;
            m_EndPosition = endPosition;
            if (endPosition.x > this.transform.localPosition.x)
            {
                // Get control point           
                float distance = endPosition.x - this.transform.localPosition.x;
                float xOffset = distance / 2.0f;
                m_ControlPosition = new Vector3(m_StartPosition.x + xOffset, m_StartPosition.y, m_StartPosition.z + (distance / 2.0f));
            }
            else
            {
                // Get control point           
                float distance = this.transform.localPosition.x - endPosition.x;
                float xOffset = distance / 2.0f;
                m_ControlPosition = new Vector3(m_StartPosition.x - xOffset, m_StartPosition.y, m_StartPosition.z - (distance / 2.0f));
            }

            m_Animate = true;
        }

        /// <summary>
        /// Animate the movement
        /// </summary>
        private void AnimateBezier()
        {
            if (m_Animate)
            {
                if (m_Timer < m_TranslationTime)
                {
                    transform.localPosition = Utility.MathUtility.Bezier(m_StartPosition, m_ControlPosition, m_EndPosition, m_Timer / m_TranslationTime);
                    m_Timer += Time.deltaTime;
                }

                else if (m_Timer >= m_TranslationTime)
                {
                    this.transform.localPosition = m_EndPosition;
                    m_Animate = false;
                    // Finish move
                    if (OnFinishMove != null)
                    {
                        OnFinishMove(this);
                    }
                }
            }
        }

        void Update()
        {
            AnimateBezier();            
        }
    }

}

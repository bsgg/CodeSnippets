using UnityEngine;
using System.Collections;
using System;

namespace ThimblerigSwapping
{
    public class BezierMovement : MonoBehaviour
    {
        public Action<BezierMovement> BezierActionCompleted;

        public void MoveTo(Vector3 endPosition, float totalTime = 0.6f)
        {
            StartCoroutine(Animate(endPosition, totalTime));
        }

        private IEnumerator Animate(Vector3 endPosition, float totalTime = 0.6f)
        {
            Vector3 startPosition = transform.localPosition;
            Vector3 controlPosition;
            if (endPosition.x > transform.localPosition.x)
            {
                // Get control point           
                float distance = endPosition.x - transform.localPosition.x;
                float xOffset = distance / 2.0f;
                controlPosition = new Vector3(startPosition.x + xOffset, startPosition.y, startPosition.z + (distance / 2.0f));
            }
            else
            {
                // Get control point           
                float distance = transform.localPosition.x - endPosition.x;
                float xOffset = distance / 2.0f;
                controlPosition = new Vector3(startPosition.x - xOffset, startPosition.y, startPosition.z - (distance / 2.0f));
            }


            float time = 0.0f;
            while (time < totalTime)
            {
                transform.localPosition = Bezier(startPosition, controlPosition, endPosition, time / totalTime);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // Finish animation
            transform.localPosition = endPosition;

            // Finish move
            if (BezierActionCompleted != null)
            {
                BezierActionCompleted(this);
            }
        }

        /// <summary>
        /// Bezier formule between 2 points
        /// </summary>
        /// <returns>The bezier.</returns>
        /// <param name="a_start">Start Point</param>
        /// <param name="a_control">Control bezier</param>
        /// <param name="a_end">End point</param>
        /// <param name="a_time">Time to generate the bezier curve</param>
        public Vector3 Bezier(Vector3 a_start, Vector3 a_control, Vector3 a_end, float a_time)
        {
            return (((1 - a_time) * (1 - a_time)) * a_start) + (2 * a_time * (1 - a_time) * a_control) + ((a_time * a_time) * a_end);
        }
    }

}

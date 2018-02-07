using UnityEngine;
using System.Collections;

namespace ThimblerigSwapping
{
    public class BezierMovement : MonoBehaviour
    {
        public delegate void BezierAction(BezierMovement obj);
        public BezierAction OnEndMovement;

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
                transform.localPosition = Utility.MathUtility.Bezier(startPosition, controlPosition, endPosition, time / totalTime);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // Finish animation
            transform.localPosition = endPosition;

            // Finish move
            if (OnEndMovement != null)
            {
                OnEndMovement(this);
            }
        }
       
    }

}

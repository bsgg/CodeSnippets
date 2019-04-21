using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IKController
{
    /// <summary>
    /// Reference video: https://www.youtube.com/watch?v=hbgDqyy8bIw
    /// </summary>
    public class IKController : MonoBehaviour
    {
        [SerializeField] private Segment segmentPrefab;

        [SerializeField] private Segment lastSegment;

        [SerializeField] private float m_segmentLength = 2.0f;
        [SerializeField] private Vector2 m_startPoint = new Vector2();

        void Start()
        {

            Segment current = Instantiate(segmentPrefab, transform);
            current.name = "segment_0";
            current.InitializeSegment(m_startPoint, m_segmentLength);

            for (int i = 0; i < 10; i++)
            {
                Segment next = Instantiate(segmentPrefab, transform);
                next.name = "segment_" + (i + 1);
                next.InitializeSegment(current, m_segmentLength);

                current.Child = next;
                current = next;
            }

            // save last segment
            lastSegment = current;
        }

        void Update()
        {
            Vector3 targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lastSegment.Follow(targetPoint.x, targetPoint.y);
            lastSegment.UpdateSegment();

            Segment next = lastSegment.Parent;

            while (next != null)
            {
                next.Follow();

                next.UpdateSegment();

                next = next.Parent;
            }

        }
    }
}

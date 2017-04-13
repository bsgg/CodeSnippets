using Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisCode
{
    public class CircularPolarCoordinates : MonoBehaviour
    {
        [Header("Polar Coordinates Settings")]
        [SerializeField] private GameObject     m_InstancePrefab;
        [SerializeField] private float          m_Radio = 5.0f;
        [SerializeField] private int            m_NumberObjects = 6;
        [SerializeField] private Vector2        m_Center;
        

        public void GeneratePolarCoords()
        {
            Reset();
            float angle = 360.0f / m_NumberObjects;
            float auxAngle = 0.0f;
            for (int i = 0; i < m_NumberObjects; i++, auxAngle += angle)
            {
                Vector2 coords = MathUtility.GetPolarCoordinates(auxAngle, m_Radio, m_Center);
                Vector3 pos = new Vector3(coords.x, coords.y, 10.0f);

                // Instantiate the object
                GameObject obj = Instantiate(m_InstancePrefab, pos, Quaternion.identity) as GameObject;
                obj.name = "object" + i.ToString();
                obj.transform.parent = transform;
            }
        }

        public void Reset()
        {
            for (int i = (transform.childCount-1) ; i>= 0 ; i--)
            {
                if ((Application.isEditor) && (!Application.isPlaying))
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }else
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
    }
}

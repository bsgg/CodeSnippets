using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets
{
    public class MovementControl : MonoBehaviour
    {
        [SerializeField]
        private ObjectController m_ObjectToMove;

        [SerializeField]
        private string m_TagFloor;

        [SerializeField]
        private Camera m_RaycastCamera;       

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_RaycastCamera.ScreenPointToRay (Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 2000.0f))
                {
                    if (hitInfo.transform.name == m_TagFloor)
                    {
                        Vector3 target = new Vector3(hitInfo.point.x, 0.0f, hitInfo.point.z);
                        m_ObjectToMove.Move(target);

                    }
                }

            }

        }
    }
}

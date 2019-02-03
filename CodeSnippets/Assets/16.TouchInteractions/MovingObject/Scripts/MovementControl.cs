using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets.MovingObject
{
    public class MovementControl : MonoBehaviour
    {
        [SerializeField] private MovingObject m_movingObject;

        [SerializeField] private string m_tagFloor = "Ground";

        [SerializeField]  private Camera m_raycastCamera;       

        void Update()
        {
            m_movingObject.Move();

            CheckInput();
        }

        private void CheckInput()
        {
            if ((m_raycastCamera == null) || (m_movingObject == null)) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_raycastCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 2000.0f))
                {
                    if (hitInfo.transform.name == m_tagFloor)
                    {
                        Vector3 target = new Vector3(hitInfo.point.x, 0.0f, hitInfo.point.z);
                        m_movingObject.SetTargetPosition(target);
                    }
                }
            }
        }
    }
}

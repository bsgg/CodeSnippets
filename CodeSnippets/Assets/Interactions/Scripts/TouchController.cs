using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeSnippets.Interactions
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField]
        private float m_RotationSpeed = 1.0f;

        private Quaternion m_OriginalRotation;

        private void Start()
        {
            m_OriginalRotation = transform.localRotation; 
        }


        void Update()
        {
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)

        //Touch
        int touchCount = Input.touchCount;
        
        if (touchCount == 1)
        {
            
            Touch t = Input.GetTouch(0);
            if(EventSystem.current.IsPointerOverGameObject(t.fingerId))return;
            
            switch (t.phase)
            {
            case TouchPhase.Moved:
                
                float xAngle = t.deltaPosition.y * m_RotationSpeed;
                float yAngle = -t.deltaPosition.x * m_RotationSpeed;
                float zAngle = 0;
                
                transform.Rotate(xAngle, yAngle, zAngle, Space.World);
                
                break;
            }
            
        }
#else
            //Mouse
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                float xAngle = Input.GetAxis("Mouse Y") * m_RotationSpeed * 80;
                float yAngle = -Input.GetAxis("Mouse X") * m_RotationSpeed * 80;
                float zAngle = 0;

                transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
            }
#endif
        }


        public void ResetObject()
        {
            transform.localRotation = m_OriginalRotation;
        }


    }
}

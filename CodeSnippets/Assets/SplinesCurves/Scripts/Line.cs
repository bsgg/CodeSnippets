using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityCurves
{ 
    public class Line : MonoBehaviour
    {
        [Header("Points")]
        [SerializeField]
        private Vector3 m_P0;
        public Vector3 P0
        {
            get { return m_P0; }
            set { m_P0 = value; }
        }

        [SerializeField]
        private Vector3 m_P1;
        public Vector3 P1
        {
            get { return m_P1; }
            set { m_P1 = value; }
        }
    }
}

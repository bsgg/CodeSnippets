using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.PaintTool
{
    public class PaintToolUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_BrushMenu;
        private bool m_BrushMenuVisible = false;

        [SerializeField] private Image[] m_ColorImageList;

        [SerializeField] private Color m_BrushColor;
        public Color BrushColor
        {
            get { return m_BrushColor; }
        }

        [SerializeField] private float[] m_BrushSizeList;

        private float m_BrushSize;
        public float BrushSize
        {
            get { return m_BrushSize; }
        }

        private void Start()
        {           
            m_BrushMenuVisible = false;
            m_BrushMenu.gameObject.SetActive(m_BrushMenuVisible);

            // First color
            m_BrushColor = new Color(m_ColorImageList[0].color.r, m_ColorImageList[0].color.g, m_ColorImageList[0].color.b,1.0f);
            m_BrushSize = m_BrushSizeList[0];
        }

        public void OnMenuPress()
        {
            Debug.Log("OnMenuPress");

            if (m_BrushMenuVisible)
            {
                m_BrushMenuVisible = false;
            }
            else
            {
                m_BrushMenuVisible = true;
            }

            m_BrushMenu.gameObject.SetActive(m_BrushMenuVisible);
        }

        public void OnColorPress(int id)
        {
            if (id < m_ColorImageList.Length)
            {
                m_BrushColor = new Color(m_ColorImageList[id].color.r, m_ColorImageList[id].color.g, m_ColorImageList[id].color.b, 1.0f);
            }
        }

        public void OnBrushSizePress(int id)
        {
            if (id < m_BrushSizeList.Length)
            {
                m_BrushSize = m_BrushSizeList[id];
            }
        }

        public void OnErasePress()
        {
            
        }

        public void OnSavePress()
        {

        }

    }
}

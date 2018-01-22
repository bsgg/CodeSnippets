using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolygoTool
{
    public class PolygonToolUI : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_PolygonCanvas;

        [SerializeField]
        private Text m_MessageText;
        public string Message
        {
            get { return m_MessageText.text; }
            set { m_MessageText.text = value; }
        }

        [SerializeField]
        private Button m_StartButton;

        public Button StartButton
        {
            get { return m_StartButton; }
        }

        [SerializeField]
        private Button m_DeleteButton;

        public Button DeleteButton
        {
            get { return m_DeleteButton; }
        }

        [SerializeField]
        private Button m_SaveButton;

        public Button SaveButton
        {
            get { return m_SaveButton; }
        }

        [SerializeField]
        private Button m_ShowPointsButton;

        public Button ShowPointsButton
        {
            get { return m_ShowPointsButton; }
        }

        [SerializeField]
        private Button m_HidePointsButton;

        public Button HidePointsButton
        {
            get { return m_HidePointsButton; }
        }

       

        private bool m_Visible = false;
        public bool IsVisible
        {
            get { return m_Visible; }
        }

        public void Show()
        {
            m_Visible = true;
            m_PolygonCanvas.alpha = 1.0f;
            m_PolygonCanvas.interactable = true;
            m_PolygonCanvas.blocksRaycasts = true;

            m_MessageText.text = "Polygon Tool";
        }

        public void Hide()
        {
            m_Visible = false;
            m_PolygonCanvas.alpha = 0.0f;
            m_PolygonCanvas.interactable = false;
            m_PolygonCanvas.blocksRaycasts = false;

            m_MessageText.text = "Polygon Tool";
        }

        public void Toggle()
        {
            if (m_Visible) Hide();
            else Show();
        }

    }
}

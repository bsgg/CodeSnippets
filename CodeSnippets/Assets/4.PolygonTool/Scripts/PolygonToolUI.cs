using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolygoTool
{
    public class PolygonToolUI : MonoBehaviour
    {

        [SerializeField]
        private Text m_MessageText;
        public string Message
        {
            get { return m_MessageText.text; }
            set { m_MessageText.text = value; }
        }

        [SerializeField]
        private Button m_NewButton;
        public Button NewButton
        {
            get { return m_NewButton; }
        }

        [SerializeField]
        private Button m_DeleteButton;

        public Button DeleteButton
        {
            get { return m_DeleteButton; }
        }        

    }
}

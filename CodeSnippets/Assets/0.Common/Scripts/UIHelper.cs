using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class UIHelper : UIBase
    {
        [Header("UI")]
        [SerializeField]
        private Text m_TitleText;
        [SerializeField]
        private Text m_DescriptionText;


        [Header("UI")]
        [SerializeField]
        private string m_Title;

        [SerializeField]
        private string m_Description;
        

        private void Start()
        {
            m_TitleText.text = m_Title;
            m_DescriptionText.text = m_Description;            
        }

        public void OnClose()
        {
            Hide();
        }
		
	}
}

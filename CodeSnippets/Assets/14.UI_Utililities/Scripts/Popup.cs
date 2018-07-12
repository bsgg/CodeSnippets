using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.UI
{
    public class Popup : UIBase
    {        
		[SerializeField]
        protected Text m_TitleText;
        public string TitleText
        {
            get { return m_TitleText.text; }
            set { m_TitleText.text = value; }
        }

        [SerializeField]
        protected Text m_MessageText;
        public string MessageText
        {
            get { return m_MessageText.text; }
            set { m_MessageText.text = value; }
        }

        public virtual void ShowPopup(string title, string message)
        {
            m_TitleText.text = title;
            m_MessageText.text = message;
            Show();
        }

    }
}

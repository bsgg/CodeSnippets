using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Utility.UI
{
    public class ProgressUI : UIBase
    {
        [SerializeField] private Image m_ProgressImage;
        [SerializeField] private Text m_ProgressText;

        public void SetProgress(string text,int value)
        {
            m_ProgressText.text = text;
            m_ProgressImage.fillAmount = value / 100.0f;
        }
	}
}

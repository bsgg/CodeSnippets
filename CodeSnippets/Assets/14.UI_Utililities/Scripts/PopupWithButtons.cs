using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

namespace Utility.UI
{
    public class PopupWithButtons : Popup
    {
        [SerializeField]
        private ButtonWithText m_MiddleBtn;
        public ButtonWithText MiddleBtn
        {
            get { return m_MiddleBtn; }
            set { m_MiddleBtn = value; }
        }

        [SerializeField]
        private ButtonWithText m_LeftBtn;
        public ButtonWithText LeftBtn
        {
            get { return m_LeftBtn; }
            set { m_LeftBtn = value; }
        }

        [SerializeField]
        private ButtonWithText m_RightBtn;
        public ButtonWithText RightBtn
        {
            get { return m_RightBtn; }
            set { m_RightBtn = value; }
        }


        public void ShowPopup(string title, string message,
           string middleBtn, ButtonWithText.ButtonWithTextAction  middleBtnCallback,
           string leftBtn, ButtonWithText.ButtonWithTextAction leftBtnCallback,
           string rightBtn, ButtonWithText.ButtonWithTextAction rightBtnCallback)
        {
            if (m_TitleText != null)
            {
                m_TitleText.text = title;
            }
            m_MessageText.text = message;

            if (middleBtnCallback != null)
            {
                //m_MiddleBtn.Set(middleBtn, middleBtnCallback);
                m_MiddleBtn.Set(0, middleBtn, middleBtnCallback);
                m_MiddleBtn.Show();
            }
            else
            {
                m_MiddleBtn.Hide();
            }

            if (leftBtnCallback != null)
            {
                m_LeftBtn.Set( 0, leftBtn, leftBtnCallback);
                m_LeftBtn.Show();
            }
            else
            {
                m_LeftBtn.Hide();
            }

            if (rightBtnCallback != null)
            {
                //m_RightBtn.Set(rightBtn, rightBtnCallback);
                m_RightBtn.Set(0, rightBtn, rightBtnCallback);
                m_RightBtn.Show();
            }
            else
            {
                m_RightBtn.Hide();
            }
            Show();
        }        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MessageQueue
{
    public class PanelTest : MonoBehaviour
    {
        public Action OnButtonPressed;
        public Action OnExitPressed;

        [SerializeField] private Text m_description;

        public void Populate(string a_desc)
        {
            m_description.text = a_desc;
        }

        public void OnBackButtonPressed()
        {
            if (OnButtonPressed != null)
            {
                OnButtonPressed();
            }
        }

        public void OnExitButtonPressed()
        {
            if (OnExitPressed != null)
            {
                OnExitPressed();
            }
        }
    }
}

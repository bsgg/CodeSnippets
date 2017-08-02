using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InkExample
{
    public class InkExample : MonoBehaviour
    {
        [SerializeField]
        private Text m_Description;
        public string Description
        {
            get { return m_Description.text; }
            set { m_Description.text = value; }
        }


        [SerializeField]
        private ButtonText[] m_ChoicesButtons;

        public void SetChoice(int id, string text)
        {
            if (id < m_ChoicesButtons.Length)
            {
                m_ChoicesButtons[id].Text = text;
            }
        }

        public void ActiveChoice(int id, bool active)
        {
            if (id < m_ChoicesButtons.Length)
            {
                m_ChoicesButtons[id].gameObject.SetActive(active);
            }
        }

        public void DisableAllChoices()
        {
            for (int i= 0; i< m_ChoicesButtons.Length; i++ )
            {
                m_ChoicesButtons[i].gameObject.SetActive(false);
            }
        }


    }
}

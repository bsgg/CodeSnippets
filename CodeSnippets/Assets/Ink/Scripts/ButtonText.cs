using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InkExample
{
    public class ButtonText : MonoBehaviour
    {
        [SerializeField]
        private Text m_Text;
        public string Text
        {
            get { return m_Text.text; }
            set { m_Text.text = value; }
        }

        [SerializeField]
        private Button m_Button;
        public Button Button
        {
            get { return m_Button; }
            set { m_Button = value; }
        }

    }
}

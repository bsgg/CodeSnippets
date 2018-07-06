using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
    public class ScrollTextUI : MonoBehaviour
    {
        [SerializeField] private RectTransform  m_ContentScroll;
        [SerializeField] private Text           m_Description;
        public string Description
        {
            get { return m_Description.text; }
            set { m_Description.text = value; }
        }

        public void SetText(string text)
        {
            m_Description.text = text;
            // Update content size
            m_ContentScroll.sizeDelta = new Vector2(m_ContentScroll.sizeDelta.x, m_Description.preferredHeight);
        }

    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class UIBase : Base
    {
        [SerializeField]  private CanvasGroup m_CanvasGroup;	

        public override void Show()
        {
            base.Show();
            if (m_CanvasGroup != null)
            {
                m_CanvasGroup.alpha = 1.0f;
                m_CanvasGroup.blocksRaycasts = true;
                m_CanvasGroup.interactable = true;
            }
        }

        public override void Hide()
        {
            base.Hide();
            if (m_CanvasGroup != null)
            {
                m_CanvasGroup.alpha = 0.0f;
                m_CanvasGroup.blocksRaycasts = false;
                m_CanvasGroup.interactable = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class TextButton : UIBase
    {

        public delegate void ClickAction(TextButton button);
        public event ClickAction OnButtonClicked;

        [SerializeField] private Button m_ButtonComponent;
        public Button ButtonComponent
        {
            get { return m_ButtonComponent; }
        }

        [SerializeField] private Text m_Title;
        public string Title
        {
            get { return m_Title.text; }
            set { m_Title.text = value; }
        }

        private int m_IdButton;
        public int IdButton
        {
            get { return m_IdButton; }
            set { m_IdButton = value; }
        }

        private int m_x;
        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        private int m_y;
        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        private EventTrigger m_EventTrigger;

        private void Awake()
        {
            m_EventTrigger = GetComponent<EventTrigger>();
            m_ButtonComponent = GetComponent<Button>();

        }

        public void Set(string title, int idButton, ClickAction action)
        {
            m_Title.text = title;
            m_IdButton = idButton;
            if (action != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { action(this); });

                if (m_EventTrigger != null)
                {
                    for (int i = m_EventTrigger.triggers.Count - 1; i >= 0; i--)
                    {
                        m_EventTrigger.triggers.RemoveAt(i);
                    }

                    m_EventTrigger.triggers.Add(entry);
                }
            }

            // Set pointer trigger
            /*EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnButtonClicked(this); });

            if (m_EventTrigger != null)
            {
                m_EventTrigger.triggers.Remove(entry);
                m_EventTrigger.triggers.Add(entry);
            }  */          
        }

        /*public void SetEventTrigger(ClickAction action)
        {
            // Set pointer trigger
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { action(this); });

            if (m_EventTrigger != null)
            {
                for (int i= m_EventTrigger.triggers.Count - 1 ; i>= 0; i--)
                {
                    m_EventTrigger.triggers.RemoveAt(i);
                }
                
                m_EventTrigger.triggers.Add(entry);
            }
        }*/
        
        /*public void OnClickPressed()
        {
            if (OnButtonClicked != null)
            {
                OnButtonClicked(this);
            }
        }*/      
    }
}

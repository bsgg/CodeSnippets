using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class PanelControllerB : MonoBehaviour
    {
        [SerializeField] private PanelTest m_panelB;

        private MessengerService m_messenger;

        private Data m_receivedData;

        private void Awake()
        {
            m_messenger = ServiceLocator.GetService<MessengerService>();
        }

        private void Start()
        {
            m_panelB.OnButtonPressed += ButtonPressed;

            m_messenger.Subscribe(Messages.HIDE_PANEL_B, HideHandleCallBack);
            m_messenger.Subscribe<Data>(Messages.SHOW_PANEL_B, ShowHandleCallback);
        }

        private void ShowHandleCallback(Data a_data)
        {
            m_receivedData = a_data;

            m_panelB.gameObject.SetActive(true);
            m_panelB.Populate(m_receivedData.Description);
        }


        private void HideHandleCallBack()
        {
            m_panelB.gameObject.SetActive(false);
        }

        private void ButtonPressed()
        {
            m_receivedData.Points += 1;
            m_receivedData.Description = "Panel A - Points " + m_receivedData.Points;

            m_messenger.Send<Data>(Messages.SHOW_PANEL_A, m_receivedData);

            m_messenger.Send(Messages.HIDE_PANEL_B);
        }
    }
}

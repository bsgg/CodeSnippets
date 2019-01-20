using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class PanelControllerA : MonoBehaviour
    {
        [SerializeField] private PanelTest m_panelA;

        private MessengerService m_messenger;

        private Data m_receivedData;

        private void Awake()
        {
            m_messenger = ServiceLocator.GetService<MessengerService>();

            m_panelA.OnButtonPressed += ButtonPressed;
            m_panelA.OnExitPressed += ExitPressed;
        }

        private void Start()
        {
            m_messenger.Subscribe<Data>(Messages.SHOW_PANEL_A, ShowHandleCallback);
            m_messenger.Subscribe(Messages.HIDE_PANEL_A, HideHandleCallback);
        }

        private void ShowHandleCallback(Data a_data)
        {
            m_receivedData = a_data;

            m_panelA.gameObject.SetActive(true);

            m_panelA.Populate(m_receivedData.Description);
        }

        private void HideHandleCallback()
        {
            m_panelA.gameObject.SetActive(false);
        }


        private void ButtonPressed()
        {
            m_messenger.Send(Messages.HIDE_PANEL_A);

            m_receivedData.Points += 1;
            m_receivedData.Description = "Panel B - Points " + m_receivedData.Points;

            m_messenger.Send<Data>(Messages.SHOW_PANEL_B, m_receivedData);
        }

        private void ExitPressed()
        {
            m_messenger.Unsubscribe<Data>(Messages.SHOW_PANEL_A, ShowHandleCallback);

            m_messenger.Send(Messages.END_GAME, m_receivedData.Points);

        }
    }
}

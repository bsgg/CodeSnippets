using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class Data
    {
        public string Description;
        public int Points;
    }

    public class StartPoint : MonoBehaviour
    {

        private MessengerService m_messenger;

        private void Awake()
        {
            m_messenger = ServiceLocator.GetService<MessengerService>();
        }

        private void Start()
        {
            m_messenger.Send(Messages.HIDE_PANEL_A);
            m_messenger.Send(Messages.HIDE_PANEL_B);
            m_messenger.Send(Messages.START_GAME);
        }
    }
}

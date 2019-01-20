using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MessageQueue
{
    public class StartGamePanel : MonoBehaviour
    {
        public Action OnStartButtonPressed;

        [SerializeField] private GameObject m_startButton;

        [SerializeField] private Text m_description;


        public void SetDescription(string a_text)
        {
            m_description.text = a_text;
        }

        public void EnableStartButton()
        {
            m_startButton.SetActive(true);
        }

        public void DisableStartButton()
        {
            m_startButton.SetActive(false);
        }

        public void OnStartButton()
        {
            if (OnStartButtonPressed != null)
            {
                OnStartButtonPressed();
            }
        }
    }

}

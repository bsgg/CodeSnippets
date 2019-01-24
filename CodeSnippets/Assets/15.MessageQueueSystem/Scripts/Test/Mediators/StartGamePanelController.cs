using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class StartGamePanelController : MonoBehaviour
    {
        [SerializeField] private StartGamePanel m_startGamePanel;

        private MessengerService m_messenger;

        private void Start()
        {
            m_messenger = ServiceLocator.GetService<MessengerService>();

            m_messenger.Subscribe(Messages.SHOW_PANEL_START_GAME, ShowHandleCallback);

            m_messenger.Subscribe(Messages.HIDE_PANEL_START_GAME, HideHandleCallback);

            m_messenger.Subscribe<int>(Messages.SHOW_START_GAME_PANEL_POINTS, ShowPointsHandleCallback);

            m_startGamePanel.OnStartButtonPressed += ButtonPressed;
        }       

        private void OnDisable()
        {
            m_messenger.Unsubscribe(Messages.SHOW_PANEL_START_GAME, ShowHandleCallback);

            m_messenger.Unsubscribe(Messages.HIDE_PANEL_START_GAME, HideHandleCallback);

            m_messenger.Unsubscribe<int>(Messages.SHOW_START_GAME_PANEL_POINTS, ShowPointsHandleCallback);

            m_startGamePanel.OnStartButtonPressed -= ButtonPressed;
        }

        private void ShowHandleCallback()
        {
            m_startGamePanel.EnableStartButton();
            m_startGamePanel.gameObject.SetActive(true);
        }

        private void HideHandleCallback()
        {
            m_startGamePanel.DisableStartButton();
            m_startGamePanel.gameObject.SetActive(false);
        }


        private void ShowPointsHandleCallback(int a_points)
        {
            m_startGamePanel.DisableStartButton();
            m_startGamePanel.SetDescription("Your final score is: " + a_points);

        }


        private void ButtonPressed()
        {
            m_startGamePanel.gameObject.SetActive(false);

            Data dataPanel = new Data
            {
                Description = "Panel A - Points: 1",
                Points = 1

            };

            m_messenger.Send<Data>(Messages.SHOW_PANEL_A, dataPanel);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class EndGameCommand : AbstractCommand<int>
    {
        protected override void OnStart(DependencyContainer a_dependancyContainer)
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnExecute(int a_data)
        {
            m_messageService.Send(Messages.HIDE_PANEL_A);

            m_messageService.Send(Messages.SHOW_PANEL_START_GAME);

            m_messageService.Send<int>(Messages.SHOW_START_GAME_PANEL_POINTS, a_data);
        }
    }
}

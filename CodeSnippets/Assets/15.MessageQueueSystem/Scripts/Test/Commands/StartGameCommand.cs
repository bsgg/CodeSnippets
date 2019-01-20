using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class StartGameCommand : AbstractCommand
    {
        protected override void OnStart(DependencyContainer a_dependancyContainer)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnExecute()
        {
            m_messageService.Send(Messages.SHOW_PANEL_START_GAME);
        }
    }
}

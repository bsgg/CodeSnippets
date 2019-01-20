using UnityEngine;
using System.Collections;

namespace MessageQueue
{
    public class GameBuilder : DependencyBuilder
    {
        protected override void OnAwake()
        {
            MapCommand<StartGameCommand>(Messages.START_GAME);
            MapCommand<EndGameCommand>(Messages.END_GAME);
        }
    }
}

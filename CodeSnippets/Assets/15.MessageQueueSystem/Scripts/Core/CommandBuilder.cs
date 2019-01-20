using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageQueue
{
    public class CommandBuilder : MonoBehaviour
    {
        private DependencyContainer m_dependencyContainer;

        private List<AbstractCommandBase> m_commandList;

        public void Initialise(DependencyContainer a_container)
        {
            m_dependencyContainer = a_container;
            m_commandList = new List<AbstractCommandBase>();
        }

        public void Map<T>(string id) where T : AbstractCommandBase, new()
        {
            T command = new T();
            command.Initialise(id, m_dependencyContainer);

            m_commandList.Add(command);
        }
    }
}

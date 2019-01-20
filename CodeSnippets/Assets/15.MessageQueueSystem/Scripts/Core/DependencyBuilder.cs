using UnityEngine;
using System;

namespace MessageQueue
{
    public abstract class DependencyBuilder : MonoBehaviour
    {
        public event Action<DependencyContainer> ContainerConstructed;

        [SerializeField] private CommandBuilder m_commandBuilder;

        private DependencyContainer m_container;

        private void Awake()
        {
            m_container = new DependencyContainer();
            m_commandBuilder.Initialise(m_container);

            AddService<MessengerService>();
            OnAwake();
        }

        public void AddDependancy<T>(T dependancyObject)
        {
            m_container.Add<T>(dependancyObject);
        }

        public void AddService<T>() where T : new()
        {
            ServiceLocator.AddService<T>();
        }

        public void MapCommand<T>(string id) where T : AbstractCommandBase, new()
        {
            m_commandBuilder.Map<T>(id);
        }

        protected abstract void OnAwake();
    }
}

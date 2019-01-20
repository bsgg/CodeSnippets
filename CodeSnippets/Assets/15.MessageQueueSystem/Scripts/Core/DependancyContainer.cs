using System.Collections.Generic;
using System;

namespace MessageQueue
{
    public class DependencyContainer
    {
        private Dictionary<Type, object> m_dependencyMap;

        public DependencyContainer()
        {
            m_dependencyMap = new Dictionary<Type, object>();
        }

        public void Add<T>(object a_dependency)
        {
            m_dependencyMap.Add(typeof(T), a_dependency);
        }

        public T Get<T>()
        {
            return (T)m_dependencyMap[typeof(T)];
        }
    }
}
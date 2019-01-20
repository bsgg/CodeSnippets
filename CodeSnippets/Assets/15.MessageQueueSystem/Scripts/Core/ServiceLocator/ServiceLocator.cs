using UnityEngine;
using System.Collections.Generic;

namespace MessageQueue
{
    public static class ServiceLocator
    {
        private static IDictionary<object, object> m_services = new Dictionary<object, object>();

        public static T GetService<T>()
        {
            try
            {
                return (T)m_services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new UnityException("The requested service is not registered");
            }
        }

        public static void AddService<T>() where T : new()
        {
            m_services.Add(typeof(T), new T());
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace MessageQueue
{
    public class MessengerService
    {
        private static IDictionary<string, List<Delegate>> m_subscribers = new Dictionary<string, List<Delegate>>();

        public void Subscribe<T>(string a_messageID, Action<T> a_callback)
        {
            AddSubscriber(a_messageID, a_callback);
        }

        public void Subscribe(string a_messageID, Action a_callback)
        {
            AddSubscriber(a_messageID, a_callback);
        }


        public void AddSubscriber(string a_messageID, Delegate a_callback)
        {
            if (MessageExists(a_messageID))
            {
                m_subscribers[a_messageID].Add(a_callback);
            }
            else
            {
                m_subscribers[a_messageID] = new List<Delegate>
            {
                a_callback
            };
            }
        }

        public void Send<T>(string a_messageID, T param)
        {
            if (MessageExists(a_messageID))
            {
                List<Delegate> callbacks = m_subscribers[a_messageID];
                for (int i = 0; i < callbacks.Count; i++)
                {
                    Action<T> callback = (Action<T>)callbacks[i];

                    callback(param);
                }
            }
        }

        public void Send(string a_messageID)
        {
            if (MessageExists(a_messageID))
            {
                List<Delegate> callbacks = m_subscribers[a_messageID];
                for (int i = 0; i < callbacks.Count; i++)
                {
                    Action callback = (Action)callbacks[i];
                    callback();
                }
            }
        }

        public void Unsubscribe(string a_messageID, Action a_callback)
        {
            RemoveSubscriber(a_messageID, a_callback);
        }

        public void Unsubscribe<T>(string a_messageID, Action<T> a_callback)
        {
            RemoveSubscriber(a_messageID, a_callback);
        }

        public void RemoveSubscriber(string a_messageID, Delegate a_callback)
        {
            if (MessageExists(a_messageID))
            {
                List<Delegate> callbacks = m_subscribers[a_messageID];

                for (int i = 0; i < callbacks.Count; i++)
                {
                    Delegate tmpCallback = (Delegate)callbacks[i];
                    if (tmpCallback.Equals(a_callback))
                    {
                        callbacks.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private bool MessageExists(string a_messageID)
        {
            return m_subscribers.ContainsKey(a_messageID);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class UICodeSnippets : MonoBehaviour
    {
        #region Instance
        private static UICodeSnippets m_Instance;
        public static UICodeSnippets Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (UICodeSnippets)FindObjectOfType(typeof(UICodeSnippets));

                    if (m_Instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(UICodeSnippets) + " is needed in the scene, but there is none.");
                    }
                }
                return m_Instance;
            }
        }
        #endregion Instance


        [SerializeField] private UIHelper m_UIHelper;

        public UIHelper UIHelper
        {
            get { return m_UIHelper; }
        }

        [SerializeField] private GameObject m_ConsoleDebug;
        [SerializeField] private bool m_EnableConsole = false;
        [SerializeField] private Text m_Debug;

        public string Log
        {
            get { return m_Debug.text; }
            set { m_Debug.text = value; }
        }

        private void Start()
        {
            m_ConsoleDebug.SetActive(m_EnableConsole);
        }

    }
}

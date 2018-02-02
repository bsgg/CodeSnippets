using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssetBundleTool
{
    public class AssetBundleUI : MonoBehaviour
    {
        [SerializeField] private Text m_Messages;
        public string Messages
        {
            get { return m_Messages.text; }
            set { m_Messages.text = value; }
        }

        [SerializeField] private Button m_DownloadButton;
        public Button DownloadButton
        {
            get { return m_DownloadButton; }
        }

        [SerializeField] private Button m_RemoveButton;
        public Button RemoveButton
        {
            get { return m_RemoveButton; }
        }
    }
}

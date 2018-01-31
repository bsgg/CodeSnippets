using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleTool
{
    [Serializable]
    public class AssetMetaData
    {
        public string ID;
        public string Reference;
    }

    public class AssetObject : MonoBehaviour
    {
        [SerializeField] private string m_NameObject = "";
        public string NameObject
        {
            get { return m_NameObject; }
        }
        [SerializeField] private TextAsset m_MetaData;
        public TextAsset MetaData
        {
            get { return m_MetaData; }
            set { m_MetaData = value; }
        }

        [SerializeField] private AssetMetaData m_Data;
        public AssetMetaData AssetMetaData
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        private void Start()
        {
            if ((m_MetaData != null) && (!string.IsNullOrEmpty(m_MetaData.text)))
            {
                AssetMetaData = JsonUtility.FromJson<AssetMetaData>(m_MetaData.text);
            }
        }


    }
}

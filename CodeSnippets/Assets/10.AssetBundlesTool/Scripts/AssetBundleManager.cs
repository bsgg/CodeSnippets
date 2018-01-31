using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AssetBundleTool
{
    [Serializable]
    public class IndexFile
    {
        public string ID;
        public string AndroidFile;        
        public string IOSFile;
        public int BundleVersion;
    }

    [Serializable]
    public class FileData
    {
        public List<IndexFile> Data;
        public FileData()
        {
            Data = new List<IndexFile>();
        }
    }

    public class AssetBundleManager : MonoBehaviour
    {      

        [SerializeField] private string m_AssetBundlesUrl = "beatrizcv.com//Data/AssetBundles/";
        [SerializeField] private string m_IndexFileData = "FileData.json";
        [SerializeField] private FileData m_FileData;

        [SerializeField] private FileData m_LocalFileData;


        [SerializeField] private List<GameObject> m_AssetBundleList;
        [SerializeField] private Transform m_AssetBundleParent;

        private int m_NumberBundlesLoaded;
        private int m_TotalBundlesToLoad;

        private string m_AssetBundlesPersistentPath;


        private void Start()
        {
            StartCoroutine(LoadBundles());

        }

        private IEnumerator LoadBundles()
        {
            // Persisten data asset bundle
            m_AssetBundlesPersistentPath = Path.Combine(Application.persistentDataPath, "AssetBundles");

            // Create directory if doesn't exsit
            if (!Directory.Exists(m_AssetBundlesPersistentPath))
            {
                Directory.CreateDirectory(m_AssetBundlesPersistentPath);
            }

            m_NumberBundlesLoaded = 0;
            m_TotalBundlesToLoad = 0;
            m_AssetBundleList = new List<GameObject>();

            yield return RequestIndexDataFile();

            if (m_FileData.Data != null)
            {
                m_TotalBundlesToLoad = m_FileData.Data.Count;
                if (m_FileData.Data.Count ==0)
                {
                    Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] 0 Bundles to load: " + "</color>");

                }
                else
                {

                    Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] Number of files: " + m_FileData.Data.Count + "</color>");

                    for (int i = 0; i < m_FileData.Data.Count; i++)
                    {
#if UNITY_IOS
                    Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] Requesting IOS Bundle: " + m_FileData.Data[i].IOSFile + "</color>");
                    yield return RequestBundle(m_FileData.Data[i].IOSFile);

#elif UNITY_ANDROID || UNITY_EDITOR
                        Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] Requesting Android Bundle: " + m_FileData.Data[i].AndroidFile + "</color>");
                        yield return RequestBundle(m_FileData.Data[i].AndroidFile);

#endif
                    }

                    Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] Bundles Loaded: " + m_NumberBundlesLoaded + " / " + m_TotalBundlesToLoad + "</color>");
                }

            }else
            {
                Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] No bundles to load: " + "</color>");
            }
        }

        private IEnumerator RequestIndexDataFile()
        {
            m_FileData = new FileData();

            if (string.IsNullOrEmpty(m_IndexFileData))
            {
                Debug.Log("<color=yellow>" + "[AssetBundleManager.LoadBundles] Index File Data is empty" + "</color>");
                yield return null;
            }
           

            string filePath = Path.Combine(m_AssetBundlesUrl, m_IndexFileData);
            WWW wwwFile = new WWW(filePath);
            yield return wwwFile;
            string jsonData = wwwFile.text;

            if (!string.IsNullOrEmpty(jsonData))
            {
                m_FileData = JsonUtility.FromJson<FileData>(jsonData);
            }


            // Check if file index exist in persistent data
            string localFileIndexPath = Path.Combine(m_AssetBundlesPersistentPath, m_IndexFileData);
            Debug.Log("<color=blue>" + "[AssetBundleManager.LoadBundles] Retrieving index file data from local: " + localFileIndexPath + "</color>");
            if (File.Exists(localFileIndexPath))
            {
                // Retrive file
                StreamReader reader = new StreamReader(localFileIndexPath);
                string text = reader.ReadToEnd();
                reader.Close();

                Debug.Log("<color=blue>" + "[AssetBundleManager.LoadBundles] Local file exits: " + text + "</color>");

                if (!string.IsNullOrEmpty(text))
                {
                    m_LocalFileData = JsonUtility.FromJson<FileData>(text);
                }
            }else
            {
                m_LocalFileData = m_FileData;
                Debug.Log("<color=blue>" + "[AssetBundleManager.LoadBundles] File local index Doesn't exist saving... " + localFileIndexPath + "</color>");
                // Save 
                byte[] bytes = wwwFile.bytes;
                File.WriteAllBytes(localFileIndexPath, bytes);
                yield return new WaitForSeconds(0.5f);
            }
        }

       
        private IEnumerator RequestBundle(string nameBundle)
        {
            if (string.IsNullOrEmpty(nameBundle)) yield return null;

            while (!Caching.ready)
                yield return null;


            string bundlePath = Path.Combine(m_AssetBundlesPersistentPath, nameBundle);

            Debug.Log("<color=yellow>" + "[AssetBundleManager.RequestBundle] Checking bundle on path: " + bundlePath  + "</color>");

            if (File.Exists(bundlePath))
            {
                Debug.Log("<color=yellow>" + "[AssetBundleManager.RequestBundle] File exists: " + bundlePath + "</color>");

                // Create asset bundle from file
                AssetBundle localBundle = AssetBundle.LoadFromFile(bundlePath);
                AssetBundleRequest requestLocal = localBundle.LoadAllAssetsAsync();

                // Wait for completion
                yield return requestLocal;

                if(requestLocal.allAssets != null)
                { 
                    ProcessAssetBundleRequest(requestLocal, nameBundle);
                }
            }
            else
            {

                string filePath = Path.Combine(m_AssetBundlesUrl, nameBundle);
                WWW www = new WWW(filePath);

                //WWW.LoadFromCacheOrDownload(filePath, 1);

                Debug.Log("AssetBundleManager.LoadBundle path: " + filePath);

                // Wait for download to complete
                yield return www;

                Debug.Log("AssetBundleManager.LoadBundle finish - bytesDownloaded: " + www.bytesDownloaded);

                if ((www.bytesDownloaded > 0) && (www.assetBundle != null))
                {
                    byte[] bytes = www.bytes;

                    // Creates a new file, writes the specified byte array to the file, and then closes the file. 
                    // If the target file already exists, it is overwritten.
                    File.WriteAllBytes(bundlePath, bytes);

                    yield return new WaitForSeconds(0.5f);

                    // Load and retrieve the AssetBundle
                    AssetBundle bundle = www.assetBundle;

                    // Load the object asynchronously
                    AssetBundleRequest request = bundle.LoadAllAssetsAsync();

                    // Wait for completion
                    yield return request;

                    if (request.allAssets != null)
                    {
                        ProcessAssetBundleRequest(request, nameBundle);
                    }
                    else
                    {
                        Debug.Log("Could not load objects in theatre bundle: ");
                    }

                    // Unload the AssetBundles compressed contents to conserve memory
                    bundle.Unload(false);
                }

                // Frees the memory from the web stream
                www.Dispose();
            }
        }


        private void ProcessAssetBundleRequest(AssetBundleRequest request, string assetID)
        {
            Debug.Log(string.Format("Successfully loaded {0} objects", request.allAssets.Length));

            try
            { 
                //Instantiate each of the loaded objects and add them to the group
                foreach (UnityEngine.Object o in request.allAssets)
                {
                    GameObject go = o as GameObject;
                    GameObject instantiatedGO = Instantiate(go);

                    m_NumberBundlesLoaded++;
                    m_AssetBundleList.Add(go);

                    // Retrieve Asset Object script
                    AssetObject asset = go.GetComponent<AssetObject>();
                    if (asset  != null)
                    {
                        Debug.Log("AssetObject NOT NULL " + asset.NameObject);
                        if (asset.MetaData != null)
                        {
                            Debug.Log("AssetObject  asset.TextAsset: " + asset.MetaData.text);
                        }
                    }else
                    {
                        Debug.Log("AssetObject NULL ");
                    }
                
                }
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load asset bundle, reason: " + e.Message);
            }
        }
                
    }
}

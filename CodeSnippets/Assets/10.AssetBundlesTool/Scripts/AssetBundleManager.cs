using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundleTool
{
    [Serializable]
    public class IndexFile
    {
        public string ID;
        public string AndroidFile;        
        public string IOSFile;
        public int BundleVersion;
        public AssetBundleManager.EBundleAction BundleAction;
        public GameObject BundleObject;
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
        public enum EBundleAction { NONE, LOADFROMLOCAL, LOADFROMSERVER};

        [Header("UI")]
        [SerializeField] private AssetBundleUI m_UI;

        [Header("Settings")]
        
        //[SerializeField] private string m_AssetBundlesUrl = "beatrizsanchez1986.000webhostapp.com/AssetBundles/";
        [SerializeField] private string m_indexFileData = "FileData.json";

        [SerializeField] private FileData m_FileData;

        //[SerializeField] private List<GameObject> m_AssetBundleList;
        [SerializeField] private Transform m_AssetBundleParent;

        private int m_NumberBundlesLoaded;
        private int m_TotalBundlesToLoad;

        
        private int m_Progress = 0;

        [Header("AssetBundle Settings")]
        [SerializeField] private string m_indexFileName = "AssetBundleData.json";
        [SerializeField] private string m_folderName = "AssetBundles";

        [Header("Cloud Settings")]        
        [SerializeField] private string m_assetBundlesUrl = "beatrizcv.com//Data/AssetBundles/";
        private string m_uri;


        private string m_localDirectory;
        private string m_localIndexFileUrl;

        private bool m_requestResult;
        private FileData m_cloundDataFile;
        private FileData m_localDataFile;

        private void Start()
        {
            SetupDirectories();






            // Persisten data asset bundle
            //m_AssetBundlesPersistentPath = Path.Combine(Application.persistentDataPath, "AssetBundles");

            m_UI.Messages = "";
            m_UI.RemoveButton.interactable = true;
            m_UI.DownloadButton.interactable = true;

            Utility.UICodeSnippets.Instance.Log = "";

            // Create directory if doesn't exsit
            

            m_NumberBundlesLoaded = 0;
            m_TotalBundlesToLoad = 0;

            m_FileData = new FileData();
        }

        private void SetupDirectories()
        {
            m_localDirectory = Path.Combine(Application.persistentDataPath, m_folderName);

            if (!Directory.Exists(m_localDirectory))
            {
                Directory.CreateDirectory(m_localDirectory);
            }

            m_localIndexFileUrl = Path.Combine(m_localDirectory, m_indexFileName);

            m_uri = Path.Combine(m_assetBundlesUrl, m_indexFileName);

        }

        public void LoadBundles()
        {
            // None bundles loaded
            if (m_NumberBundlesLoaded == 0)
            {
                StartCoroutine(LoadBundlesRoutine());
            }else
            {
                m_UI.Messages = "All bundles downloaded";
            }
        }

        public void RemoveBundles()
        { 
            StartCoroutine(Remove());
        }

        

        private void UpdateProgressUI()
        {
            m_UI.Messages = "Loading bundles:" + m_Progress + "%";
        }

        #region Load
        private IEnumerator LoadBundlesRoutine()
        {
            m_UI.RemoveButton.interactable = false;
            m_UI.DownloadButton.interactable = false;

            m_Progress = 0;
            UpdateProgressUI();


            // TODO: CHECK INTERNET
            m_requestResult = false;
            yield return DownloadIndexFile();

            if (!m_requestResult)
            {
                Debug.Log("<color=purple>" + "[AssetBundleManager.LoadBundlesRoutine] File Index Downloaded Incorrectly " + "</color>");

                yield return null;
            }

            Debug.Log("<color=purple>" + "[AssetBundleManager.LoadBundlesRoutine] File Index Downloaded Correctly " + "</color>");

            m_Progress = 1;
            UpdateProgressUI();

            Debug.Log("<color=purple>" + "[AssetBundleManager.LoadIndexFileAssetBundlesFromLocal] Load Index File from : " + m_localIndexFileUrl + "</color>");

            if (!File.Exists(m_localIndexFileUrl))// File exists
            {
                // Save file from first time
                m_localDataFile = m_cloundDataFile;

                WriteFileData(m_localDataFile, m_localIndexFileUrl);

            }
            else
            {
                m_requestResult = false;
                yield return LoadIndexFileFromLocal();
                if (!m_requestResult)
                {
                    Debug.Log("<color=purple>" + "[AssetBundleManager.LoadBundlesRoutine] Local File Index Loaded Incorrectly " + "</color>");

                    // Save file

                    yield return null;
                }

                Debug.Log("<color=purple>" + "[AssetBundleManager.LoadBundlesRoutine] Local File Index loaded Correctly " + "</color>");

            }


            // Compare both files
            yield return CheckDataFiles(m_localDataFile, m_cloundDataFile);

            if (m_FileData.Data.Count == 0)
            {
                Utility.UICodeSnippets.Instance.Log += "<color=red>" + "There are (0) asset bundles to load" + "\n</color>";
                Debug.Log("<color=purple>" + "[AssetBundleManager] No Bundles to load " + "</color>");
            }else
            {
                for (int i=0; i< m_FileData.Data.Count;i++)
                {
                    yield return RequestAssetBundle(m_FileData.Data[i]);
                }
            }

            m_UI.RemoveButton.interactable = true;
            m_UI.DownloadButton.interactable = true;

            m_Progress = 100;
            UpdateProgressUI();




            /*m_Progress = 100;
            UpdateProgressUI();
            m_UI.RemoveButton.interactable = true;
            m_UI.DownloadButton.interactable = true;

            */

        }

        private void WriteFileData(FileData a_file, string a_path)
        {
            if (a_file == null) return;

            try
            {
                if (File.Exists(a_path))
                {
                    File.Delete(a_path);
                }

                string dataAsJson = JsonUtility.ToJson(a_file);
                File.WriteAllText(a_path, dataAsJson);

            }
            catch (Exception e)
            {
                Debug.Log("<color=purple>" + "[AssetBundleManager.WriteFileData] Exception trying to write fileData" + "</color>");
            }
        }
       

        private IEnumerator DownloadIndexFile()
        {
            m_requestResult = false;

            if (string.IsNullOrEmpty(m_indexFileData) || string.IsNullOrEmpty(m_uri))
            {
                Debug.Log("<color=purple>" + "[AssetBundleManager.DownloadIndexFile] Error: Index File Data is empty or incorred uri" + "</color>");

                yield return null;
            }

            Debug.Log("<color=purple>" + "[AssetBundleManager.DownloadIndexFile] Downloading from " + m_uri + "</color>");

            using (UnityWebRequest webRequest = UnityWebRequest.Get(m_uri))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log("<color=purple>" + "[AssetBundleManager.DownloadIndexFile] Webrequest Error: " + webRequest.error + "</color>");
                    
                    yield return null;
                }

                Debug.Log("<color=purple>" + "[AssetBundleManager.DownloadIndexFile] Webrequest Success " + "</color>");

                m_cloundDataFile = new FileData();

                if (!string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    try
                    {
                        m_cloundDataFile = JsonUtility.FromJson<FileData>(webRequest.downloadHandler.text);
                        m_requestResult = true;
                    }
                    catch (Exception e)
                    {
                        Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: Malformed Json Cloud Index File" + "\n</color>";
                        Debug.Log("<color=purple>" + "[AssetBundleManager] ERROR: Malformed index JSON SERVER File" + "</color>");
                    }
                }
                else
                {
                    Debug.Log("<color=purple>" + "[AssetBundleManager.DownloadIndexFile] Error: Json Cloud Index File is empty or null " + "</color>");

                    Utility.UICodeSnippets.Instance.Log += "<color=red>" + "Error: Json Cloud Index File is empty or null" + "\n</color>";

                    
                }

                yield return null;
            }
        }

        private IEnumerator LoadIndexFileFromLocal()
        {
            m_requestResult = false;
            
            // Read local file
            using (StreamReader reader = new StreamReader(m_localIndexFileUrl))
            {
                string jsonData = reader.ReadToEnd();
                reader.Close();

                Debug.Log("<color=purple>" + "[AssetBundleManager.LoadIndexFileFromLocal] Webrequest Success " + "</color>");

                m_localDataFile = new FileData();

                if (!string.IsNullOrEmpty(jsonData))
                {
                    try
                    {
                        m_localDataFile = JsonUtility.FromJson<FileData>(jsonData);
                        m_requestResult = true;
                    }
                    catch (Exception e)
                    {
                        Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: Malformed Json Local Index File" + "\n</color>";
                        Debug.Log("<color=purple>" + "[AssetBundleManager.LoadIndexFileFromLocal] ERROR: Malformed index JSON Local File" + "</color>");
                    }
                }
                else
                {
                    Debug.Log("<color=purple>" + "[AssetBundleManager.LoadIndexFileFromLocal] Error: Json Local Index File is empty or null " + "</color>");

                    Utility.UICodeSnippets.Instance.Log += "<color=red>" + "Error: Json Local Index File is empty or null" + "\n</color>";
                }
            }
            yield return null;
            
        }


        private IEnumerator CheckDataFiles(FileData a_localData, FileData a_cloudData)
        {
            if ((a_localData == null) || 
                (a_cloudData == null) || 
                (a_localData.Data == null) || 
                (a_cloudData.Data == null)) yield return null;


            for (int iServer = 0; iServer < a_cloudData.Data.Count; iServer++)
            {
                // Set data as  load from server as default
                a_cloudData.Data[iServer].BundleAction = EBundleAction.LOADFROMSERVER;

                // Find server data in local file
                if (FindDataInFileData(a_localData, a_cloudData.Data[iServer]))
                {
                    // Get local data path
                    string assetbundleLocalPath = string.Empty;

#if UNITY_IOS
                    assetbundleLocalPath = Path.Combine(m_localDirectory, newIndex.IOSFile);

#elif UNITY_ANDROID || UNITY_EDITOR

                    assetbundleLocalPath = Path.Combine(m_localDirectory, a_cloudData.Data[iServer].AndroidFile);
#endif
                    // Check if asset bundle exists in local
                    if (File.Exists(assetbundleLocalPath))
                    {
                        // Same version, load from local
                        if (a_localData.Data[iServer].BundleVersion == a_cloudData.Data[iServer].BundleVersion)
                        {
                            a_cloudData.Data[iServer].BundleAction = EBundleAction.LOADFROMLOCAL;

                        }
                        else // Local version is less or greater than server version, load from server
                        {
                            Debug.Log("<color=purple>" + "[AssetBundleManager.CheckDataFiles] AssetBundle: " + a_localData.Data[iServer].ID + " Local Version:  " + a_localData.Data[iServer].BundleVersion + " Server Version " + a_cloudData.Data[iServer].BundleVersion + " </color>");
                            // Remove file from local
                            File.Delete(assetbundleLocalPath);
                        }
                    }

                }

                m_FileData.Data.Add(a_cloudData.Data[iServer]);
            }

            // Update local file
            WriteFileData(m_FileData, m_localIndexFileUrl);

            yield return new WaitForSeconds(0.5f);
        }

        private bool FindDataInFileData(FileData a_fileData, IndexFile a_data)
        {
            if ((a_fileData == null) || (a_data == null)) return false;

            for (int iData = 0; iData < a_fileData.Data.Count; iData++)
            {
                int compare = string.Compare(a_fileData.Data[iData].ID, a_data.ID, true);

                if (compare == 0) // same ID
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator RequestAssetBundle(IndexFile a_data)
        {
            if (a_data == null) yield return null;

            string bundleName = string.Empty;

#if UNITY_ANDROID || UNITY_EDITOR
            bundleName = a_data.AndroidFile;
#elif UNITY_IOS
            bundleName = a_data.IOSFile;
#endif
            if (string.IsNullOrEmpty(bundleName)) yield return null;

            Debug.Log("<color=purple>" + "[AssetBundleManager.RequestAssetBundle] Requesting " + bundleName + " </color>");

            // Check if caching is ready
            while (!Caching.ready)
                yield return null;

            string bundlePath = Path.Combine(m_localDirectory, bundleName);

            // Load asset from local
            if (a_data.BundleAction == EBundleAction.LOADFROMLOCAL)
            {
                // Check if reload
                bool loadFromLocal = true;

                // Check if there is an existing element in memory
                if (a_data.BundleObject != null)
                {
                    // Different name, destroy existing
                    if (a_data.ID != a_data.BundleObject.name)
                    {
                        Destroy(a_data.BundleObject);

                        yield return new WaitForEndOfFrame();

                    }
                    else
                    {
                        loadFromLocal = false;
                    }
                }

                if (loadFromLocal && (File.Exists(bundlePath)))
                {
                    // Load from file and wait for completion
                    AssetBundle localBundle = AssetBundle.LoadFromFile(bundlePath);
                    AssetBundleRequest requestLocal = localBundle.LoadAllAssetsAsync();

                    yield return requestLocal;

                    if (requestLocal.allAssets != null)
                    {
                        a_data.BundleObject = ProcessAssetBundleRequest(requestLocal, a_data.ID);
                    }

                    // Unload the AssetBundles compressed contents to conserve memory
                    localBundle.Unload(false);
                }
                else
                {
                    // If in this point (this should not happens) the file doesn't exit, set action to load from server
                    a_data.BundleAction = EBundleAction.LOADFROMSERVER;
                }

            }

            if (a_data.BundleAction == EBundleAction.LOADFROMSERVER)
            {
                string uri = Path.Combine(m_assetBundlesUrl, bundleName);

               

                using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri))
                {
                    yield return webRequest.SendWebRequest();



                    if (webRequest.isNetworkError || webRequest.isHttpError)
                    {
                        Debug.Log("<color=purple>" + "[AssetBundleManager.RequestAssetBundle] Webrequest Error: " + webRequest.error + "</color>");
                        yield return null;
                    }
                    else
                    {
                        Debug.Log("<color=purple>" + "[AssetBundleManager.RequestAssetBundle] Asset Bundle downloaded: " + webRequest.downloadedBytes + "</color>");

                        //SAVE ON DISK NOT SUPPORTED: NOT SUPPORTED WITH  GET ASSET BUNDLE

                        //File.WriteAllBytes(bundlePath, webRequest.downloadHandler.data);
                        // Get downloaded asset bundle
                        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);                        

                        if (bundle == null)
                        {
                            Debug.Log("<color=purple>" + "[AssetBundleManager.RequestAssetBundle] Asset Bundle is null "  + "</color>");

                            yield return null;
                        }

                        // Load the object asynchronously
                        AssetBundleRequest request = bundle.LoadAllAssetsAsync();

                        // Wait for completion
                        yield return request;

                        if (request.allAssets != null)
                        {
                            // Destroy existing object
                            if (a_data.BundleObject != null)
                            {
                                Destroy(a_data.BundleObject);
                            }
                            yield return new WaitForEndOfFrame();

                            a_data.BundleObject = ProcessAssetBundleRequest(request, bundleName);
                        }

                        bundle.Unload(false);
                    }
                }
            }
        }

        #endregion Load

        private GameObject ProcessAssetBundleRequest(AssetBundleRequest request, string assetID)
        {
            Debug.Log(string.Format("Successfully loaded {0} objects", request.allAssets.Length));

            try
            { 
                //Instantiate each of the loaded objects and add them to the group
                foreach (UnityEngine.Object o in request.allAssets)
                {
                    GameObject go = o as GameObject;
                    GameObject instantiatedGO = Instantiate(go);
                    instantiatedGO.name = assetID;
                    instantiatedGO.transform.parent = m_AssetBundleParent;

                    m_NumberBundlesLoaded++;

                    // Retrieve Asset Object script
                    AssetObject asset = go.GetComponent<AssetObject>();
                    if (asset  != null)
                    {
                        if (asset.MetaData != null)
                        {
                            // DO ACTION

                        }else
                        {
                            Debug.Log("<color=purple>" + "[AssetBundleManager] AssetObject Metadata not found in: " + assetID + "</color>");
                        }
                    }else
                    {
                        Debug.Log("<color=purple>" + "[AssetBundleManager] AssetObject not found in: " + assetID + "</color>");
                    }

                    return instantiatedGO;
                
                }
            }
            catch (Exception e)
            {
                Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: Failed to load asset bundle, reason " + e.Message + "\n</color>";

                Debug.Log("Failed to load asset bundle, reason: " + e.Message);
            }

            return null;
        }

        #region Remove

        private IEnumerator Remove()
        {
            m_UI.RemoveButton.interactable = false;
            m_UI.DownloadButton.interactable = false;

            m_UI.Messages = "Removing bundles";

            if (Directory.Exists(""))
            {
                string[] files = Directory.GetFiles("");

                if (files != null)
                {
                    m_UI.Messages = files.Length + " Files found";

                    int nFilesDeleted = 0;
                    for (int i = files.Length - 1; i >= 0; i--)
                    {

                        try
                        {
                            File.Delete(files[i]);

                        }
                        catch (Exception e)
                        {
                            Debug.Log("Exception " + e.Message);
                        }
                        yield return new WaitForSeconds(0.5f);
                        nFilesDeleted++;

                        m_UI.Messages = " Deleted (" + nFilesDeleted + "/" + files.Length + ")";
                    }
                }

                yield return new WaitForSeconds(0.5f);

                // Remove from scene
                if ((m_FileData != null) && (m_FileData.Data != null))
                {
                    for (int i = m_FileData.Data.Count - 1; i >= 0; i--)
                    {
                        if (m_FileData.Data[i].BundleObject != null)
                        {
                            DestroyImmediate(m_FileData.Data[i].BundleObject);
                            m_FileData.Data[i].BundleObject = null;
                        }
                    }
                }

                yield return new WaitForSeconds(0.5f);

                m_UI.Messages = "Action completed";
            }
            else
            {
                m_UI.Messages = "Action completed, no files in local folder";
            }

            m_NumberBundlesLoaded = 0;
            m_UI.RemoveButton.interactable = true;
            m_UI.DownloadButton.interactable = true;
        }

        #endregion Remove
    }
}

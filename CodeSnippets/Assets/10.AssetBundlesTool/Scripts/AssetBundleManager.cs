﻿using System;
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
        public AssetBundleManager.EBundleAction BundleAction;
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
        [SerializeField] private string m_AssetBundlesUrl = "beatrizcv.com//Data/AssetBundles/";
        [SerializeField] private string m_IndexFileData = "FileData.json";

        [SerializeField] private FileData m_FileData;

        [SerializeField] private List<GameObject> m_AssetBundleList;
        [SerializeField] private Transform m_AssetBundleParent;

        private int m_NumberBundlesLoaded;
        private int m_TotalBundlesToLoad;

        private string m_AssetBundlesPersistentPath;
        
        public void LoadBundle()
        {
            StartCoroutine(LoadBundlesRoutine());
        }

        private IEnumerator LoadBundlesRoutine()
        {
            Utility.UICodeSnippets.Instance.Log = "";

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

            m_FileData = new FileData();

            yield return RequestIndexDataFile();
            
            m_TotalBundlesToLoad = m_FileData.Data.Count;
            if (m_FileData.Data.Count ==0)
            {
                Utility.UICodeSnippets.Instance.Log += "<color=red>" + "There are (0) asset bundles to load" + "\n</color>";
                Debug.Log("<color=purple>" + "[AssetBundleManager] No Bundles to load " + "</color>");
            }
            else
            {

                Utility.UICodeSnippets.Instance.Log += "There are ("+ m_FileData.Data.Count + ") asset bundles to load" + "\n";
                Debug.Log("<color=purple>" + "[AssetBundleManager] Number of files: " + m_FileData.Data.Count + "</color>");

                for (int i = 0; i < m_FileData.Data.Count; i++)
                {

                    yield return RequestBundle(m_FileData.Data[i]);
                }

                Utility.UICodeSnippets.Instance.Log += "Completed: " + m_NumberBundlesLoaded + "/" + m_TotalBundlesToLoad + "\n";
                Debug.Log("<color=purple>" + "[AssetBundleManager] Bundles Loaded: " + m_NumberBundlesLoaded + " / " + m_TotalBundlesToLoad + "</color>");
            }

            
        }

        /// <summary>
        /// Routine to request and compare the index file data (local and server)
        /// </summary>
        /// <returns></returns>
        private IEnumerator RequestIndexDataFile()
        {
            if (string.IsNullOrEmpty(m_IndexFileData))
            {
                Debug.Log("<color=purple>" + "[AssetBundleManager] Index File Data is empty" + "</color>");
                yield return null;
            }

            // Try to retrieve server index file data
            string filePath = Path.Combine(m_AssetBundlesUrl, m_IndexFileData);
            WWW wwwFile = new WWW(filePath);

            yield return wwwFile;

            string jsonData = wwwFile.text;


            FileData serverFile = new FileData();
            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    serverFile = JsonUtility.FromJson<FileData>(jsonData);
                }
                catch (Exception e)
                {
                    Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: Malformed index JSON SERVER File" + "\n</color>";
                    Debug.Log("<color=purple>" + "[AssetBundleManager] ERROR: Malformed index JSON SERVER File" + "</color>");
                }
            }else
            {
                Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: File index data JSON is empty " +  "\n</color>";
                Debug.Log("<color=purple>" + "[AssetBundleManager] ERROR: File index data JSON is empty" + "</color>");

                yield return null;
            }


            // Try to retrieve local index file data
            string localFileIndexPath = Path.Combine(m_AssetBundlesPersistentPath, m_IndexFileData);
            Debug.Log("<color=purple>" + "[AssetBundleManager] Retrieving index file data from local: " + localFileIndexPath + "</color>");

            if (File.Exists(localFileIndexPath))// File exists
            {
                Utility.UICodeSnippets.Instance.Log += "Found local index file in local folder: " + localFileIndexPath + "\n";
                Debug.Log("<color=purple>" + "[AssetBundleManager] Found local index file in local folder: " + localFileIndexPath + "</color>");

                // Retrive file
                StreamReader reader = new StreamReader(localFileIndexPath);
                string text = reader.ReadToEnd();
                reader.Close();

                if (!string.IsNullOrEmpty(text))
                {
                    FileData localFile = new FileData();
                    try
                    {
                        localFile = JsonUtility.FromJson<FileData>(text);
                    }
                    catch (Exception e)
                    {
                        Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: Malformed index JSON LOCAL File" + "\n</color>";
                        Debug.Log("<color=purple>" + "[AssetBundleManager] ERROR: Malformed index JSON LOCAL File" + "</color>");
                    }

                    if ((localFile.Data != null && serverFile.Data != null))
                    {
                        // Compare server files with local server
                        for (int iServer = 0; iServer < serverFile.Data.Count; iServer++)
                        { 
                            IndexFile newIndex = new IndexFile();
                            newIndex.ID = serverFile.Data[iServer].ID;
                            newIndex.AndroidFile = serverFile.Data[iServer].AndroidFile;
                            newIndex.IOSFile = serverFile.Data[iServer].IOSFile;


                            bool fileFound = false;
                            for (int iLocal = 0; iLocal < localFile.Data.Count; iLocal++)
                            {
                                int compare = string.Compare(localFile.Data[iLocal].ID, serverFile.Data[iServer].ID, true);
                                if (compare == 0) // same ID
                                {
                                    fileFound = true;

                                    string bundleLocalPath = string.Empty;

#if UNITY_IOS
                                bundleLocalPath = Path.Combine(m_AssetBundlesPersistentPath, newIndex.IOSFile);

#elif UNITY_ANDROID || UNITY_EDITOR

                                    bundleLocalPath = Path.Combine(m_AssetBundlesPersistentPath, newIndex.AndroidFile);
#endif

                                    newIndex.BundleVersion = serverFile.Data[iServer].BundleVersion;

                                    // Check if file exists in local, 
                                    if (File.Exists(bundleLocalPath))// File exists
                                    {
                                        int localVersion = localFile.Data[iLocal].BundleVersion;
                                        int serverVersion = serverFile.Data[iServer].BundleVersion;
                                        if (localVersion == serverVersion)
                                        {
                                            newIndex.BundleAction = EBundleAction.LOADFROMLOCAL;

                                        }
                                        else if (serverVersion > localVersion)
                                        {
                                            Utility.UICodeSnippets.Instance.Log += "AssetBundle " + newIndex.ID + " New Bundle Version " + serverVersion +"\n";

                                            Debug.Log("<color=purple>" + "[AssetBundleManager] AssetBundle: " + newIndex.ID + " Current Version:  " + localVersion + " New Bundle Version " + serverVersion + " </color>");

                                            newIndex.BundleAction = EBundleAction.LOADFROMSERVER;

                                            // Remove file from local
                                            File.Delete(bundleLocalPath);
                                        }
                                    }
                                    else
                                    {
                                        // File doesn't exists in local, load form server
                                        newIndex.BundleAction = EBundleAction.LOADFROMSERVER;
                                    }
                                    
                                    break;
                                }
                            }

                            // File doesn't exist, load from server
                            if (!fileFound)
                            {
                                newIndex.BundleAction = EBundleAction.LOADFROMSERVER;
                            }

                            m_FileData.Data.Add(newIndex);
                        }

                        // Check if there is any extra files in local
                        if (localFile.Data.Count > serverFile.Data.Count)
                        {
                            for (int iLocal = 0; iLocal< localFile.Data.Count; iLocal++)
                            {
                                bool found = false;
                                string bundleLocalPath = string.Empty;
#if UNITY_IOS
                                bundleLocalPath = Path.Combine(m_AssetBundlesPersistentPath, localFile.Data[iLocal].IOSFile);

#elif UNITY_ANDROID || UNITY_EDITOR

                                bundleLocalPath = Path.Combine(m_AssetBundlesPersistentPath, localFile.Data[iLocal].AndroidFile);
#endif
                                for (int iServer = 0; iServer < serverFile.Data.Count; iServer++)
                                {
                                    int compare = string.Compare(localFile.Data[iLocal].ID, serverFile.Data[iServer].ID, true);
                                    if (compare == 0) // same ID
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    
                                    if (File.Exists(bundleLocalPath))// File exists
                                    {
                                        Utility.UICodeSnippets.Instance.Log += "Unnecessary AssetBundle found in local folder: " + bundleLocalPath + "\n";

                                        Debug.Log("<color=purple>" + "[AssetBundleManager] Unnecessary AssetBundle found in local folder: " + bundleLocalPath + " </color>");

                                        File.Delete(bundleLocalPath);
                                    }else
                                    {
                                        Utility.UICodeSnippets.Instance.Log += "Unnecessary AssetBundle found in local folder: (unnable to delete it): " + bundleLocalPath + "\n";

                                        Debug.Log("<color=purple>" + "[AssetBundleManager] Unnecessary AssetBundle found in local folder: (unnable to delete it: " + bundleLocalPath + "</color>");
                                    }
                                }
                            }
                        }

                        // Update local file
                        byte[] bytes = wwwFile.bytes;
                        File.WriteAllBytes(localFileIndexPath, bytes);
                        yield return new WaitForSeconds(0.5f);
                    }
                }

            }else // Doesn't exist, save the server one and take
            {
                // Final file is from the server
                m_FileData = serverFile;

                // Set actions
                for (int i=0; i< m_FileData.Data.Count; i++)
                {
                    m_FileData.Data[i].BundleAction = EBundleAction.LOADFROMSERVER;
                }

                Utility.UICodeSnippets.Instance.Log += "No files found in local. Files will be downloaded from the server"+ "\n";

                Debug.Log("<color=purple>" + "[AssetBundleManager] No files found in local. Files will be downloaded from the server" + "</color>");
                // Save file taking the srever one
                byte[] bytes = wwwFile.bytes;
                File.WriteAllBytes(localFileIndexPath, bytes);
                yield return new WaitForSeconds(0.5f);
            }
        }
        

        private IEnumerator RequestBundle(IndexFile index)
        {
            string nameBundle = string.Empty;

#if UNITY_ANDROID || UNITY_EDITOR
            nameBundle = index.AndroidFile;
#elif UNITY_IOS
            nameBundle = index.IOSFile;
#endif

            Utility.UICodeSnippets.Instance.Log += "Requesting " +  m_NumberBundlesLoaded + "/" + m_TotalBundlesToLoad + " : " + nameBundle + "\n";
            Debug.Log("<color=purple>" + "[AssetBundleManager] Requesting " +  m_NumberBundlesLoaded + " / " + m_TotalBundlesToLoad + " : " + nameBundle + " </color>");

            if (string.IsNullOrEmpty(nameBundle)) yield return null;

            while (!Caching.ready)
                yield return null;

            string bundlePath = Path.Combine(m_AssetBundlesPersistentPath, nameBundle);
            // Check action on this file
            if (index.BundleAction == EBundleAction.LOADFROMLOCAL) // Load file from local path
            {               
                if (File.Exists(bundlePath))
                {
                    Debug.Log("<color=purple>" + "[AssetBundleManager] Local file exits: " + bundlePath + "</color>");

                    // Load from local
                    // Create asset bundle from file
                    AssetBundle localBundle = AssetBundle.LoadFromFile(bundlePath);
                    AssetBundleRequest requestLocal = localBundle.LoadAllAssetsAsync();

                    // Wait for completion
                    yield return requestLocal;

                    if (requestLocal.allAssets != null)
                    {
                        ProcessAssetBundleRequest(requestLocal, nameBundle);
                    }

                }
                else
                {
                    // If in this point (this should not happens) the file doesn't exit, set action to load from server
                    index.BundleAction = EBundleAction.LOADFROMSERVER;
                }
            }


            if (index.BundleAction == EBundleAction.LOADFROMSERVER)
            {

                string filePath = Path.Combine(m_AssetBundlesUrl, nameBundle);
                WWW www = new WWW(filePath);

                // Wait for download to complete
                yield return www;

                Utility.UICodeSnippets.Instance.Log += "Download from server " + www.bytesDownloaded  + "\n";

                Debug.Log("<color=purple>" + "[AssetBundleManager] Load from server bytesDownloaded: " + www.bytesDownloaded + "</color>");

                if ((www.bytesDownloaded > 0) && (www.assetBundle != null))
                {
                    byte[] bytes = www.bytes;
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

                        Debug.Log("<color=purple>" + "[AssetBundleManager] Could not load from asset bundle" + "</color>");
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
                
                }
            }
            catch (Exception e)
            {
                Utility.UICodeSnippets.Instance.Log += "<color=red>" + "ERROR: Failed to load asset bundle, reason " + e.Message + "\n</color>";

                Debug.Log("Failed to load asset bundle, reason: " + e.Message);
            }
        }                
    }
}

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

namespace AssetBundleTool
{
    public class ExportAndroidBundles : MonoBehaviour
    {
        // .agt Extension  for Android
        // .igt Extension for IOS
        [MenuItem("AssetBundle/Build AssetBundles Android")]
        static void BuildAllAssetBundles()
        {
            string folder = Path.Combine(Application.dataPath, "10.AssetBundlesTool/AssetBundles");
            string directory = Path.Combine(folder, "Android");
            if (!Directory.Exists(directory))
            {
                //if it doesn't, create it
                Directory.CreateDirectory(directory);
            }

            BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.Android);
        }
    }
}

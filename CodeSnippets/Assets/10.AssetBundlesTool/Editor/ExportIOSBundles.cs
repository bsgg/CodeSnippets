using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

namespace AssetBundleTool
{
    public class ExportIOSBundles : MonoBehaviour
    {
        // .agt Extension  for Android
        // .igt Extension for IOS
        [MenuItem("AssetBundle/Build AssetBundles IOS")]
        static void BuildAllAssetBundles()
        {
            string folder = Path.Combine(Application.dataPath, "AssetBundles");
            string directory = Path.Combine(folder, "IOS");
            if (!Directory.Exists(directory))
            {
                //if it doesn't, create it
                Directory.CreateDirectory(directory);
            }

            BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.iOS);
        }
    }
}

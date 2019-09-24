using System.IO;
using UnityEditor;

namespace Editor {
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            var assetBundleDirectory = "Assets/StreamingAssets";
            if(!Directory.Exists(assetBundleDirectory)){
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        }
    }
}
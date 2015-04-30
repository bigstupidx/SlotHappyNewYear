using UnityEngine;
using UnityEditor;
using System.Linq;


public class ExportSceneAssetBundles : MonoBehaviour
{
    [MenuItem("Build/Build Scenes for iPhone")]
    static void BuildSceneForIPhone()
    {
        ExportScene(BuildTarget.iPhone);
    }

    [MenuItem("Build/Build Scenes for Android")]
    static void BuildSceneForAndroid()
    {
        ExportScene(BuildTarget.Android);
    }

    [MenuItem("Build/Build Scenes for Web")]
    static void BuildSceneForWeb()
    {
        ExportScene(BuildTarget.WebPlayer);
    }

    [MenuItem("Build/Build Scenes for Windows")]
    static void BuildSceneForWindows()
    {
        ExportScene(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Build/Build Scenes for OS X")]
    static void BuildSceneForOSX()
    {
        ExportScene(BuildTarget.StandaloneOSXUniversal);
    }

    static void ExportScene(BuildTarget platform)
    {
        var list =
            from scene in EditorBuildSettings.scenes
            where scene.enabled
            select scene.path;

        string[] levels = list.ToArray();

        if (levels.Length > 0)
        {
            foreach (string s in levels)
                Debug.Log("Export Scene: " + s);

            string path = EditorUtility.SaveFilePanel("Build Bundle", "", "*", "unity3d");
            BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, platform);
        }
        else
            Debug.LogWarning("Here's No Level In Build Settings");
    }

    //在Unity编辑器中添加菜单 for win
    [MenuItem("Build/Create AssetBunldes ALL")]
    static void ExportResource()
    {
        // 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // 选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包  
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iPhone);

        }
    }  
}
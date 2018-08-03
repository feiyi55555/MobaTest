using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Packager
{
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    [MenuItem("Game/Build iPhone Resources", false, 1)]
    public static void BuildiPhoneResources()
    {
        BuildAssetResource(BuildTarget.iOS, false);
    }

    [MenuItem("Game/Build Android Resources", false, 2)]
    public static void BuildAndroidResources()
    {
        BuildAssetResource(BuildTarget.Android, true);
    }

    [MenuItem("Game/Build Windows Resources", false, 3)]
    public static void BuildWindowsResources()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows, true);
    }

    private static void BuildAssetResource(BuildTarget target, bool isWin)
    {
        string dataPath = Util.DataPath;
        if (Directory.Exists(dataPath))
            Directory.Delete(dataPath, true);
        string assetFile = string.Empty;

        string resPath = AppDataPath + "/" + AppConst.AssetDirName + "/";
        if (!Directory.Exists(resPath))
            Directory.CreateDirectory(resPath);

        BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.None, target);

        AssetDatabase.Refresh(); // 刷新
    }

    static string AppDataPath
    {
        get
        {
            return Application.dataPath.ToLower();
        }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    /// <param name="path"></param>
    static void Recusive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach(string fileName in names)
        {
            // 跳过所有的meta文件
            string ext = Path.GetExtension(fileName);
            if (ext.Equals(".meta")) continue;
            files.Add(fileName.Replace('\\', '/'));
        }
        foreach(string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recusive(dir);
        }
    }

    /// <summary>
    /// 更新编辑器的进度条
    /// </summary>
    /// <param name="progress"></param>
    /// <param name="progressMax"></param>
    /// <param name="desc"></param>
    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Processing...[" + progress + "-" + progressMax + "]";
        float val = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, val);
    }
}

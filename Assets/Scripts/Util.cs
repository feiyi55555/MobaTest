using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 通用类
/// </summary>
public class Util
{
    public static int Int(object o)
    {
        return Convert.ToInt32(o);
    }

    public static float Float(object o)
    {
        return (float)Math.Round(Convert.ToSingle(o), 2); // 保留2位小数
    }

    public static long Long(object o)
    {
        return Convert.ToInt64(o);
    }

    public static int Random(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float Random(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// 去除前缀
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static string Uid(string uid)
    {
        int position = uid.LastIndexOf('_');
        return uid.Remove(0, position + 1);
    }

    /// <summary>
    /// 当前的毫秒时间
    /// </summary>
    /// <returns></returns>
    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }
    
    /// <summary>
    /// 获得Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static T Get<T>(GameObject go, string subNode) where T : Component
    {
        if (go != null)
        {
            Transform sub = go.transform.Find(subNode);
            if (sub != null)
            {
                return sub.GetComponent<T>();
            }
        }
        return null;
    }

    /// <summary>
    /// 获得Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static T Get<T>(Component go, string subNode) where T : Component
    {
        return go.transform.Find(subNode).GetComponent<T>();
    }
    
    /// <summary>
    /// 添加Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T Add<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            T[] ts = go.GetComponents<T>();
            for (int i = 0; i < ts.Length; i++)
            {
                if (ts[i] != null)
                {
                    // 移除老组件
                    GameObject.Destroy(ts[i]);
                }
            }
            return go.gameObject.AddComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 添加Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T Add<T>(Transform go) where T : Component
    {
        return Add<T>(go.gameObject);
    }
    
    /// <summary>
    /// 查找子go
    /// </summary>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static GameObject Child(GameObject go, string subNode)
    {
        return Child(go.transform, subNode);
    }

    /// <summary>
    /// 查找子go
    /// </summary>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static GameObject Child(Transform go, string subNode)
    {
        Transform trans = go.Find(subNode);
        if (trans != null)
        {
            return trans.gameObject;
        }
        return null;
    }
    
    /// <summary>
    /// 查找同级go
    /// </summary>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static GameObject Peer(GameObject go, string subNode)
    {
        return Peer(go.transform, subNode);
    }

    /// <summary>
    /// 查找同级go
    /// </summary>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static GameObject Peer(Transform go, string subNode)
    {
        Transform trans = go.parent.Find(subNode);
        if (trans != null)
        {
            return trans.gameObject;
        }
        return null;
    }
    
    /// <summary>
    /// Base64编码
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }

    /// <summary>
    /// 字符串是否是纯数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNumeric(string str)
    {
        if (string.IsNullOrEmpty(str)) return false;
        for (int i = 0; i < str.Length; i++)
        {
            if (!Char.IsNumber(str[i]))
                return false; // 只要是其中有一个不是数字，整体就不是数字了
        }
        return true;
    }

    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] result = md5.ComputeHash(bytes);
            StringBuilder sbr = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sbr.Append(result[i].ToString("x2"));
            }
            return sbr.ToString();
        }
    }

    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        StringBuilder sbr = new StringBuilder();
        for (int i = 0; i < md5Data.Length; i++)
        {
            sbr.Append(Convert.ToString(md5Data[i], 16).PadLeft(1, '0'));
        }
        return sbr.ToString().PadLeft(32, '0');
    }

    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sbr = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sbr.Append(result[i].ToString("x2"));
            }
            return sbr.ToString();
        }
        catch (Exception e)
        {
            //return string.Empty;
            throw new Exception("文件MD5处理失败" + e.Message);
        }
    }

    public static string GetKey(string key)
    {
        return AppConst.AppPrefix + "_" + key;
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(GetKey(key));
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(GetKey(key));
    }

    public static void SetInt(string key, int val)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetInt(name, val);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(GetKey(key));
    }

    public static void SetString(string key, string val)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetString(name, val);
    }

    public static void Remove(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }

    public static void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 是否是数字
    /// </summary>
    /// <param name="strNumber"></param>
    /// <returns></returns>
    public static bool IsNumber(string strNumber)
    {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }

    public static string DataPath
    {
        get
        {
            string game = AppConst.AppName.ToLower();
            if (Application.isMobilePlatform)
                return Application.persistentDataPath + "/" + game + "/"; // 移动平台可读目录
            else
                return Application.dataPath + "/" + AppConst.AssetDirName + "/"; // 桌面平台就是Asset目录
        }
    }

    public static bool NetAvailable
    {
        get { return Application.internetReachability != NetworkReachability.NotReachable; }
    }

    public static bool IsWifi
    {
        get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
    }

    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/" + AppConst.AssetDirName + "/";
                break;
        }
        return path;
    }

    public static void Log(string log) 
    {
        Debug.Log(log);
    }
}

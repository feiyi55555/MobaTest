using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Facade
{
    static GameObject m_GameManager;
    static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

    private static Facade _instance;

    private Facade()
    {
    }

    GameObject AppGameManager
    {
        get
        {
            if (m_GameManager == null)
                m_GameManager = GameObject.Find("GameManager");
            return m_GameManager;
        }
    }

    public static Facade Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Facade();
            return _instance;
        }
    }

    public void AddManager(string typeName, object obj)
    {
        if (!m_Managers.ContainsKey(typeName))
            m_Managers.Add(typeName, obj);
    }

    /// <summary>
    /// 加入管理并且成为一个组件，接受update
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public T AddManager<T>(string typeName) where T : Component
    {
        object result = null;
        m_Managers.TryGetValue(typeName, out result);
        if (result != null)
            return result as T;

        Component c = AppGameManager.AddComponent<T>();
        m_Managers.Add(typeName, c);
        return default(T); //??
    }

    /// <summary>
    /// 获得一个Manager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public T GetManager<T>(string typeName) where T : class
    {
        if (!m_Managers.ContainsKey(typeName))
            return default(T);

        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        return manager as T;
    }

    public void RemoveManager(string typeName)
    {
        if (!m_Managers.ContainsKey(typeName))
            return;
        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        Type type = manager.GetType();
        if (type.IsSubclassOf(typeof (MonoBehaviour)))
        {
            UnityEngine.Object.Destroy(manager as Component);
        }
        m_Managers.Remove(typeName);
    }

    private ResourceManager m_ResMgr;

    public ResourceManager ResMgr
    {
        get
        {
            if (m_ResMgr == null)
            {
                m_ResMgr = GetManager<ResourceManager>("ResourceManager");
            }
            return m_ResMgr;
        }
    }

    private ObjManager m_ObjMgr;

    public ObjManager ObjMgr
    {
        get
        {
            if (m_ObjMgr == null)
            {
                m_ObjMgr = GetManager<ObjManager>("ObjManager");
            }
            return m_ObjMgr;
        }
    }
}

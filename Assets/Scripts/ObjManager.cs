using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjManager : MonoBehaviour {

    Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

    public GameObject GetPanel(string name)
    {
        if (objDic.ContainsKey(name))
            return objDic[name];
        else
            return null;
    }

    public void SetPanel(GameObject obj)
    {
        GameObject canvas = GameObject.Find("Canvas");
        RectTransform rect = obj.GetComponent<RectTransform>();
		rect.SetParent(canvas.transform);
        rect.localScale = Vector3.one;
        rect.localPosition = Vector3.zero;
        rect.anchorMax = Vector2.one;
        rect.anchorMin = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    public GameObject CreateAndLoadObj(string bundleName, string objName = null)
    {
        ResourceManager resMgr = Facade.Instance.GetManager<ResourceManager>("ResourceManager");
        AssetBundle bundle = resMgr.LoadBundle(bundleName);
        if (bundle == null) return null;
        GameObject prefab = null;
        if (objName != null)
            prefab = bundle.LoadAsset(objName, typeof(GameObject)) as GameObject;
        else
            prefab = bundle.mainAsset as GameObject;
        GameObject go = Instantiate(prefab) as GameObject;
        //bundle.Unload(false); // 都要缓存了，你还卸载了？
        if (!abDic.ContainsKey(bundleName))
            abDic.Add(bundleName, bundle);
        if (!objDic.ContainsKey(bundleName))
            objDic.Add(bundleName, go);
        return go;
    }

    public Material CreateAndLoadMat(string bundleName, string objName = null)
    {
        ResourceManager resMgr = Facade.Instance.GetManager<ResourceManager>("ResourceManager");
        AssetBundle bundle = resMgr.LoadBundle(bundleName);
        if (bundle == null) return null;
        Material prefab = null;
        if (objName != null)
            prefab = bundle.LoadAsset(objName, typeof(GameObject)) as Material;
        else
            prefab = bundle.mainAsset as Material;
        Material go = Instantiate(prefab) as Material;
        if (!abDic.ContainsKey(bundleName))
            abDic.Add(bundleName, bundle);
        return go;
    }

    public Sprite CreateAndLoadSprite(string bundleName, string objName = null)
    {
        ResourceManager resMgr = Facade.Instance.GetManager<ResourceManager>("ResourceManager");
        AssetBundle bundle = resMgr.LoadBundle(bundleName);
        if (bundle == null) return null;
        Sprite prefab = null;
        if (objName != null)
            prefab = bundle.LoadAsset(objName, typeof(GameObject)) as Sprite;
        else
            prefab = bundle.mainAsset as Sprite;
        Sprite go = Instantiate(prefab) as Sprite;
        if (!abDic.ContainsKey(bundleName))
            abDic.Add(bundleName, bundle);
        return go;
    }

    public void AddSpriteTexture(GameObject obj, string bundleName, string objName = null)
    {
        Sprite sprite = CreateAndLoadSprite(bundleName, objName);
        obj.GetComponent<Image>().sprite = sprite;
    }

    public void UnLoadAllAB()
    {
        foreach (AssetBundle bundle in abDic.Values)
        {
            if (bundle != null)
                bundle.Unload(false);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
}

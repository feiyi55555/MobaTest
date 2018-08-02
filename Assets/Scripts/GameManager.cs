using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	    Init();

        ObjManager objMgr = Facade.Instance.GetManager<ObjManager>("ObjManager");
		GameObject panel_front = objMgr.CreateAndLoadObj("Panel_Begin", "Panel_Begin");
        objMgr.SetPanel(panel_front);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void Init()
    {
        Facade.Instance.AddManager<ObjManager>("ObjManager");
        Facade.Instance.AddManager<ResourceManager>("ResourceManager");
    }
}

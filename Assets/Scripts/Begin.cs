using UnityEngine;
using System.Collections;

public class Begin : MonoBehaviour
{
	public GameObject Controller;
	public static bool IsColone;
	public TextAsset luaStr;
	// Use this for initialization
	void Start ()
	{
//		LuaScriptMgr mgr = new LuaScriptMgr ();
//		mgr.Start ();
//		mgr.DoString ("print('helo lua')");

//		AndroidJavaClass ac = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject ao = ac.GetStatic<AndroidJavaObject> ("currentActivity");
//		Debug.Log ("ao==============1:" + ao);
//		ao.Call ("SetListener", name, "onBillingFinish");
//		Debug.Log ("ao==============2:" + ao);
//
//		ao.Call ("Order", "", 1);
		AndroidJavaClass ac = new AndroidJavaClass ("com.iap.mm.IAPController");
		ac.CallStatic ("init", "300009184004", "2F3CE83A8367CC82CF5F15D1A9269CEA", name, "onBillingFinish");
	}

	public void hhs ()
	{
		Debug.Log ("klshdadhakjhk=====");
	}
	void OnGUI ()
	{
		if (GUI.Button (new Rect (30, 30, 200, 50), "立即购买"))
		{
			AndroidJavaClass ac = new AndroidJavaClass ("com.iap.mm.IAPController");
			ac.CallStatic ("order", "30000918400401", 1);
		}
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	void onBillingFinish (string resut)
	{
		Debug.Log (resut);
	}
}

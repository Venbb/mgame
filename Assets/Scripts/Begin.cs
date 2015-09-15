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
//		AndroidJavaClass ac = new AndroidJavaClass ("com.iap.cm.IAP_CM");
//		ac.CallStatic ("SetListener", name, "onBillingFinish");
		MM_IAPController.Instance.Init ();
	}

	public void hhs ()
	{
		Debug.Log ("klshdadhakjhk=====");
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (30, 30, 200, 50), "立即购买")) {
			MM_IAPController.Instance.Order ("30000918400401", 1, onback);
//			AndroidJavaClass ac = new AndroidJavaClass ("com.iap.cm.IAP_CM");
//			ac.CallStatic ("order", "30000918400401", 1);
		}
	}

	void OnLoadFinish ()
	{
		Debug.Log ("OnLoadFinish");
	}

	void onback (string result)
	{
		Debug.Log ("onback:" + result);
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

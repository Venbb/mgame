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
//		MM_IAPController.Instance.Init ();
//		Egame_IAPController.Instance.Init ();
//		AndroidJavaClass ac = new AndroidJavaClass ("com.unity.plugins.PathEx");
//		Debug.Log ("hasSDCard:" + ac.CallStatic<bool> ("hasSDCard"));
//		Debug.Log ("isExternalStorageRemovable:" + ac.CallStatic<bool> ("isExternalStorageRemovable"));
//		Debug.Log ("getRootDirectory:" + ac.CallStatic<string> ("getRootDirectory"));
//		Debug.Log ("getFilesDir:" + ac.CallStatic<string> ("getFilesDir"));
//		Debug.Log ("getExternalFilesDir:" + ac.CallStatic<string> ("getExternalFilesDir"));
//		Debug.Log ("getExternalCacheDir:" + ac.CallStatic<string> ("getExternalCacheDir"));
//		Debug.Log ("getCacheDir:" + ac.CallStatic<string> ("getCacheDir"));
//		Debug.Log ("getDataDirectory:" + ac.CallStatic<string> ("getDataDirectory"));
//		Debug.Log ("getDownloadCacheDirectory:" + ac.CallStatic<string> ("getDownloadCacheDirectory"));
//		Debug.Log ("getExternalStorageDirectory:" + ac.CallStatic<string> ("getExternalStorageDirectory"));
////		Debug.Log ("getExternalStoragePublicDirectory:" + ac.CallStatic<string> ("getExternalStoragePublicDirectory"));
//		Debug.Log ("Application.persistentDataPath:" + Application.persistentDataPath);
	}

	public void hhs ()
	{
		Debug.Log ("klshdadhakjhk=====");
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (30, 30, 200, 50), "立即购买"))
		{
//			MM_IAPController.Instance.Order ("30000918400401", 1, onback);
//			AndroidJavaClass ac = new AndroidJavaClass ("com.iap.cm.IAP_CM");
//			ac.CallStatic ("order", "30000918400401", 1);
			Egame_IAPController.Instance.Order ("TOOL1", "一小篮番茄", onback);
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

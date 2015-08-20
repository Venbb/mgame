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

		AndroidJavaClass ac = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ao = ac.GetStatic<AndroidJavaObject> ("currentActivity");
		Debug.Log ("ao==============1:" + ao);
		ao.Call ("SetListener", name, "onBillingFinish");
		Debug.Log ("ao==============2:" + ao);

		ao.Call ("Order", "", 1);
	}

	public void hhs ()
	{
		Debug.Log ("klshdadhakjhk=====");
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

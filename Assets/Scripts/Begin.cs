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
		LuaScriptMgr mgr = new LuaScriptMgr ();
		mgr.Start ();
		mgr.DoString ("print('helo lua')");
	}

	public void hhs ()
	{
		Debug.Log ("klshdadhakjhk=====");
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}

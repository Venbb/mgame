using UnityEngine;
using System.Collections;
using LuaInterface;

public class Begin : MonoBehaviour
{
	public GameObject Controller;
	public static bool IsColone;
	public TextAsset luaStr;
	// Use this for initialization
	void Start ()
	{
//		if (!IsColone) 
//		{
//			GameObject o= Instantiate(Controller) as GameObject;
//			DontDestroyOnLoad(o);
//			IsColone=true;
//		}
//		LuaState ls = new LuaState ();
//		ls.RegisterFunction ("hhs", this, this.GetType ().GetMethod ("hhs"));
//		ls.DoString (luaStr.ToString ());
//
//		Debug.Log ("ls[\"nimei\"]:" + ls ["nimei"]);
//
//		LuaFunction lf = ls.GetFunction ("fun");
//		object[] objs = lf.Call (20);
//		Debug.Log ("objs[0]:" + objs [0]);

//		lf = ls.GetFunction ("hhsa");
//		lf.Call ();
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

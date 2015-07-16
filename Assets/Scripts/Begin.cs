using UnityEngine;
using System.Collections;
using LuaInterface;

public class Begin : MonoBehaviour 
{
	public GameObject Controller;
	public static bool IsColone;
	// Use this for initialization
	void Start () 
	{
//		if (!IsColone) 
//		{
//			GameObject o= Instantiate(Controller) as GameObject;
//			DontDestroyOnLoad(o);
//			IsColone=true;
//		}
		LuaState ls = new LuaState ();
		ls.DoString ("print('Hello World !')");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

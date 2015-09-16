using UnityEngine;
using System.Collections;

public class child : mobj
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (10, 120, 100, 100), "send")) {
			BroadcastMessage ("haha", SendMessageOptions.DontRequireReceiver);
		}
	}

	protected override void methodName ()
	{
		base.methodName ();
		Debug.Log ("hahahahah");
	}

	void OnEnable ()
	{
		Debug.Log ("OnEnable:2");
	}

	void OnDisable ()
	{
		Debug.Log ("OnDisable");
	}

	void OnDestroy ()
	{
		Debug.Log ("OnDestroy");
	}
}

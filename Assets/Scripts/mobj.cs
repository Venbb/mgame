using UnityEngine;
using System.Collections;

public class mobj : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected virtual void methodName ()
	{
		Debug.Log ("virtual void methodName:" + name);
	}

	protected virtual void haha ()
	{
		Debug.Log ("haha:" + name);
	}

	void OnEnable ()
	{
		Debug.Log ("OnEnable:1");
	}
}

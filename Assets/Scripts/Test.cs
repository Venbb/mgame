using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
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
//		if (GUI.Button (new Rect (10, 10, 100, 100), "Create")) {
//			Object obj = ResourceManager.Instance.getObstacle ();
//			Instantiate (obj);
//		}
//		if (GUI.Button (new Rect (10, 120, 100, 100), "changescene")) {
//			Application.LoadLevel ("game");
//		}
		if (GUI.Button (new Rect (10, 10, 100, 100), "Create")) {
			BroadcastMessage ("methodName", SendMessageOptions.DontRequireReceiver);
		}
	}
}

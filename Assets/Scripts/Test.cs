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
	void OnGUI()
	{
		if (GUILayout.Button ("click")) 
		{
			Application.LoadLevel("Loading");
		}
	}
}

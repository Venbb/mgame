using UnityEngine;
using System.Collections;
using MEngine;

public class GameController : MonoBehaviour
{
	static GameController _instance;

	public static GameController Instance
	{
		get
		{
//			if(_instance==null)
//			{
//				_instance=	GameObject.FindObjectOfType<GameController>();
//			}
			return _instance;
		}

	}

	static bool IsLoaded = false;

	void Awake ()
	{
//		if (!IsLoaded) 
//		{
//			IsLoaded=true;
		DontDestroyOnLoad (this);
//		}
		Debuger.Log ("Application.dataPath:" + Application.dataPath);
		Debuger.Log ("Application.persistentDataPath:" + Application.persistentDataPath);
		Debuger.Log ("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
		Debuger.Log ("Application.temporaryCachePath:" + Application.temporaryCachePath);
	}

	// Use this for initialization
	void Start ()
	{

	}

	string curStr = "Main";
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			Application.LoadLevel (curStr);	
			switch (curStr)
			{
			case "Main":
				curStr = "game";
				break;
			case "Loading":
				curStr = "Main";
				break;
			case "game":
				curStr = "Loading";
				break;
			}
//		curStr=curStr.Equals("Main")?"Loading":"Main";
		}
	}
	//	void OnGUI()
	//	{
	//		if (GUI.Button(new Rect(100,0,100,100),"Main"))
	//		{
	//			Application.LoadLevel("Main");
	//		}
	//	}
	public void PrintMsg ()
	{
		Debug.Log (gameObject.name);
	}
}

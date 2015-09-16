//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class ResourceManager : MonoBehaviour
//{
//	//障碍物
//	private Dictionary<string, Object> Obstacle_prefabTable;
//	public static ResourceManager Instance;
//	// Use this for initialization
//	void Start ()
//	{
//		DontDestroyOnLoad (this);
//		Instance = this;
//	}
//
//	Vgame.CallBack onFinish = null;
//
//	public void Load (Vgame.CallBack callback)
//	{
//		onFinish = callback;
//		StartCoroutine (IniobstacleObj ());
//	}
//
//	WWW obstacle_www;
//
//	IEnumerator IniobstacleObj ()
//	{
//		string path = "file://" + Application.streamingAssetsPath + "/iOS/Obstacle.assetbundle";
//		obstacle_www = new WWW (path);
//		yield return obstacle_www;
//		if (obstacle_www.error != null) {
//			Debug.LogError (obstacle_www.error);
//			yield break;
//		}
//		AssetBundle bundle = obstacle_www.assetBundle;
//		Object[] objs = bundle.LoadAll (typeof(Object));
//		foreach (Object item in objs) {
//			if ((item is GameObject) &&
//			    item.name.StartsWith ("Obstacle")) {
//				Obstacle_prefabTable.Add (item.name, item);
//			} 
//		}
//		bundle.Unload (false);
//		if (onFinish != null)
//			onFinish ();
//	}
//
//	public Object getObstacle ()
//	{
//		Object obj = null;
//		Obstacle_prefabTable.TryGetValue ("Obstacle_bitzer_Action1", out obj);
//		return obj;
//	}
//}

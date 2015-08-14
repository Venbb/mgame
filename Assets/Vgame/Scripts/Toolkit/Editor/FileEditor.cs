using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Vgame.ToolKit;

namespace VgameEditor
{
	/// <summary>
	/// 作者:niko
	/// 创建时间:2015/08/13 16:17:30
	/// 描述:
	/// </summary>
	public static class FileEditor
	{
		public static void CreateZipFile ()
		{
			Object[] objs = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);
			var pathes = new List<string> ();
			foreach (Object o in objs)
			{
				string path = AssetDatabase.GetAssetPath (o);
				Debug.Log ("path:" + path);
				List<string> dirs = FileEx.GetDirectories (path);
				dirs.Add (path);
				foreach (string d in dirs)
				{
					List<string> files = FileEx.GetFiles (d, false, ".ds_store", ".meta");
					foreach (string p in files) pathes.Add (p);	
				}
			}
			FileEx.CreateZipFile (pathes, Application.persistentDataPath);
			AssetDatabase.Refresh ();
		}
	}
}
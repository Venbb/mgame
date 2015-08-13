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
			List<string> pathes = new List<string> ();
			foreach (Object o in objs)
			{
				string[] files = Directory.GetFiles (AssetDatabase.GetAssetPath (o));
				foreach (string p in files) pathes.Add (p);
			}
			FileEx.CreateZipFile (pathes, Application.persistentDataPath);
			AssetDatabase.Refresh ();
		}
	}
}
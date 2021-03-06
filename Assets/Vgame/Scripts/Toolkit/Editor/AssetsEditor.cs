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
	public static class AssetsEditor
	{
		/// <summary>
		/// 压缩文件
		/// </summary>
		public static void CreateZipFile ()
		{
			Object[] objs = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);
			var entries = new List<string> ();
			string savePath = "";
			foreach (Object o in objs)
			{
				string path = AssetDatabase.GetAssetPath (o);
				entries.Add (path);
				string zipPath = Path.Combine (Application.persistentDataPath, Path.GetFileNameWithoutExtension (path) + ".zip");
				if (savePath != zipPath)
				{
					if (string.IsNullOrEmpty (savePath))
					{
						savePath = zipPath;
						continue;
					}
					if (zipPath.Length < savePath.Length) savePath = zipPath;
				}
			}
			if (entries.Count == 0)
			{
				EditorUtility.DisplayDialog ("O(∩_∩)O哈哈~", "请选择要压缩的文件", "");
				return;
			}
			FileEx.ZipFile (entries, savePath, "", false, ".DS_Store", ".meta");
			AssetDatabase.Refresh ();
			EditorUtility.RevealInFinder (savePath);
		}
	}
}
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;

namespace Vgame.ToolKit.VgameEditor
{
	public static class VgameEditor
	{
		/// <summary>
		/// 默认文件名
		/// </summary>
		const string DEFAULT_NAME = "NewLuaScript.lua";

		/// <summary>
		/// 创建lua脚本
		/// </summary>
		public static void CreateLuaScript ()
		{
			foreach (Object o in Selection.objects)
			{
				string path = AssetDatabase.GetAssetPath (o);
				path = Path.Combine (path, DEFAULT_NAME);
//				File.Create (path);
				StringBuilder sb = new StringBuilder ();
//				Debug.Log (System.Net.Dns.GetHostName ());
//				Debug.Log (System.Environment.MachineName);
//				Debug.Log (System.Environment.UserDomainName);
				sb.AppendLine ("--===========================");
				sb.AppendLine ("--作者:" + System.Environment.UserName);
				sb.AppendLine ("--创建时间:" + System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss"));
				sb.AppendLine ("--===========================");
				StreamWriter sw = new StreamWriter (path, false, Encoding.Unicode);
				sw.Write (sb);
				sw.Close ();
			}
			AssetDatabase.Refresh ();
		}

	}
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;

namespace VgameEditor
{
	public static class ScriptEditor
	{
		/// <summary>
		/// 默认文件名
		/// </summary>
		const string DEFAULT_FILE_NAME = "NewVgameScript";

		/// <summary>
		/// 创建C#脚本
		/// </summary>
		public static void CreateCSharpScript ()
		{
			Object[] objs = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);
			foreach (Object o in objs)
			{
				string path = AssetDatabase.GetAssetPath (o);
				StringBuilder sb = new StringBuilder ();
				sb.AppendLine ("using UnityEngine;");
				sb.AppendLine ("using Vgame;");
				sb.AppendLine ("");
				sb.AppendLine ("/// <summary>");
				sb.AppendLine ("/// 作者:" + System.Environment.UserName);
				sb.AppendLine ("/// 创建时间:" + System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss"));
				sb.AppendLine ("/// 描述:");
				sb.AppendLine ("/// </summary>");
				sb.AppendLine ("public class " + DEFAULT_FILE_NAME);
				sb.AppendLine ("{");
				sb.Append ("}");

				CreatScript (path, ".cs", sb);
			}
			AssetDatabase.Refresh ();
		}

		/// <summary>
		/// 创建lua脚本
		/// </summary>
		public static void CreateLuaScript ()
		{
			Object[] objs = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);
			foreach (Object o in objs)
			{
				string path = AssetDatabase.GetAssetPath (o);
				//				File.Create (path);
				StringBuilder sb = new StringBuilder ();
				//				Debug.Log (System.Net.Dns.GetHostName ());
				//				Debug.Log (System.Environment.MachineName);
				//				Debug.Log (System.Environment.UserDomainName);
				sb.AppendLine ("-- ===========================");
				sb.AppendLine ("-- 作者:" + System.Environment.UserName);
				sb.AppendLine ("-- 创建时间:" + System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss"));
				sb.AppendLine ("-- 描述:");
				sb.AppendLine ("-- ===========================");

				CreatScript (path, ".lua", sb);
			}
			AssetDatabase.Refresh ();
		}

		/// <summary>
		/// 写入脚本
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="extensionName">Extension name.</param>
		/// <param name="sb">Sb.</param>
		/// <param name="index">Index.</param>
		static void CreatScript (string path, string extensionName, StringBuilder sb, int index = 0)
		{
			string fileName = DEFAULT_FILE_NAME + (index > 0 ? "" + index : "");
			string filePath = Path.Combine (path, fileName + extensionName);
			if (File.Exists (filePath))
			{
				CreatScript (path, extensionName, sb, index + 1);
				return;
			}
			string str = sb.ToString ();
			str = str.Replace (DEFAULT_FILE_NAME, fileName);
			StreamWriter sw = new StreamWriter (filePath, false, Encoding.Unicode);
			sw.Write (str);
			sw.Close ();
		}
	}
}

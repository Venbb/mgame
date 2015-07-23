using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Vgame.ToolKit
{
	public static class FileTools
	{
		/// <summary>
		/// 查找路径
		/// </summary>
		/// <returns>返回包含指定路径片段的所有文件夹</returns>
		/// <param name="pathPart">路径片段</param>
		public static List<string> GetDirectories (string pathPart)
		{
			List<string> pathes = new List<string> ();
			string fileName = pathPart.Contains ("/") ? pathPart.Substring (pathPart.LastIndexOf ("/") + 1) : pathPart;
			string[] files = Directory.GetDirectories (Application.dataPath, fileName, SearchOption.AllDirectories);
			foreach (string file in files)
			{
				if (!file.Contains (pathPart)) continue;
				pathes.Add (file);
			}
			return pathes;
		}

		/// <summary>
		/// 查找文件
		/// </summary>
		/// <returns>返回指定目录片段下的所有文件</returns>
		/// <param name="pathPart">路径片段</param>
		/// <param name = "extents">指定要查找的文件扩展名</param>
		public static List<string> GetFiles (string pathPart, params string[] extents)
		{
			return GetFiles (GetDirectories (pathPart), extents);
		}

		/// <summary>
		/// 查找文件
		/// </summary>
		/// <returns>返回指定目录下的所有文件</returns>
		/// <param name="directories">要查询的目录</param>
		/// <param name="extents">指定要查找的文件扩展名</param>
		public static List<string> GetFiles (List<string> directories, params string[] extents)
		{
			List<string> pathes = new List<string> ();
			foreach (string dir in directories)
			{
				string[] files = Directory.GetFiles (dir);

				foreach (string file in files)
				{
					string regexStr = string.Format (@"{0}", string.Join ("|", extents));
					if (Regex.IsMatch (file, regexStr))
					{
						pathes.Add (file);
					}
				}
			}
			return pathes;
		}
	}
}
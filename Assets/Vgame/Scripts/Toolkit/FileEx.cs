using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using System;

namespace Vgame.ToolKit
{
	public static class FileEx
	{
		/// <summary>
		/// 创建压缩文件
		/// </summary>
		/// <param name="filesPath">Files path.</param>
		/// <param name="zipFilePath">Zip file path.</param>
		/// <param name = "saveName"></param>
		public static void CreateZipFile (string filesPath, string zipFilePath, string saveName = "vgame")
		{
			Debug.Log (filesPath);
			Debug.Log (Path.GetFileNameWithoutExtension (filesPath));
			if (!Directory.Exists (filesPath))
			{
				Debug.LogError (string.Format ("Cannot find directory:'{0}'", filesPath));
				return;
			}
			string[] filenames = Directory.GetFiles (filesPath);
			List<string> sourcePathes = new List<string> ();
			foreach (string path in filenames)
			{
				sourcePathes.Add (path);
			}
			CreateZipFile (sourcePathes, zipFilePath, saveName);
		}

		/// <summary>
		/// 创建压缩文件
		/// </summary>
		/// <param name="sourcePathes">Source pathes.</param>
		/// <param name="zipFilePath">Zip file path.</param>
		/// <param name="saveName">Save name.</param>
		public static void CreateZipFile (List<string> sourcePathes, string zipFilePath, string saveName = "vgame")
		{
			zipFilePath = Path.Combine (zipFilePath, saveName + ".zip");
			if (File.Exists (zipFilePath))
			{
				File.Delete (zipFilePath);
			}
			try
			{
				using (var s = new ZipOutputStream (File.Create (zipFilePath)))
				{
					s.SetLevel (9); // 压缩级别 0-9
					//s.Password = "123"; //Zip压缩文件密码
					var buffer = new byte[4096]; //缓冲区大小
					foreach (string file in sourcePathes)
					{
						var entry = new ZipEntry (Path.GetFileName (file));
						entry.DateTime = DateTime.Now;
						s.PutNextEntry (entry);
						using (FileStream fs = File.OpenRead (file))
						{
							int sourceBytes;
							do
							{
								sourceBytes = fs.Read (buffer, 0, buffer.Length);
								s.Write (buffer, 0, sourceBytes);
							}
							while (sourceBytes > 0);
						}
					}
					s.Finish ();
					s.Close ();
				}
				Debug.Log (string.Format ("Create zip success:'{0}'", zipFilePath));
			}
			catch (Exception ex)
			{
				Debug.LogError (string.Format ("Exception during processing:{0}", ex));
			}
		}

		/// <summary>
		/// 解压文件
		/// </summary>
		/// <param name="zipFilePath">Zip file path.</param>
		/// <param name = "outPutPath"></param>
		/// <param name = "autoDelete"></param>
		public static void UnZipFile (string zipFilePath, string outPutPath = "", bool autoDelete = true)
		{
			if (!File.Exists (zipFilePath))
			{
				Debug.LogError (string.Format ("Cannot find file '{0}'", zipFilePath));
				return;
			}
			//默认解压到当前文件夹
			outPutPath = string.IsNullOrEmpty (outPutPath) ? Path.GetDirectoryName (zipFilePath) : outPutPath;
			if (!Directory.Exists (outPutPath))
			{
				Directory.CreateDirectory (outPutPath);
			}
			using (var s = new ZipInputStream (File.OpenRead (zipFilePath)))
			{
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry ()) != null)
				{
					string fileName = Path.GetFileName (theEntry.Name);
					Debug.Log (fileName);
					if (string.IsNullOrEmpty (fileName)) return;
					using (FileStream streamWriter = File.Create (Path.Combine (outPutPath, fileName)))
					{

						int size = 2048;
						var data = new byte[2048];
						while (true)
						{
							size = s.Read (data, 0, data.Length);
							if (size <= 0) break;
							streamWriter.Write (data, 0, size);
						}
					}
				}
				if (autoDelete) File.Delete (zipFilePath);
				Debug.Log ("UnZipFile Success!");
			}
		}

		/// <summary>
		/// 根据路径片段获取所有子文件夹
		/// </summary>
		/// <returns>返回包含指定路径片段的所有文件夹</returns>
		/// <param name="pathPart">路径片段</param>
		public static List<string> GetDirectoriesInpart (string pathPart)
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
		/// 根据路径片段获取所有子文件
		/// </summary>
		/// <returns>返回指定目录片段下的所有文件</returns>
		/// <param name="pathPart">路径片段</param>
		/// <param name = "extents">指定要查找的文件扩展名</param>
		public static List<string> GetFilesInPart (string pathPart, params string[] extents)
		{
			return GetFiles (GetDirectoriesInpart (pathPart), extents);
		}

		/// <summary>
		/// 获取所有子文件夹
		/// </summary>
		/// <returns>The directories.</returns>
		/// <param name="path">Path.</param>
		public static List<string> GetDirectories (string path)
		{
			List<string> directories = new List<string> ();
			string[] pathes = Directory.GetDirectories (path);
			foreach (string str in pathes) directories.Add (str);
			return directories;
		}

		/// <summary>
		/// 查找文件
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="path">Path.</param>
		/// <param name = "isEndExtent"></param>
		/// <param name="extents">Extents.</param>
		public static List<string> GetFiles (string path, bool isEndExtent, params string[] extents)
		{
			List<string> pathes = new List<string> ();
			if (!Directory.Exists (path))
			{
				Debug.Log (string.Format ("Cannot find directory '{0}'", path));
				return pathes;
			}
			string[] files = Directory.GetFiles (path);
			foreach (string file in files)
			{
				string regexStr = string.Format (@"^.*.(?i)({0})$", string.Join ("|", extents));
				if (Regex.IsMatch (file, regexStr) == isEndExtent)
				{
					pathes.Add (file);
				}
			}
			return pathes;
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
					string regexStr = string.Format (@"^.*.(?i)({0})$", string.Join ("|", extents));
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
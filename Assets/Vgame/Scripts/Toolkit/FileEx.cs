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
		/// 流文件保存路径
		/// </summary>
		/// <value>The streaming assets path.</value>
		public static string StreamingAssetsPath
		{
			get
			{
				string path = Application.streamingAssetsPath;
				#if UNITY_IPHONE
				path = Path.Combine (path, "iOS");
				#elif UNITY_ANDROID
				path = Path.Combine (path, "Android");
				#endif
				return path;
			}
		}
		static string _persistentDataPath;
		/// <summary>
		/// Gets the persistent data path.
		/// </summary>
		/// <value>The persistent data path.</value>
		public static string persistentDataPath
		{
			get
			{
				if (string.IsNullOrEmpty (_persistentDataPath))
					_persistentDataPath = Application.persistentDataPath;
				#if UNITY_EDITOR
				#elif UNITY_ANDROID
				if (string.IsNullOrEmpty (_persistentDataPath))
				{
					_persistentDataPath = AndroidPathEx.ExternalFilesDir;
					if (string.IsNullOrEmpty (_persistentDataPath))_persistentDataPath = AndroidPathEx.FilesDir;
				}
				#endif
				return _persistentDataPath;
			}
		}
		/// <summary>
		/// 压缩文件
		/// </summary>
		/// <param name="strFile">压缩源路径</param>
		/// <param name="strZip">压缩文件保存路径</param>
		/// <param name = "rootDir">压缩文件根目录</param>
		/// <param name = "isEndExtent">是否压缩包含扩展名的文件</param>
		/// <param name = "extents">用于筛选的扩展名</param>
		public static void ZipFile (string strFile, string strZip, string rootDir = "", bool isEndExtent = false, params string[] extents)
		{
			var s = new ZipOutputStream (File.Create (strZip));
			s.SetLevel (9); // 0 - store only to 9 - means best compression
			zip (strFile, s, rootDir, isEndExtent, extents);
			s.Finish ();
			s.Close ();
		}

		/// <summary>
		/// 压缩文件
		/// </summary>
		/// <param name="files">压缩源</param>
		/// <param name="strZip">压缩文件保存路径</param>
		/// <param name = "rootDir">压缩文件根目录</param>
		/// <param name = "isEndExtent">是否压缩包含扩展名的文件</param>
		/// <param name = "extents">用于筛选的扩展名</param>
		public static void ZipFile (List<string> files, string strZip, string rootDir = "", bool isEndExtent = false, params string[] extents)
		{
			var s = new ZipOutputStream (File.Create (strZip));
			s.SetLevel (9); // 0 - store only to 9 - means best compression
			foreach (string str in files)
				zip (str, s, rootDir, isEndExtent, extents);
			s.Finish ();
			s.Close ();
		}

		static void zip (string strFile, ZipOutputStream s, string rootDir = "", bool isEndExtent = false, params string[] extents)
		{
			List<string> filenames = GetFileSystemEntries (strFile, isEndExtent, extents);
			foreach (string file in filenames)
			{
				if (!Directory.Exists (file))
				{
					FileStream fs = File.OpenRead (file);
					var buffer = new byte[fs.Length];
					fs.Read (buffer, 0, buffer.Length);
					string tempfile = Path.Combine (rootDir, Path.GetFileName (file));
					var entry = new ZipEntry (tempfile);

					entry.DateTime = DateTime.Now;
					entry.Size = fs.Length;
					fs.Close ();
					s.PutNextEntry (entry);

					s.Write (buffer, 0, buffer.Length);

				}
				else
					zip (file, s, Path.Combine (rootDir, Path.GetFileName (file)), isEndExtent, extents);
			}
		}

		/// <summary>
		/// 解压文件
		/// </summary>
		/// <param name="zipFilePath">要解压的文件路径</param>
		/// <param name = "outPutPath">默认解压到当前文件夹</param>
		/// <param name = "autoDelete">是否自动删除压缩文件</param>
		/// <param name = "isSameDir">是否解压到同一目录</param>
		/// <param name = "isCreate">是否新建一个同名文件夹</param>
		/// <param name = "isAppend">是否以追加方式解压</param>
		public static void UnZipFile (string zipFilePath, string outPutPath = "", bool autoDelete = true, bool isSameDir = false, bool isCreate = true, bool isAppend = false)
		{
			if (!File.Exists (zipFilePath))
			{
				Debug.LogError (string.Format ("Cannot find file '{0}'", zipFilePath));
				return;
			}
			//默认解压到当前文件夹
			outPutPath = string.IsNullOrEmpty (outPutPath) ? Path.GetDirectoryName (zipFilePath) : outPutPath;
			if (isCreate)
				outPutPath = Path.Combine (outPutPath, Path.GetFileNameWithoutExtension (zipFilePath));
			if (!isAppend)
			{
				DeleteFiles (outPutPath);
			}
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
					if (string.IsNullOrEmpty (fileName))
						return;
					string outPath = isSameDir ? outPutPath : Path.Combine (outPutPath, Path.GetDirectoryName (theEntry.Name));
					if (!Directory.Exists (outPath))
					{
						Directory.CreateDirectory (outPath);
					}
					using (FileStream streamWriter = File.Create (Path.Combine (outPath, fileName)))
					{

						int size = 2048;
						var data = new byte[2048];
						while (true)
						{
							size = s.Read (data, 0, data.Length);
							if (size <= 0)
								break;
							streamWriter.Write (data, 0, size);
						}
					}
				}
				if (autoDelete)
					File.Delete (zipFilePath);
				Debug.Log ("UnZipFile Success!");
			}
		}

		/// <summary>
		///  清除本地存储文件
		/// </summary>
		public static void ClearPersistentData ()
		{
			DeleteFiles (persistentDataPath, false);
		}

		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name = "delSelf">是否删除根目录</param>
		public static void DeleteFiles (string path, bool delSelf = true)
		{
			if (!delSelf)
			{
				string[] entries = Directory.GetFileSystemEntries (path);
				foreach (string p in entries)
					DeleteFiles (p, true);
				return;
			}
			string[] pathes = Directory.GetFiles (path);
			foreach (string p in pathes)
			{
				if (File.Exists (p))
					File.Delete (p);
			}
			if (!Directory.Exists (path))
				return;
			pathes = Directory.GetDirectories (path);
			foreach (string p in pathes)
			{
				DeleteFiles (p);
			}
			pathes = Directory.GetDirectories (path);
			if (pathes.Length == 0)
			{
				Directory.Delete (path);
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
				if (!file.Contains (pathPart))
					continue;
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
			foreach (string str in pathes)
				directories.Add (str);
			return directories;
		}

		/// <summary>
		/// 获取指定路径下得所有文件及文件夹
		/// </summary>
		/// <returns>The file system entries.</returns>
		/// <param name="path">Path.</param>
		/// <param name="isEndExtent">If set to <c>true</c> is end extent.</param>
		/// <param name="extents">Extents.</param>
		public static List<string> GetFileSystemEntries (string path, bool isEndExtent, params string[] extents)
		{
			List<string> entries = new List<string> ();
			string[] files = Directory.GetFileSystemEntries (path);
			foreach (string file in files)
			{
				string regexStr = string.Format (@"^.*.(?i)({0})$", string.Join ("|", extents));
				if (Regex.IsMatch (file, regexStr) == isEndExtent)
				{
					entries.Add (file);
				}
			}
			return entries;
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
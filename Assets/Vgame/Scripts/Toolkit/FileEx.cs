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
		public static void CreateZipFile (string filesPath, string zipFilePath)
		{
			if (!Directory.Exists (filesPath))
			{
				Debug.Log (string.Format ("Cannot find directory '{0}'", filesPath));
				return;
			}
			try
			{
				string[] filenames = Directory.GetFiles (filesPath);
				using (var s = new ZipOutputStream (File.Create (zipFilePath)))
				{

					s.SetLevel (9); // 压缩级别 0-9
					//s.Password = "123"; //Zip压缩文件密码
					var buffer = new byte[4096]; //缓冲区大小
					foreach (string file in filenames)
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
			}
			catch (Exception ex)
			{
				Debug.Log (string.Format ("Exception during processing {0}", ex));
			}
		}

		/// <summary>
		/// 解压文件
		/// </summary>
		/// <param name="zipFilePath">Zip file path.</param>
		public static void UnZipFile (string zipFilePath)
		{
			if (!File.Exists (zipFilePath))
			{
				Debug.Log (string.Format ("Cannot find file '{0}'", zipFilePath));
				return;
			}
			using (var s = new ZipInputStream (File.OpenRead (zipFilePath)))
			{
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry ()) != null)
				{
					Console.WriteLine (theEntry.Name);

					string directoryName = Path.GetDirectoryName (theEntry.Name);
					string fileName = Path.GetFileName (theEntry.Name);

					// create directory
					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory (directoryName);
					}

					if (string.IsNullOrEmpty (fileName)) return;
					using (FileStream streamWriter = File.Create (theEntry.Name))
					{

						int size = 2048;
						var data = new byte[2048];
						while (true)
						{
							size = s.Read (data, 0, data.Length);
							if (size > 0)
							{
								streamWriter.Write (data, 0, size);
							}
							else
							{
								break;
							}
						}
					}
				}
			}
		}

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
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Vgame.ToolKit;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System;

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
			UnityEngine.Object[] objs = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.Assets);
			var pathes = new List<string> ();
			foreach (UnityEngine.Object o in objs)
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

		public static void ZipFile (string strFile, string strZip)
		{
			if (strFile [strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
			ZipOutputStream s = new ZipOutputStream (File.Create (strZip));
			s.SetLevel (9); // 0 - store only to 9 - means best compression
			zip (strFile, s, Application.persistentDataPath);
			s.Finish ();
			s.Close ();
		}


		private static void zip (string strFile, ZipOutputStream s, string staticFile)
		{
			if (strFile [strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
			Crc32 crc = new Crc32 ();
			string[] filenames = Directory.GetFileSystemEntries (strFile);
			foreach (string file in filenames)
			{
				if (Directory.Exists (file))
				{
					zip (file, s, staticFile);
				}
				else // 否则直接压缩文件
				{
					//打开压缩文件
					FileStream fs = File.OpenRead (file);

					byte[] buffer = new byte[fs.Length];
					fs.Read (buffer, 0, buffer.Length);
					string tempfile = file.Substring (staticFile.LastIndexOf ("\\") + 1);
					ZipEntry entry = new ZipEntry (tempfile);

					entry.DateTime = DateTime.Now;
					entry.Size = fs.Length;
					fs.Close ();
					crc.Reset ();
					crc.Update (buffer);
					entry.Crc = crc.Value;
					s.PutNextEntry (entry);

					s.Write (buffer, 0, buffer.Length);
				}
			}
		}
	}
}
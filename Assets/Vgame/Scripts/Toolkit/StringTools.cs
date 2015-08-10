using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace Vgame.ToolKit
{
	public static class StringTools
	{
		/// <summary>
		/// 首写字母大写
		/// </summary>
		/// <param name="str">String.</param>
		public static void ToUpperFirstChar (ref string str)
		{
			if (string.IsNullOrEmpty (str)) return;
			char c1 = char.ToUpper (str [0]);
			if (str.Length > 1)
			{
				str = c1 + str.Substring (1);
			}
		}

		/// <summary>
		/// 首字母小写
		/// </summary>
		/// <param name="str">String.</param>
		public static void ToLowerFirstChar (ref string str)
		{
			if (string.IsNullOrEmpty (str)) return;
			char c1 = char.ToLower (str [0]);
			if (str.Length > 1)
			{
				str = c1 + str.Substring (1);
			}
		}

		/// <summary>
		/// 压缩字节数组
		/// </summary>
		/// <param name="inputBytes">Input bytes.</param>
		public static byte[] Compress (byte[] inputBytes)
		{
			var outStream = new MemoryStream ();
			var zipStream = new GZipOutputStream (outStream);
			zipStream.Write (inputBytes, 0, inputBytes.Length);
			zipStream.Close ();	
			return outStream.ToArray ();	
		}

		/// <summary>
		/// 解压字节数组
		/// </summary>
		/// <param name="inputBytes">Input bytes.</param>
		public static byte[] Decompress (byte[] inputBytes)
		{
			var inputStream = new MemoryStream (inputBytes);
			var zipStream = new GZipInputStream (inputStream);
			var outStream = new MemoryStream ();
			int size = 2048;
			var outBytes = new byte[size];
			while (size > 0)
			{
				size = zipStream.Read (outBytes, 0, size);
				if (size > 0) outStream.Write (outBytes, 0, size);
			}
			zipStream.Close ();
			return outStream.ToArray ();
		}
	}
}


//using UnityEngine;
//using System;
//using System.IO;
//using System.Text;
//using System.Security.Cryptography;
//using System.Text.RegularExpressions;
//using ICSharpCode.SharpZipLib.GZip;
//using ICSharpCode.SharpZipLib.Zip;
//using LitJson;
//using Junfine.Debuger;
//
//public class Util : MonoBehaviour
//{
//	public static int Int (object o)
//	{
//		return Convert.ToInt32 (o);
//	}
//
//	public static float Float (object o)
//	{
//		return (float)Math.Round (Convert.ToSingle (o), 2);
//	}
//
//	public static long Long (object o)
//	{
//		return Convert.ToInt64 (o);
//	}
//
//	public static int Random (int min, int max)
//	{
//		return UnityEngine.Random.Range (min, max);
//	}
//
//	public static float Random (float min, float max)
//	{
//		return UnityEngine.Random.Range (min, max);
//	}
//
//	public static string Uid (string uid)
//	{
//		int position = uid.LastIndexOf ('_');
//		return uid.Remove (0, position + 1);
//	}
//
//	public static long GetTime ()
//	{
//		TimeSpan ts = new TimeSpan (DateTime.UtcNow.Ticks - new DateTime (1970, 1, 1, 0, 0, 0).Ticks);
//		return (long)ts.TotalMilliseconds;
//	}
//
//	/// <summary>
//	/// ËÑË÷×ÓÎïÌå×é¼þ-GameObject°æ
//	/// </summary>
//	public static T Get<T> (GameObject go, string subnode) where T : Component
//	{
//		if (go != null)
//		{
//			Transform sub = go.transform.FindChild (subnode);
//			if (sub != null) return sub.GetComponent<T> ();
//		}
//		return null;
//	}
//
//	/// <summary>
//	/// ËÑË÷×ÓÎïÌå×é¼þ-Transform°æ
//	/// </summary>
//	public static T Get<T> (Transform go, string subnode) where T : Component
//	{
//		if (go != null)
//		{
//			Transform sub = go.FindChild (subnode);
//			if (sub != null) return sub.GetComponent<T> ();
//		}
//		return null;
//	}
//
//	/// <summary>
//	/// ËÑË÷×ÓÎïÌå×é¼þ-Component°æ
//	/// </summary>
//	public static T Get<T> (Component go, string subnode) where T : Component
//	{
//		return go.transform.FindChild (subnode).GetComponent<T> ();
//	}
//
//	/// <summary>
//	/// Ìí¼Ó×é¼þ
//	/// </summary>
//	public static T Add<T> (GameObject go) where T : Component
//	{
//		if (go != null)
//		{
//			T[] ts = go.GetComponents<T> ();
//			for (int i = 0; i < ts.Length; i++)
//			{
//				if (ts [i] != null) Destroy (ts [i]);
//			}
//			return go.gameObject.AddComponent<T> ();
//		}
//		return null;
//	}
//
//	/// <summary>
//	/// Ìí¼Ó×é¼þ
//	/// </summary>
//	public static T Add<T> (Transform go) where T : Component
//	{
//		return Add<T> (go.gameObject);
//	}
//
//	/// <summary>
//	/// ²éÕÒ×Ó¶ÔÏó
//	/// </summary>
//	public static GameObject Child (GameObject go, string subnode)
//	{
//		return Child (go.transform, subnode);
//	}
//
//	/// <summary>
//	/// ²éÕÒ×Ó¶ÔÏó
//	/// </summary>
//	public static GameObject Child (Transform go, string subnode)
//	{
//		Transform tran = go.FindChild (subnode);
//		if (tran == null) return null;
//		return tran.gameObject;
//	}
//
//	/// <summary>
//	/// È¡Æ½¼¶¶ÔÏó
//	/// </summary>
//	public static GameObject Peer (GameObject go, string subnode)
//	{
//		return Peer (go.transform, subnode);
//	}
//
//	/// <summary>
//	/// È¡Æ½¼¶¶ÔÏó
//	/// </summary>
//	public static GameObject Peer (Transform go, string subnode)
//	{
//		Transform tran = go.parent.FindChild (subnode);
//		return tran == null ? null : tran.gameObject;
//	}
//
//	/// <summary>
//	/// ÊÖ»úÕð¶¯
//	/// </summary>
//	public static void Vibrate ()
//	{
//		//int canVibrate = PlayerPrefs.GetInt(Const.AppPrefix + "Vibrate", 1);
//		//if (canVibrate == 1) iPhoneUtils.Vibrate();
//	}
//
//	/// <summary>
//	/// Base64±àÂë
//	/// </summary>
//	public static string Encode (string message)
//	{
//		byte[] bytes = Encoding.GetEncoding ("utf-8").GetBytes (message);
//		return Convert.ToBase64String (bytes);
//	}
//
//	/// <summary>
//	/// Base64½âÂë
//	/// </summary>
//	public static string Decode (string message)
//	{
//		byte[] bytes = Convert.FromBase64String (message);
//		return Encoding.GetEncoding ("utf-8").GetString (bytes);
//	}
//
//	/// <summary>
//	/// ÅÐ¶ÏÊý×Ö
//	/// </summary>
//	public static bool IsNumeric (string str)
//	{
//		if (string.IsNullOrEmpty (str)) return false;
//		for (int i = 0; i < str.Length; i++)
//		{
//			if (!Char.IsNumber (str [i]))
//			{
//				return false;
//			}
//		}
//		return true;
//	}
//
//	/// <summary>
//	/// HashToMD5Hex
//	/// </summary>
//	public static string HashToMD5Hex (string sourceStr)
//	{
//		byte[] Bytes = Encoding.UTF8.GetBytes (sourceStr);
//		using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ())
//		{
//			byte[] result = md5.ComputeHash (Bytes);
//			StringBuilder builder = new StringBuilder ();
//			for (int i = 0; i < result.Length; i++) builder.Append (result [i].ToString ("x2"));
//			return builder.ToString ();
//		}
//	}
//
//	/// <summary>
//	/// ¼ÆËã×Ö·û´®µÄMD5Öµ
//	/// </summary>
//	public static string md5 (string source)
//	{
//		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
//		byte[] data = System.Text.Encoding.UTF8.GetBytes (source);
//		byte[] md5Data = md5.ComputeHash (data, 0, data.Length);
//		md5.Clear ();
//
//		string destString = "";
//		for (int i = 0; i < md5Data.Length; i++)
//		{
//			destString += System.Convert.ToString (md5Data [i], 16).PadLeft (2, '0');
//		}
//		destString = destString.PadLeft (32, '0');
//		return destString;
//	}
//
//	/// <summary>
//	/// ¼ÆËãÎÄ¼þµÄMD5Öµ
//	/// </summary>
//	public static string md5file (string file)
//	{
//		try
//		{
//			FileStream fs = new FileStream (file, FileMode.Open);
//			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
//			byte[] retVal = md5.ComputeHash (fs);
//			fs.Close ();
//
//			StringBuilder sb = new StringBuilder ();
//			for (int i = 0; i < retVal.Length; i++)
//			{
//				sb.Append (retVal [i].ToString ("x2"));
//			}
//			return sb.ToString ();
//		}
//		catch (Exception ex)
//		{
//			throw new Exception ("md5file() fail, error:" + ex.Message);
//		}
//	}
//
//	/// <summary>
//	/// ¹¦ÄÜ£ºÑ¹Ëõ×Ö·û´®
//	/// </summary>
//	/// <param name="infile">±»Ñ¹ËõµÄÎÄ¼þÂ·¾¶</param>
//	/// <param name="outfile">Éú³ÉÑ¹ËõÎÄ¼þµÄÂ·¾¶</param>
//	public static void CompressFile (string infile, string outfile)
//	{
//		Stream gs = new GZipOutputStream (File.Create (outfile));
//		FileStream fs = File.OpenRead (infile);
//		byte[] writeData = new byte[fs.Length];
//		fs.Read (writeData, 0, (int)fs.Length);
//		gs.Write (writeData, 0, writeData.Length);
//		gs.Close ();
//		fs.Close ();
//	}
//
//	/// <summary>
//	/// ¹¦ÄÜ£ºÊäÈëÎÄ¼þÂ·¾¶£¬·µ»Ø½âÑ¹ºóµÄ×Ö·û´®
//	/// </summary>
//	public static string DecompressFile (string infile)
//	{
//		string result = string.Empty;
//		Stream gs = new GZipInputStream (File.OpenRead (infile));
//		MemoryStream ms = new MemoryStream ();
//		int size = 2048;
//		byte[] writeData = new byte[size];
//		while (true)
//		{
//			size = gs.Read (writeData, 0, size);
//			if (size > 0)
//			{
//				ms.Write (writeData, 0, size);
//			}
//			else
//			{
//				break;
//			}
//		}
//		result = new UTF8Encoding ().GetString (ms.ToArray ());
//		gs.Close ();
//		ms.Close ();
//		return result;
//	}
//
//	/// <summary>
//	/// Ñ¹Ëõ×Ö·û´®
//	/// </summary>
//	public static string Compress (string source)
//	{
//		byte[] data = Encoding.UTF8.GetBytes (source);
//		MemoryStream ms = null;
//		using (ms = new MemoryStream ())
//		{
//			using (Stream stream = new GZipOutputStream (ms))
//			{
//				try
//				{
//					stream.Write (data, 0, data.Length);
//				}
//				finally
//				{
//					stream.Close ();
//					ms.Close ();
//				}
//			}
//		}
//		return Convert.ToBase64String (ms.ToArray ());
//	}
//
//	/// <summary>
//	/// ½âÑ¹×Ö·û´®
//	/// </summary>
//	public static string Decompress (string source)
//	{
//		string result = string.Empty;
//		byte[] buffer = null;
//		try
//		{
//			buffer = Convert.FromBase64String (source);
//		}
//		catch
//		{
//			Debug.LogError ("Decompress---->>>>" + source);
//		}
//		using (MemoryStream ms = new MemoryStream (buffer))
//		{
//			using (Stream sm = new GZipInputStream (ms))
//			{
//				StreamReader reader = new StreamReader (sm, Encoding.UTF8);
//				try
//				{
//					result = reader.ReadToEnd ();
//				}
//				finally
//				{
//					sm.Close ();
//					ms.Close ();
//				}
//			}
//		}
//		return result;
//	}
//
//	/// <summary>
//	/// Çå³ýËùÓÐ×Ó½Úµã
//	/// </summary>
//	public static void ClearChild (Transform go)
//	{
//		if (go == null) return;
//		for (int i = go.childCount - 1; i >= 0; i--)
//		{
//			Destroy (go.GetChild (i).gameObject);
//		}
//	}
//
//	/// <summary>
//	/// Éú³ÉÒ»¸öKeyÃû
//	/// </summary>
//	public static string GetKey (string key)
//	{
//		return Const.AppPrefix + Const.UserId + "_" + key;
//	}
//
//	/// <summary>
//	/// È¡µÃÕûÐÍ
//	/// </summary>
//	public static int GetInt (string key)
//	{
//		string name = GetKey (key);
//		return PlayerPrefs.GetInt (name);
//	}
//
//	/// <summary>
//	/// ÓÐÃ»ÓÐÖµ
//	/// </summary>
//	public static bool HasKey (string key)
//	{
//		string name = GetKey (key);
//		return PlayerPrefs.HasKey (name);
//	}
//
//	/// <summary>
//	/// ±£´æÕûÐÍ
//	/// </summary>
//	public static void SetInt (string key, int value)
//	{
//		string name = GetKey (key);
//		PlayerPrefs.DeleteKey (name);
//		PlayerPrefs.SetInt (name, value);
//	}
//
//	/// <summary>
//	/// È¡µÃÊý¾Ý
//	/// </summary>
//	public static string GetString (string key)
//	{
//		string name = GetKey (key);
//		return PlayerPrefs.GetString (name);
//	}
//
//	/// <summary>
//	/// ±£´æÊý¾Ý
//	/// </summary>
//	public static void SetString (string key, string value)
//	{
//		string name = GetKey (key);
//		PlayerPrefs.DeleteKey (name);
//		PlayerPrefs.SetString (name, value);
//	}
//
//	/// <summary>
//	/// É¾³ýÊý¾Ý
//	/// </summary>
//	public static void RemoveData (string key)
//	{
//		string name = GetKey (key);
//		PlayerPrefs.DeleteKey (name);
//	}
//
//	/// <summary>
//	/// ÇåÀíÄÚ´æ
//	/// </summary>
//	public static void ClearMemory ()
//	{
//		GC.Collect ();
//		Resources.UnloadUnusedAssets ();
//	}
//
//	/// <summary>
//	/// ÊÇ·ñÎªÊý×Ö
//	/// </summary>
//	public static bool IsNumber (string strNumber)
//	{
//		Regex regex = new Regex ("[^0-9]");
//		return !regex.IsMatch (strNumber);
//	}
//
//	/// <summary>
//	/// È¡µÃApp°üÀïÃæµÄ¶ÁÈ¡Ä¿Â¼
//	/// </summary>
//	public static Uri AppContentDataUri
//	{
//		get
//		{
//			string dataPath = Application.dataPath;
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				var uriBuilder = new UriBuilder ();
//				uriBuilder.Scheme = "file";
//				uriBuilder.Path = Path.Combine (dataPath, "Raw");
//				return uriBuilder.Uri;
//			}
//			else if (Application.platform == RuntimePlatform.Android)
//			{
//				return new Uri ("jar:file://" + dataPath + "!/assets");
//			}
//			else
//			{
//				var uriBuilder = new UriBuilder ();
//				uriBuilder.Scheme = "file";
//				uriBuilder.Path = Path.Combine (dataPath, "StreamingAssets");
//				return uriBuilder.Uri;
//			}
//		}
//	}
//
//	/// <summary>
//	/// È¡µÃÊý¾Ý´æ·ÅÄ¿Â¼
//	/// </summary>
//	public static string DataPath
//	{
//		get
//		{
//			string game = Const.AppName.ToLower ();
//			string dataPath = Application.dataPath;
//			if (Const.DebugMode) AppContentDataUri.ToString ();
//
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				string path = Path.GetDirectoryName (Path.GetDirectoryName (dataPath));
//				return Path.Combine (path, "Documents/" + game + "/");
//			}
//			else if (Application.platform == RuntimePlatform.Android)
//			{
//				return "/sdcard/" + game + "/";
//			}
//			return dataPath + "/StreamingAssets/";
//		}
//	}
//
//	/// <summary>
//	/// È¡µÃÐÐÎÄ±¾
//	/// </summary>
//	public static string GetFileText (string path)
//	{
//		return File.ReadAllText (path);
//	}
//
//	/// <summary>
//	/// ÍøÂç¿ÉÓÃ
//	/// </summary>
//	public static bool NetAvailable
//	{
//		get
//		{
//			return  Application.internetReachability != NetworkReachability.NotReachable;
//			//return false;
//		}
//	}
//
//	/// <summary>
//	/// ÊÇ·ñÊÇÎÞÏß
//	/// </summary>
//	public static bool IsWifi
//	{
//		get
//		{
//			return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
//		}
//	}
//
//	/// <summary>
//	/// È¡µÃÊý¾Ý´æ·ÅÄ¿Â¼
//	/// </summary>
//	public static string GetDataDir ()
//	{
//		string dataPath = Application.dataPath;
//		if (Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			string path = Path.GetDirectoryName (Path.GetDirectoryName (dataPath));
//			return Path.Combine (path, "Documents/testgame/");
//		}
//		else if (Application.platform == RuntimePlatform.Android)
//		{
//			return "/sdcard/testgame/";
//		}
//		if (Const.DebugMode) return dataPath;
//		return "c:/";
//	}
//
//	/// <summary>
//	/// Ó¦ÓÃ³ÌÐòÄÚÈÝÂ·¾¶
//	/// </summary>
//	public static string AppContentPath ()
//	{
//
//		#if UNITY_EDITOR
//		#if UNITY_ANDROID
//		return "file://" + Application.streamingAssetsPath + "/Android/";
//		#elif  UNITY_IPHONE
//		return "file://" + Application.streamingAssetsPath + "/iPhone/";
//		#endif
//		return "file://" + Application.streamingAssetsPath + "/Android/";
//
//		#elif UNITY_IPHONE
//		string url=Application.streamingAssetsPath + "/iPhone/";
//		if (url.IndexOf("://")==-1)
//		{
//		url="file://"+url;
//		}
//		return url;
//		#elif UNITY_ANDROID
//		string url= Application.streamingAssetsPath + "/Android/";
//		if (url.IndexOf("://")==-1)
//		{
//		url="file://"+url;
//		}
//		return url;
//		#endif
//		}
//
//		/// <summary>
//		/// ÊÇ·ñÊÇµÇÂ¼³¡¾°
//		/// </summary>
//		public static bool isLogin
//		{
//		get { return Application.loadedLevelName.CompareTo ("login") == 0; }
//		}
//
//		/// <summary>
//		/// ÊÇ·ñÊÇ³ÇÕò³¡¾°
//		/// </summary>
//		public static bool isMain
//		{
//		get { return Application.loadedLevelName.CompareTo ("main") == 0; }
//		}
//
//		/// <summary>
//		/// ÅÐ¶ÏÊÇ·ñÊÇÕ½¶·³¡¾°
//		/// </summary>
//		public static bool isFight
//		{
//		get { return Application.loadedLevelName.CompareTo ("fight") == 0; }
//		}
//
//		public static void Log (string str)
//		{
//		Debug.Log (str);
//		}
//
//		public static void LogWarning (string str)
//		{
//		Debug.LogWarning (str);
//		}
//
//		public static void LogError (string str)
//		{
//		Debug.LogError (str);
//		}
//
//		#region ¼ÓÑ¹·½·¨
//
//		/// <summary>
//		/// ¹¦ÄÜ£ºÑ¹ËõÎÄ¼þ£¨ÔÝÊ±Ö»Ñ¹ËõÎÄ¼þ¼ÐÏÂÒ»¼¶Ä¿Â¼ÖÐµÄÎÄ¼þ£¬ÎÄ¼þ¼Ð¼°Æä×Ó¼¶±»ºöÂÔ£©
//		/// </summary>
//		/// <param name="dirPath">±»Ñ¹ËõµÄÎÄ¼þ¼Ð¼ÐÂ·¾¶</param>
//		/// <param name="zipFilePath">Éú³ÉÑ¹ËõÎÄ¼þµÄÂ·¾¶£¬Îª¿ÕÔòÄ¬ÈÏÓë±»Ñ¹ËõÎÄ¼þ¼ÐÍ¬Ò»¼¶Ä¿Â¼£¬Ãû³ÆÎª£ºÎÄ¼þ¼ÐÃû+.zip</param>
//		/// <param name="err">³ö´íÐÅÏ¢</param>
//		/// <returns>ÊÇ·ñÑ¹Ëõ³É¹¦</returns>
//		public static bool ZipFile (string dirPath, string zipFilePath, out string err)
//		{
//		err = "";
//		if (dirPath == string.Empty)
//		{
//		err = "ÒªÑ¹ËõµÄÎÄ¼þ¼Ð²»ÄÜÎª¿Õ£¡";
//		return false;
//		}
//		if (!Directory.Exists (dirPath))
//		{
//		err = "ÒªÑ¹ËõµÄÎÄ¼þ¼Ð²»´æÔÚ£¡";
//		return false;
//		}
//		//Ñ¹ËõÎÄ¼þÃûÎª¿ÕÊ±Ê¹ÓÃÎÄ¼þ¼ÐÃû£«.zip
//		if (zipFilePath == string.Empty)
//		{
//		if (dirPath.EndsWith ("//"))
//		{
//		dirPath = dirPath.Substring (0, dirPath.Length - 1);
//		}
//		zipFilePath = dirPath + ".zip";
//		}
//
//		try
//		{
//		string[] filenames = Directory.GetFiles (dirPath);
//		using (ZipOutputStream s = new ZipOutputStream (File.Create (zipFilePath)))
//		{
//		s.SetLevel (9);
//		byte[] buffer = new byte[4096];
//		foreach (string file in filenames)
//		{
//		if (file.EndsWith (".assetbundle"))
//		{
//		ZipEntry entry = new ZipEntry (Path.GetFileName (file));
//		entry.DateTime = DateTime.Now;
//		s.PutNextEntry (entry);
//		using (FileStream fs = File.OpenRead (file))
//		{
//		int sourceBytes;
//		do
//		{
//		sourceBytes = fs.Read (buffer, 0, buffer.Length);
//		s.Write (buffer, 0, sourceBytes);
//		}
//		while (sourceBytes > 0);
//		}
//		}
//
//		}
//		s.Finish ();
//		s.Close ();
//		}
//		}
//		catch (Exception ex)
//		{
//		err = ex.Message;
//		return false;
//		}
//		return true;
//		}
//
//		#endregion
//
//		#region ½âÑ¹
//
//		/// <summary>
//		/// ¹¦ÄÜ£º½âÑ¹zip¸ñÊ½µÄÎÄ¼þ¡£
//		/// </summary>
//		/// <param name="zipFilePath">Ñ¹ËõÎÄ¼þÂ·¾¶</param>
//		/// <param name="unZipDir">½âÑ¹ÎÄ¼þ´æ·ÅÂ·¾¶,Îª¿ÕÊ±Ä¬ÈÏÓëÑ¹ËõÎÄ¼þÍ¬Ò»¼¶Ä¿Â¼ÏÂ£¬¸úÑ¹ËõÎÄ¼þÍ¬ÃûµÄÎÄ¼þ¼Ð</param>
//		/// <param name="err">³ö´íÐÅÏ¢</param>
//		/// <returns>½âÑ¹ÊÇ·ñ³É¹¦</returns>
//		public static bool UnZipFile (string zipFilePath, string unZipDir, out string err)
//		{
//		err = "";
//		if (zipFilePath == string.Empty)
//		{
//		err = "Ñ¹ËõÎÄ¼þ²»ÄÜÎª¿Õ£¡";
//		return false;
//		}
//		if (!File.Exists (zipFilePath))
//		{
//		err = "Ñ¹ËõÎÄ¼þ²»´æÔÚ£¡";
//		return false;
//		}
//		//½âÑ¹ÎÄ¼þ¼ÐÎª¿ÕÊ±Ä¬ÈÏÓëÑ¹ËõÎÄ¼þÍ¬Ò»¼¶Ä¿Â¼ÏÂ£¬¸úÑ¹ËõÎÄ¼þÍ¬ÃûµÄÎÄ¼þ¼Ð
//		if (unZipDir == string.Empty) unZipDir = zipFilePath.Replace (Path.GetFileName (zipFilePath), Path.GetFileNameWithoutExtension (zipFilePath));
//		if (!unZipDir.EndsWith ("//")) unZipDir += "//";
//		if (!Directory.Exists (unZipDir)) Directory.CreateDirectory (unZipDir);
//
//		try
//		{
//		using (ZipInputStream s = new ZipInputStream (File.OpenRead (zipFilePath)))
//		{
//
//		ZipEntry theEntry;
//		while ((theEntry = s.GetNextEntry ()) != null)
//		{
//		string directoryName = Path.GetDirectoryName (theEntry.Name);
//		string fileName = Path.GetFileName (theEntry.Name);
//		if (directoryName.Length > 0)
//		{
//		Directory.CreateDirectory (unZipDir + directoryName);
//		}
//		if (!directoryName.EndsWith ("//")) directoryName += "//";
//		if (fileName != String.Empty)
//		{
//		using (FileStream streamWriter = File.Create (unZipDir + theEntry.Name))
//		{
//
//		int size = 2048;
//		byte[] data = new byte[2048];
//		while (true)
//		{
//		size = s.Read (data, 0, data.Length);
//		if (size > 0)
//		{
//		streamWriter.Write (data, 0, size);
//		}
//		else
//		{
//		break;
//		}
//		}
//		}
//		}
//		}//while
//		}
//		}
//		catch (Exception ex)
//		{
//		err = ex.Message;
//		return false;
//		}
//		return true;
//		}
//		//½âÑ¹½áÊø
//
//		#endregion
//
//		/// <summary>
//		/// Éè±¸id
//		/// </summary>
//		/// <returns></returns>
//		public static string DeviceID
//		{
//		get
//		{
//		return SystemInfo.deviceUniqueIdentifier;
//		}
//
//		}
//
//		/// <summary>
//		///  È¡µÃÆ½Ì¨²ÎÊý ¸üÐÂ×ÊÔ´ÓÃ
//		/// </summary>
//		public static string getTarget
//		{
//		get
//		{
//		string result = "";
//		switch (Application.platform)
//		{
//		case RuntimePlatform.Android:
//		result = "android";
//		break;
//		case RuntimePlatform.IPhonePlayer:
//		result = "ios";
//		break;
//		default:
//		result = "";
//		break;
//		}
//		return result;
//		}
//		}
//
//		public static string GetKey (string json, long TimeUnix)
//		{
//		json = json.Replace ("0.0", "0");
//		//²»ÄÜÐÞ¸Ä
//		string Key = "$2y$14$YAEaF1Jk3ij1GcwhOS77ZuwHPibKN5Yar07cc8bND0ncg27Fi.Az2";
//
//		string cl = Player.ID + json + Key + TimeUnix.ToString ();
//		Debuger.Log ("PreMD5:" + cl);
//		string result = "";
//		#region ÔÚpcÃ»ÎÊÌâ ÔÚAndroid»á±¨´í
//		//MD5 md5 = MD5.Create();
//		#endregion
//		MD5 md5 = new MD5CryptoServiceProvider ();
//
//		// ¼ÓÃÜºóÊÇÒ»¸ö×Ö½ÚÀàÐÍµÄÊý×é£¬ÕâÀïÒª×¢Òâ±àÂëUTF8/UnicodeµÈµÄÑ¡Ôñ¡¡
//		byte[] s = md5.ComputeHash (Encoding.UTF8.GetBytes (cl));
//
//		// Í¨¹ýÊ¹ÓÃÑ­»·£¬½«×Ö½ÚÀàÐÍµÄÊý×é×ª»»Îª×Ö·û´®£¬´Ë×Ö·û´®ÊÇ³£¹æ×Ö·û¸ñÊ½»¯ËùµÃ
//		for (int i = 0; i < s.Length; i++)
//		{
//		// ½«µÃµ½µÄ×Ö·û´®Ê¹ÓÃÊ®Áù½øÖÆÀàÐÍ¸ñÊ½¡£¸ñÊ½ºóµÄ×Ö·ûÊÇÐ¡Ð´µÄ×ÖÄ¸£¬Èç¹ûÊ¹ÓÃ´óÐ´£¨X£©Ôò¸ñÊ½ºóµÄ×Ö·ûÊÇ´óÐ´×Ö·û,Èç¹ûÊ¹ÓÃÐ¡Ð´£¨x£©Ôò¸ñÊ½ºóµÄ×Ö·ûÊÇ´óÐ´×Ö·û
//		result = result + s [i].ToString ("x").PadLeft (2, '0');
//		}
//		Debuger.Log ("MD5_Result:" + result);
//		return result;
//		}
//
//		public static string getAssetbundleUrl (string bundle)
//		{
//		if (File.Exists (Application.persistentDataPath + "/" + bundle))
//		{
//		return "file:///" + Application.persistentDataPath + "/" + bundle;
//		}
//		else
//		{
//		string url = Util.AppContentPath () + bundle;
//		return url;
//		}
//
//		}
//
//		public static string getFriendDataUrl ()
//		{
//		return Application.persistentDataPath + "/firendData.txt";
//		}
//
//		public static string getAllrankDataUrl ()
//		{
//		return Application.persistentDataPath + "/AllrankData.txt";
//		}
//
//		public static string getWeekrankDataUrl ()
//		{
//		return Application.persistentDataPath + "/WeekrankData.txt";
//		}
//
//		public static string getActrankDataUrl ()
//		{
//		return Application.persistentDataPath + "/ActrankData.txt";
//		}
//
//		public static string getEmailDataUrl ()
//		{
//		return Application.persistentDataPath + "/emailData.txt";
//		}
//
//		public static int getResversion ()
//		{
//		int version = 0;
//		switch (getTarget)
//		{
//		case "android":
//		version = PrefabManager.Android_resversion;
//		break;
//		case "ios":
//		version = PrefabManager.IOS_resversion;
//		break;
//		default:
//		version = PrefabManager.Android_resversion;
//		break;
//		}
//		return version;
//		}
//
//		public static int getSoftwareversion ()
//		{
//		int version = 0;
//		switch (getTarget)
//		{
//		case "android":
//		version = PrefabManager.Android_softwareversion;
//		break;
//		case "ios":
//		version = PrefabManager.IOS_softwareversion;
//		break;
//		default:
//		version = PrefabManager.Android_softwareversion;
//		break;
//		}
//		return version;
//		}
//
//		public static void ClearMailData ()
//		{
//		string path = Util.getEmailDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		}
//
//		#region ¸üÐÂ±¾µØºÃÓÑÐÅÏ¢
//
//		public static void UpdataFriendData (rqback_FriendList_result[] result)
//		{
//		if (result == null) return;
//		string friendData = JsonMapper.ToJson (result);
//		friendData = Util.Compress (friendData);
//		string path = Util.getFriendDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		FileStream fs = new FileStream (path, FileMode.Create, FileAccess.Write);//´´½¨Ð´ÈëÎÄ¼þ
//		StreamWriter sr = new StreamWriter (fs);
//		sr.Write (friendData);
//		sr.Close ();
//		fs.Close ();
//		}
//
//		#endregion
//
//		#region »ñÈ¡±¾µØºÃÓÑÐÅÏ¢
//
//		public static rqback_FriendList_result[] GetFriendData ()
//		{
//		string path = Util.getFriendDataUrl ();
//		if (!File.Exists (path)) return null;
//		string friendData = File.ReadAllText (path);
//		friendData = Util.Decompress (friendData);
//		return JsonMapper.ToObject<rqback_FriendList_result[]> (friendData);
//		}
//
//		public static void ClearFriendData ()
//		{
//		string path = Util.getFriendDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		}
//
//		#endregion
//
//		#region ¸üÐÂ±¾µØ×ÜÅÅÐÐÐÅÏ¢
//
//		public static void UpdataAllrankData (rqback_Rank_result data)
//		{
//		if (data == null) return;
//		string d = JsonMapper.ToJson (data);
//		d = Util.Compress (d);
//		string path = Util.getAllrankDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		FileStream fs = new FileStream (path, FileMode.Create, FileAccess.Write);//´´½¨Ð´ÈëÎÄ¼þ
//		StreamWriter sr = new StreamWriter (fs);
//		sr.Write (d);
//		sr.Close ();
//		fs.Close ();
//
//		}
//
//		#endregion
//
//		#region ¸üÐÂ±¾µØÖÜÅÅÐÐÐÅÏ¢
//
//		public static void UpdataWeekrankData (rqback_Rank_result data)
//		{
//		if (data == null) return;
//		string d = JsonMapper.ToJson (data);
//		d = Util.Compress (d);
//		string path = Util.getWeekrankDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		FileStream fs = new FileStream (path, FileMode.Create, FileAccess.Write);//´´½¨Ð´ÈëÎÄ¼þ
//		StreamWriter sr = new StreamWriter (fs);
//		sr.Write (d);
//		sr.Close ();
//		fs.Close ();
//
//		}
//
//		#endregion
//
//		#region ¸üÐÂ±¾µØ»î¶¯ÅÅÐÐÐÅÏ¢
//
//		public static void UpdataActrankData (rqback_Rank_result data)
//		{
//		if (data == null) return;
//		string d = JsonMapper.ToJson (data);
//		d = Util.Compress (d);
//		string path = Util.getActrankDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		FileStream fs = new FileStream (path, FileMode.Create, FileAccess.Write);//´´½¨Ð´ÈëÎÄ¼þ
//		StreamWriter sr = new StreamWriter (fs);
//		sr.Write (d);
//		sr.Close ();
//		fs.Close ();
//
//		}
//
//		#endregion
//
//		#region »ñÈ¡±¾µØ×ÜÅÅÐÐÐÅÏ¢
//
//		public static rqback_Rank_result GetAllrankData ()
//		{
//		string path = Util.getAllrankDataUrl ();
//		if (!File.Exists (path)) return null;
//		string Data = File.ReadAllText (path);
//		Data = Util.Decompress (Data);
//		return JsonMapper.ToObject<rqback_Rank_result> (Data);
//
//		}
//
//		#endregion
//
//		#region »ñÈ¡±¾µØÖÜÅÅÐÐÐÅÏ¢
//
//		public static rqback_Rank_result GetWeekrankData ()
//		{
//		string path = Util.getWeekrankDataUrl ();
//		if (!File.Exists (path)) return null;
//		string Data = File.ReadAllText (path);
//		Data = Util.Decompress (Data);
//		return JsonMapper.ToObject<rqback_Rank_result> (Data);
//		}
//
//		#endregion
//
//		#region »ñÈ¡±¾µØ»î¶¯ÅÅÐÐÐÅÏ¢
//
//		public static rqback_Rank_result GetActrankData ()
//		{
//		string path = Util.getActrankDataUrl ();
//		if (!File.Exists (path)) return null;
//		string Data = File.ReadAllText (path);
//		Data = Util.Decompress (Data);
//		return JsonMapper.ToObject<rqback_Rank_result> (Data);
//		}
//
//		#endregion
//
//		public static int StringLength (string str)
//		{
//		if (string.IsNullOrEmpty (str)) return 0;
//		return Encoding.Default.GetBytes (str).Length;
//		}
//
//		/// <summary>
//		/// »ñÈ¡Ö¸¶¨×Ö·û³¤¶ÈµÄ×Ö´®
//		/// </summary>
//		/// <returns>The string value for length.</returns>
//		/// <param name="str">Ô´×Ö·û´®</param>
//		/// <param name="vLength">×Ö½Ú³¤¶È</param>
//		public static string GetStringValueForLength (string str, int vLength)
//		{
//		string tmpResult = string.Empty;
//		if (string.IsNullOrEmpty (str)) return tmpResult;
//
//		for (int i = 0; i < str.Length; i++)
//		{
//		tmpResult += str [i];
//		if (StringLength (tmpResult) > vLength)
//		{
//		tmpResult = tmpResult.Substring (0, i);
//		break;
//		}
//		}
//
//		return tmpResult;
//		}
//
//		//½¨Á¢Ò»¸öÎÄ±¾ÎÄ¼þ//
//		public static void WriteToFile (string vFileName, string vContent)
//		{
//		StreamWriter SW;
//		SW = File.CreateText (vFileName);
//		SW.WriteLine (vContent);
//		SW.Close ();
//		}
//
//		//¶ÁÎÄ¼þ//
//		public static string ReadFromFile (string vFileName)
//		{
//		string S = string.Empty;
//		if (File.Exists (vFileName))
//		{
//		StreamReader SR;
//		SR = File.OpenText (vFileName);
//		S = SR.ReadLine ();
//		SR.Close ();
//		}
//		return S;
//		}
//
//		//Ìí¼Ó//
//		public static void AppendToFile (string vFileName, string vContent)
//		{
//		StreamWriter SW;
//		SW = File.AppendText (vFileName);
//		SW.WriteLine (vContent);
//		SW.Close ();
//		}
//
//		/// <summary>
//		/// Gets the width of the string.
//		/// »ñÈ¡×Ö·û´®´óÐ¡(µ¥ÐÐ)
//		/// </summary>
//		/// <returns>The string width.</returns>
//		/// <param name="vLabel">²Ù×÷µÄ¿Ø¼þ</param>
//		/// <param name="vString">Ô´×Ö·û´®</param>
//		public static float GetStringWidth (UILabel vLabel, string vString)
//		{
//		if (vLabel == null) return 0;
//		return vLabel.GetTextWidth (vString);
//		}
//
//		/// <summary>
//		/// Gets the width of the string value for.
//		/// ½ØÈ¡Ö¸¶¨ÏñËØ¿í¶ÈµÄ×Ö·û´®
//		/// </summary>
//		/// <returns>The string value for width.</returns>
//		/// <param name="vLabel">²Ù×÷µÄ¿Ø¼þ</param>
//		/// <param name="vString">Ô´×Ö·û´®</param>
//		/// <param name="vWidth">½ØÈ¡³¤¶È</param>
//		public static string GetStringValueForWidth (UILabel vLabel, string vString, float vWidth)
//		{
//		string tmpResult = string.Empty;
//
//		if ((vLabel == null) || (string.IsNullOrEmpty (vString))) return tmpResult;
//
//		for (int i = 0; i < vString.Length; i++)
//		{
//		tmpResult += vString [i];
//		if (GetStringWidth (vLabel, tmpResult) > vWidth)
//		{
//		tmpResult = tmpResult.Substring (0, i);
//		break;
//		}
//		}
//
//		return tmpResult;
//		}
//
//		/// <summary>
//		/// Gets the width of the string value for.
//		/// ¶Ô±È²¢½ØÈ¡Ö¸¶¨ÏñØ¿í¶ÈµÄ×Ö·??	/// </summary>
//		/// <returns>The string value for width.</returns>
//		/// <param name="vLabel">²Ù×÷µÄ¿Ø¼þ</param>
//		/// <param name="vString">Ô´×Ö·û´®</param>
//		/// <param name="vWidth">½ØÈ¡³¤¶È</param>
//		/// <param name="vLimit">ÏÞ¶¨³¤¶È</param>
//		public static string GetStringValueForWidth (UILabel vLabel, string vString, float vWidth, float vLimit)
//		{
//		if ((vLabel == null) || (string.IsNullOrEmpty (vString))) return string.Empty;
//
//		if (GetStringWidth (vLabel, vString) > vLimit) return GetStringValueForWidth (vLabel, vString, vWidth);
//
//		return vString;
//		}
//
//		/// <summary>
//		/// ÅÐ¶Ï¸Ã¹Ø¿¨ÊÇ·ñ½âËø
//		/// </summary>
//		/// <param name="id"></param>
//		/// <returns></returns>
//		public static bool IsClockCheckPoint (int id)
//		{
//		bool res = false;
//		string[] arry = Player.havedCheckPoints.Split (',');
//		foreach (string item in arry)
//		{
//		if (item == id.ToString ())
//		{
//		res = true;
//		break;
//		}
//		}
//		return res;
//		}
//
//		public static Vector3 getVector3ByValue (string value)
//		{
//		string[] vec = value.Split (',');
//		float x = float.Parse (vec [0]);
//		float y = float.Parse (vec [1]);
//		float z = float.Parse (vec [2]);
//		return new Vector3 (x, y, z);
//		}
//
//
//		/// <summary>
//		/// Ë¢ÐÂshader
//		/// </summary>
//		/// <param name="assetBundle"></param>
//		public static void RefreshShader (AssetBundle assetBundle)
//		{
//		UnityEngine.Object[] materials = assetBundle.LoadAll (typeof(Material));
//		foreach (UnityEngine.Object m in materials)
//		{
//		Material mat = m as Material;
//
//		string shaderName = mat.shader.name;
//		Shader newShader = Shader.Find (shaderName);
//		if (newShader != null)
//		{
//		mat.shader = newShader;
//		}
//		else
//		{
//		Debug.LogWarning ("unable to refresh shader: " + shaderName + " in material " + m.name);
//		}
//		}
//		}
//
//		/// <summary>
//		///
//		/// </summary>
//		/// <param name="www"></param>
//		public static void RefreshShader (WWW www)
//		{
//		if (www.assetBundle != null) RefreshShader (www.assetBundle);
//		}
//
//		/// <summary>
//		/// ¸ù¾ÝÂ·¾¶»ñÈ¡ ´øºó×ºµÄÎÄ¼þÃû
//		/// </summary>
//		/// <param name="fullPath"></param>
//		/// <returns></returns>
//		public static string getFileNameAndTypeByPath (string fullPath)
//		{
//		string filename = System.IO.Path.GetFileName (fullPath);//ÎÄ¼þÃû  ¡°Default.aspx¡±
//		return filename;
//		}
//
//
//		/// <summary>
//		/// ¸ù¾ÝÂ·¾¶»ñÈ¡ ºó×º
//		/// </summary>
//		/// <param name="fullPath"></param>
//		/// <returns></returns>
//		public static string getFileTypeByPath (string fullPath)
//		{
//		string extension = System.IO.Path.GetExtension (fullPath);//À©Õ¹Ãû ¡°.aspx¡±
//		return extension;
//		}
//
//
//		/// <summary>
//		/// ¸ù¾ÝÂ·¾¶»ñÈ¡ Ã»ºó×ºµÄÎÄ¼þÃû
//		/// </summary>
//		/// <param name="fullPath"></param>
//		/// <returns></returns>
//		public static string getFileNameNoTypeByPath (string fullPath)
//		{
//		string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension (fullPath);// Ã»ÓÐÀ©Õ¹ÃûµÄÎÄ¼þÃû ¡°Default¡±
//		return fileNameWithoutExtension;
//		}
//
//		/// <summary>
//		/// ÊÇ·ñÊÇÐÂÊÖÒýµ¼
//		/// </summary>
//		public static bool isNew
//		{
//		get
//		{
//		return Player.isNew < 8;
//		}
//		}
//
//		public static string BUNDLE = "cn.s7k7k.sheep";
//
//		/// <summary>
//		/// Î¢ÐÅÍ·ÏñµØÖ·
//		/// </summary>
//		public static string weixin_headimg
//		{
//		get
//		{
//		return Application.persistentDataPath + "/weixin_player.png";
//		}
//		}
//
//		/// <summary>
//		/// Çå³ýËùÓÐÅÅÐÐÊý¾Ý
//		/// </summary>
//		public static void ClearAllrankData ()
//		{
//		string path = Util.getAllrankDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		}
//
//		/// <summary>
//		/// Çå³ýÖÜÅÅÐÐÊý¾Ý
//		/// </summary>
//		public static void ClearWeekrankData ()
//		{
//		string path = Util.getWeekrankDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		}
//
//		/// <summary>
//		/// Çå³ý»î¶¯ÅÅÐÐÊý¾Ý
//		/// </summary>
//		public static void ClearActrankData ()
//		{
//		string path = Util.getActrankDataUrl ();
//		if (File.Exists (path))
//		{
//		File.Delete (path);
//		}
//		}
//
//		public static int getGuideValue(GuideType type)
//		{
//		if (type.Equals(GuideType.ReliveUI))
//		{
//		return Player.New_reliveUI;
//		}
//		else if (type.Equals(GuideType.FightUI))
//		{
//		return Player.New_fightUI;
//		}
//		else if (type.Equals(GuideType.DressUI))
//		{
//		return Player.New_dressUI;
//		}
//		else
//		{
//		return 0;
//		}
//		}
//
//		public static void setGuideValue(GuideType type)
//		{
//		if (type.Equals(GuideType.ReliveUI))
//		{
//		Player.New_reliveUI+=1;
//		}
//		else if (type.Equals(GuideType.FightUI))
//		{
//		Player.New_fightUI+=1;
//		}
//		else if (type.Equals(GuideType.DressUI))
//		{
//		Player.New_dressUI+=1;
//		}
//		}
//
//		public static void IniNew()
//		{
//		if(Player.isNew<8)
//		{
//		Player.isNew = 0;
//		}
//		if(Player.New_dressUI>0 && Player.New_dressUI<4)
//		{
//		Player.New_dressUI = 4;
//		}
//		if(Player.New_fightUI>0 && Player.New_fightUI<2)
//		{
//		Player.New_fightUI = 2;
//		}
//		if(Player.New_reliveUI>0 && Player.New_reliveUI<2)
//		{
//		Player.New_reliveUI = 2;
//		}
//		}
//		}
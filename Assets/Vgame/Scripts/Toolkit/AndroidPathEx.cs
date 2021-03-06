using UnityEngine;
using Vgame;

namespace Vgame.ToolKit
{
/// <summary>
/// 作者:niko
/// 创建时间:2015/09/22 15:50:45
/// 描述:安卓目录,依赖main.jar包
/// </summary>
#if UNITY_ANDROID
	public static class AndroidPathEx
	{
		const string PAKAGE_PATH = "com.unity.plugins.PathEx";
		/// <summary>
		/// 设备外存是否存在并且可读写
		/// </summary>
		/// <value><c>true</c> if has SD card; otherwise, <c>false</c>.</value>
		public static bool HasSDCard
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<bool> ("hasSDCard") : false;
			}
		}
		/// <summary>
		/// 设备的外存是否是可以拆卸的，比如SD卡，是则返回true。(API Level 9)
		/// </summary>
		/// <value><c>true</c> if is external storage removable; otherwise, <c>false</c>.</value>
		public static bool IsExternalStorageRemovable
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<bool> ("isExternalStorageRemovable") : false;
			}
		}
		/// <summary>
		/// 应用在内存上的目录
		/// </summary>
		/// <value>The files dir.</value>
		public static string FilesDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getFilesDir") : "";
			}
		}
		/// <summary>
		/// 应用在内存上的缓存目录
		/// </summary>
		/// <value>The cache dir.</value>
		public static string CacheDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getCacheDir") : "";
			}
		}
		/// <summary>
		/// Android的根目录
		/// </summary>
		/// <value>The files dir.</value>
		public static string RootDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getRootDirectory") : "";
			}
		}
		/// <summary>
		/// 应用在外存上的目录，这个等于Application.persistentDataPath
		/// </summary>
		/// <value>The external files dir.</value>
		public static string ExternalFilesDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getExternalFilesDir") : "";
			}
		}
		/// <summary>
		/// 获取应用在外存上的缓存目录
		/// </summary>
		/// <value>The external cache dir.</value>
		public static string ExternalCacheDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getExternalCacheDir") : "";
			}
		}
		/// <summary>
		/// 用户数据目录
		/// </summary>
		/// <value>The external cache dir.</value>
		public static string DataDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getDataDirectory") : "";
			}
		}
		/// <summary>
		/// 下载缓存内容目录
		/// </summary>
		/// <value>The download cache dir.</value>
		public static string DownloadCacheDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getDownloadCacheDirectory") : "";
			}
		}
		/// <summary>
		/// 主要的外部存储目录
		/// </summary>
		/// <value>The external storage dir.</value>
		public static string ExternalStorageDir
		{
			get
			{
				AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
				return ac != null ? ac.CallStatic<string> ("getExternalStorageDirectory") : "";
			}
		}
	}
#endif
}
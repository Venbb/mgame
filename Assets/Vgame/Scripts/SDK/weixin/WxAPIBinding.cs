using UnityEngine;
using Vgame;

/// <summary>
/// 作者:niko
/// 创建时间:2015/09/28 17:07:24
/// 描述:
/// </summary>
#if UNITY_ANDROID
public static class WxAPIBinding
{
	const string PAKAGE_PATH = "cn.s7k7k.sheep.WXAPIController";
	
	/// <summary>
	///	注册微信
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="callback">Callback.</param>
	public static void RegToWx (string appid, string target, string callback)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("regToWx", appid, target, callback);
	}
	
	/// <summary>
	/// 是否安装微信客户端
	/// </summary>
	/// <param name="paycode">Paycode.</param>
	/// <param name="paynum">Paynum.</param>
	public static bool IsWXAppInstalled (string paycode, int paynum)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		return ac.CallStatic<bool> ("isWXAppInstalled");
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="paycode">Paycode.</param>
	/// <param name="paynum">Paynum.</param>
	public static bool IsWXAppSupportAPI (string paycode, int paynum)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		return ac.CallStatic<bool> ("isWXAppSupportAPI");
	}
	/// <summary>
	/// 微信登录
	/// </summary>
	public static void Login ()
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("login");
	}
	/// <summary>
	/// 微信分享
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="desc">Desc.</param>
	/// <param name="iconPath">Icon path.</param>
	/// <param name="url">URL.</param>
	public static void Share (string title, string desc, string iconPath, string url)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("Share", title, desc, iconPath, url);
	}
	/// <summary>
	/// 设置监听
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="callback">Callback.</param>
	public static void SetListener (string target, string callback)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("SetListener", target, callback);
	}
}
#endif
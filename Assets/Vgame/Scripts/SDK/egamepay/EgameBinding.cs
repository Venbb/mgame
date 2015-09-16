using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
public class EgameBinding
{
	const string PAKAGE_PATH = "com.iap.egame.Egame_IAPController";
	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="callback">Callback.</param>
	public static void Init (string target, string callback)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("Init", target, callback);
	}
	/// <summary>
	/// 发起支付
	/// </summary>
	/// <param name="paycode">Paycode.</param>
	/// <param name="nameStr">nameStr.</param>
	public static void Order (string paycode, string nameStr)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("Order", paycode, nameStr);
	}
	/// <summary>
	/// 暂停
	/// </summary>
	public static void OnPause ()
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("OnPause");
	}
	/// <summary>
	/// 恢复
	/// </summary>
	public static void OnResume ()
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("OnResume");
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

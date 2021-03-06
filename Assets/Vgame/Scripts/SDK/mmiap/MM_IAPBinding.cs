using UnityEngine;
using Vgame;

/// <summary>
/// 作者:niko
/// 创建时间:2015/08/27 15:46:33
/// 描述:
/// </summary>
#if UNITY_ANDROID
public class MM_IAPBinding
{
	const string PAKAGE_PATH = "com.iap.cm.CM_IAPController";

	/// <summary>
	/// 是否已经完成初始化
	/// </summary>
	/// <value><c>true</c> if is init; otherwise, <c>false</c>.</value>
	public static bool IsInit
	{
		get
		{
			AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
			return ac.GetStatic<bool> ("isInit");
		}
	}

	/// <summary>
	///	初始化
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="callback">Callback.</param>
	public static void Init (string target, string callback)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("SetListener", target, callback);
		ac.CallStatic ("Init");
	}

	/// <summary>
	/// 发起支付
	/// </summary>
	/// <param name="paycode">Paycode.</param>
	/// <param name="paynum">Paynum.</param>
	public static void Order (string paycode, int paynum)
	{
		AndroidJavaClass ac = new AndroidJavaClass (PAKAGE_PATH);
		ac.CallStatic ("Order", paycode, paynum);
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
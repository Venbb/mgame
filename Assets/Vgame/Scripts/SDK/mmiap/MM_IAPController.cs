using UnityEngine;
using Vgame;

/// <summary>
/// 作者:niko
/// 创建时间:2015/08/27 15:14:30
/// 描述:移动内购支付
/// </summary>
public class MM_IAPController : MonoBehaviour
{
	public static MM_IAPController Instance;

	public MM_IAPController ()
	{
		Instance = this;
	}
	#if UNITY_ANDROID
	CallBackWithParams<string> onCallBack;
	string mPayCode = "";
	int mPayNum = 1;

	public bool IsInit
	{
		get
		{
			return MM_IAPBinding.IsInit;
		}
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public void Init ()
	{
		MM_IAPBinding.Init (name, "OnInit");
	}

	/// <summary>
	/// 初始化返回
	/// </summary>
	/// <param name="result">Result.</param>
	void OnInit (string result)
	{
		Debug.Log ("OnInit:" + result);
	}

	/// <summary>
	/// 发起支付
	/// </summary>
	/// <param name="paycode">Paycode.</param>
	/// <param name="paynum">Paynum.</param>
	/// <param name="ckFun">Ck fun.</param>
	public void Order (string paycode, int paynum, CallBackWithParams<string> ckFun)
	{
		onCallBack = ckFun;
		if (!IsInit)
			return;
		MM_IAPBinding.SetListener (name, "OnBillingFinish");
		MM_IAPBinding.Order (paycode, paynum);
	}

	void OnBillingFinish (string result)
	{
		Debug.Log ("OnBillingFinish:" + result);
		if (onCallBack != null)
			onCallBack (result);
		onCallBack = null;
	}
	#endif
}
using UnityEngine;
using Vgame;

/// <summary>
/// 作者:niko
/// 创建时间:2015/09/16 13:59:40
/// 描述:
/// </summary>
public class Egame_IAPController : MonoBehaviour
{
	public static Egame_IAPController Instance;
	
	public Egame_IAPController ()
	{
		Instance = this;
	}
#if UNITY_ANDROID
	CallBackWithParams<string> onCallBack;
	/// <summary>
	/// 初始化
	/// </summary>
	public void Init ()
	{
		EgameBinding.Init (name, "OnBillingFinish");
	}
	/// <summary>
	/// 发起支付
	/// </summary>
	/// <param name="paycode">Paycode.</param>
	/// <param name="paynum">Paynum.</param>
	/// <param name="ckFun">Ck fun.</param>
	public void Order (string paycode, string nameStr, CallBackWithParams<string> ckFun)
	{
		onCallBack = ckFun;

		EgameBinding.Order (paycode, nameStr);
	}
	
	void OnBillingFinish (string result)
	{
		Debug.Log ("OnBillingFinish:" + result);
		if (onCallBack != null)
			onCallBack (result);
		onCallBack = null;
	}
	void OnApplicationPause (bool paused)
	{
		if (paused)
			EgameBinding.OnPause ();
		else
			EgameBinding.OnResume ();
	}
	#endif
}
using UnityEngine;
using Vgame;

/// <summary>
/// 作者:niko
/// 创建时间:2015/09/28 17:22:25
/// 描述:
/// </summary>
public class WxAPIController : MonoBehaviour
{
	public static WxAPIController Instance;
	
	public WxAPIController ()
	{
		Instance = this;
	}
	#if UNITY_ANDROID
	CallBackWithParams<string> onCallBack;
	
	/// <summary>
	/// 初始化
	/// </summary>
	public void RegToWx ()
	{
		WxAPIBinding.RegToWx ("wx77125009d79d3b29", name, "");
	}
	public void Login ()
	{
		WxAPIBinding.Login ();
	}
	public void Share ()
	{
		WxAPIBinding.Share ("", "", Application.streamingAssetsPath + "/Android/icon-60pt.png", "www.baidu.com");
	}
	void Weixincallback_LoginSuccess (string code)
	{
		Debug.Log ("Weixincallback_LoginSuccess:" + code);
	}
	void Weixincallback_shareSuccess (string code)
	{
		Debug.Log ("Weixincallback_shareSuccess:" + code);
	}
	#endif
}
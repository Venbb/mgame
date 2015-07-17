using UnityEngine;
using System.Collections;

public class SDKHandler : MonoBehaviour
{
	//	public static Callback OnPurchaseSucc;

	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	public static void Init ()
	{
//		if (!Util.NetAvailable)
//		{
//			return;		
//		}
//		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer) StoreKitHandler.Init ();
//		#endif
//		switch (Channel.curChannel) 
//		{
//			case Channel.CHANNEL_APPSTORE:
//				StoreKitHandler.Init();
//				break;
//		}
	}

	//	public static void purchaseproducts (int pId, Callback ckFun)
	//	{
	////		if (!Util.NetAvailable)
	////		{
	////			UIController.Instance.ShowAlertUI (UIController.NOTNETWORKUI_PATH, "无法连接到网络");	
	////			return;		
	////		}
	////		#if UNITY_IPHONE
	//		if (Application.platform == RuntimePlatform.IPhonePlayer)
	//		{
	////			OnPurchaseSucc = ckFun;
	//			StoreKitHandler.purchaseproducts (Channel.APPSTORE_PRODUCT_ID_PRE + pId);
	//		}
	////		#endif
	//	}

	/// <summary>
	/// 获取价格
	/// </summary>
	/// <param name="propId">Property identifier.</param>
	public static int getPrice (int propId)
	{
		int price = 0;
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			price = StoreKitHandler.getPrice (propId);
			break;
		default:
			break;
		}
		return price;
	}

	/// <summary>
	/// 内购成功返回
	/// </summary>
	/// <param name="transaction">Transaction.</param>
	public static void onPurchaseSuccessful (StoreKitTransaction transaction)
	{
//		UIController.Instance.HideWaiting ();
		string idStr = transaction.productIdentifier;
		idStr = idStr.Substring (idStr.Length - 6, 6);
//		Debuger.Log ("purchaseSuccessful idStr:" + idStr);
//		DataManager.instance.OnGetProp (System.Convert.ToInt32(idStr));
//		MainTop.Instance.UpdateData();
//		if (OnPurchaseSucc != null) OnPurchaseSucc ();
//		OnPurchaseSucc = null;
//		UIController.Instance.ShowAlertUI (UIController.ALERT_YESUI_PATH,"提示","购买成功!");
	}
}

/// <summary>
/// 渠道定义
/// </summary>
public class Channel
{
	/// <summary>
	/// 是否沙盒模式
	/// </summary>
	public const bool IsSandbox = false;
	/// <summary>
	/// 默认渠道（无）
	/// </summary>
	public const string CHANNEL_DEMO = "demo";
	/// <summary>
	/// 微信
	/// </summary>
	public const string CHANNEL_QQ_WEIXIN = "qq_weixin";
	/// <summary>
	/// AppStore
	/// </summary>
	public const string CHANNEL_APPSTORE = "AppStore";
	/// <summary>
	/// android
	/// </summary>
	public const string CHANNEL_GOOLE_ANDROID = "google_android";

	/// <summary>
	/// 当前渠道
	/// </summary>
	public static string CurChannel
	{
		get{ return PlayerPrefs.GetString ("curChannel"); }
		set{ PlayerPrefs.SetString ("curChannel", value); }
	}

	/// <summary>
	/// 苹果商店道具ID前缀
	/// </summary>
	public const string APPSTORE_PRODUCT_ID_PRE = "com.7k7k.sheep.";
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

//using Junfine.Debuger;

public class StoreKitHandler : MonoBehaviour
{
	/// <summary>
	/// 初始化，同步上架道具
	/// </summary>
	public static void Init ()
	{
		autoConfirmTransactions = false;

		enableHighDetailLogs (true);

//		var productIdentifiers = new string[] { "anotherProduct", "tt", "testProduct", "sevenDays", "oneMonthSubsciber" };
//		requestProductData (DataManager.instance.IOS_phaseids);
	}

	/// <summary>
	/// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
	/// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
	/// </summary>
	public static List<StoreKitProduct> products;

	public static bool canMakePayments
	{
		get{ return StoreKitBinding.canMakePayments (); }
	}

	/// <summary>
	/// 请求同步上架商品数据
	/// array of product ID's from iTunesConnect.  MUST match exactly what you have there!
	/// </summary>
	public static void requestProductData (string[] productIdentifiers)
	{
//		Debuger.Log( "\nrequestProductData: " + productIdentifiers.Length );
		
//		foreach( string str in productIdentifiers )
//			Debuger.Log( str + "\n" );

		StoreKitBinding.requestProductData (productIdentifiers);
	}

	/// <summary>
	/// 回复已经完成的交易
	/// </summary>
	public static void restoreCompletedTransactions ()
	{
		StoreKitBinding.restoreCompletedTransactions ();
	}

	/// <summary>
	/// 获取所有存储的交易
	/// </summary>
	public static void getAllSavedTransactions ()
	{
		List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions ();
		
		// Print all the transactions to the console
//		Debuger.Log( "\ntotal transaction received: " + transactionList.Count );
//		
//		foreach( StoreKitTransaction transaction in transactionList )
//			Debuger.Log( transaction.ToString() + "\n" );
	}

	public static void displayStoreWithProductId (string id)
	{
		StoreKitBinding.displayStoreWithProductId (id);
	}

	/// <summary>
	/// this is only necessary in the case where you turned off confirmation of transactions
	/// </summary>
	public static void finishPendingTransactions ()
	{
		StoreKitBinding.finishPendingTransactions ();
	}

	/// <summary>
	/// Cancels the downloads.
	/// </summary>
	public static void cancelDownloads ()
	{
		StoreKitBinding.cancelDownloads ();
	}

	/// <summary>
	/// Enables the high detail logs.
	/// </summary>
	/// <param name="isEnable">If set to <c>true</c> is enable.</param>
	public static void enableHighDetailLogs (bool isEnable)
	{
		StoreKitBinding.enableHighDetailLogs (isEnable);
	}

	/// <summary>
	/// 自动确认交易
	/// this is used when you want to validate receipts on your own server or when dealing with iTunes hosted content
	/// you must manually call StoreKitBinding.finishPendingTransactions when the download/validation is complete
	/// </summary>
	/// <value><c>true</c> if auto confirm transactions; otherwise, <c>false</c>.</value>
	public static bool autoConfirmTransactions
	{
		set
		{	
			StoreKitManager.autoConfirmTransactions = value;
		}
	}

	/// <summary>
	/// 支付
	/// </summary>
	public static void purchaseproducts (string productIdentifier)
	{
		// enforce the fact that we can't purchase products until we retrieve the product data
//		Debuger.Log ("preparing to purchase product: " + productIdentifier);
//		if (products == null && products.Count == 0) 
//		{
//			string str="";
//			if(Util.NetAvailable)
//			{
//				str="无法发起支付，请稍后再试!";
//				Init();
//			}
//			else
//			{
//				str="无法连接到网络，请稍后再试!";
//			}
//			UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH,"提示",str);	
//			return;
//		}
//		if (!canMakePayments) 
//		{
//			UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH,"提示","不能发起支付!");		
//			return;
//		}
//		if (IsSales (productIdentifier)) 
//		{
//			UIController.Instance.ShowWaiting();
//			StoreKitBinding.purchaseProduct (productIdentifier, 1);
//		} 
//		else 
//		{
//			UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH,"提示","道具未上架!");	
//		}
	}

	/// <summary>
	/// 是否上架
	/// </summary>
	/// <returns><c>true</c> if is sales the specified productId; otherwise, <c>false</c>.</returns>
	/// <param name="productId">Product identifier.</param>
	public static bool IsSales (string productIdentifier)
	{
		bool isSales = false;
		if (products != null && products.Count > 0)
		{
			foreach (StoreKitProduct p in products)
			{
				if (p.productIdentifier.Equals (productIdentifier)) return true;
			}
		}
		return isSales;
	}

	/// <summary>
	/// 获取价格
	/// </summary>
	/// <returns>The price.</returns>
	/// <param name="propId">Property identifier.</param>
	public static int getPrice (int propId)
	{
		int price = 0;
		string productIdentifier = Channel.APPSTORE_PRODUCT_ID_PRE + propId;
		if (products != null && products.Count > 0)
		{
			foreach (StoreKitProduct p in products)
			{
				if (p.productIdentifier.Equals (productIdentifier))
				{
					int.TryParse (p.price, out price);
					break;
				}
			}
		}
		return price;
	}
}

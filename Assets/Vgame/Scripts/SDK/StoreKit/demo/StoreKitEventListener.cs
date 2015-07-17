using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using Junfine.Debuger;


public class StoreKitEventListener : MonoBehaviour
{
	//#if UNITY_IPHONE
	void OnEnable ()
	{
		// Listens to all the StoreKit events. All event listeners MUST be removed before this object is disposed!
		StoreKitManager.transactionUpdatedEvent += transactionUpdatedEvent;
		StoreKitManager.productPurchaseAwaitingConfirmationEvent += productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent += purchaseFailedEvent;
		StoreKitManager.productListReceivedEvent += productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailedEvent;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinishedEvent;
		StoreKitManager.paymentQueueUpdatedDownloadsEvent += paymentQueueUpdatedDownloadsEvent;
	}

	
	void OnDisable ()
	{
		// Remove all the event handlers
		StoreKitManager.transactionUpdatedEvent -= transactionUpdatedEvent;
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent -= purchaseFailedEvent;
		StoreKitManager.productListReceivedEvent -= productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent -= productListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailedEvent;
		StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinishedEvent;
		StoreKitManager.paymentQueueUpdatedDownloadsEvent -= paymentQueueUpdatedDownloadsEvent;
	}

	
	
	void transactionUpdatedEvent (StoreKitTransaction transaction)
	{
//		Debuger.Log( "transactionUpdatedEvent: " + transaction );
	}

	
	void productListReceivedEvent (List<StoreKitProduct> productList)
	{
//		Debuger.Log( "productListReceivedEvent. total products received: " + productList.Count );
//		
//		// print the products to the console
//		foreach( StoreKitProduct product in productList )
//			Debuger.Log( product.ToString() + "\n" );
		StoreKitHandler.products = productList;
	}

	
	void productListRequestFailedEvent (string error)
	{
//		Debuger.Log( "productListRequestFailedEvent: " + error );
	}


	void purchaseFailedEvent (string error)
	{
//		Debuger.Log ("purchaseFailedEvent: " + error);
//		UIController.Instance.HideWaiting ();
//		SDKHandler.OnPurchaseSucc = null;
//		UIController.Instance.ShowAlertUI (UIController.ALERT_YESUI_PATH, "提示", error);
	}


	void purchaseCancelledEvent (string error)
	{
//		Debuger.Log( "purchaseCancelledEvent: " + error );
//		SDKHandler.OnPurchaseSucc = null;
//		UIController.Instance.HideWaiting ();
	}

	
	void productPurchaseAwaitingConfirmationEvent (StoreKitTransaction transaction)
	{
//		Debuger.Log( "productPurchaseAwaitingConfirmationEvent: " + transaction );
//
//		StoreKitHandler.finishPendingTransactions();
//
//		rq_storekit_result result = new rq_storekit_result ();
//		result.receipt = transaction.base64EncodedTransactionReceipt;
//		result.quantity = transaction.quantity;
//		result.identifier = transaction.productIdentifier;
//		result.transactionid = transaction.transactionIdentifier;
//
//		GameController.Instance.GameNet.storekitCall (OnStoreKitCallback, result);
	}

	void OnStoreKitCallback (bool succ)
	{
//		if (succ) 
//		{
//			Debuger.Log("订单已经提交验证!");
//			//			UIController.Instance.ShowAlertUI (UIController.ALERT_YESUI_PATH,"提示","订单已经提交，请等待...");
//		}
//		else
//		{
//			Debuger.Log("购买验证错误!");
//		}
	}

	void purchaseSuccessfulEvent (StoreKitTransaction transaction)
	{
//		Debuger.Log( "purchaseSuccessfulEvent: " + transaction );

		SDKHandler.onPurchaseSuccessful (transaction);
	}

	
	void restoreTransactionsFailedEvent (string error)
	{
//		Debuger.Log( "restoreTransactionsFailedEvent: " + error );
	}

	
	void restoreTransactionsFinishedEvent ()
	{
//		Debuger.Log( "restoreTransactionsFinished" );
	}

	
	void paymentQueueUpdatedDownloadsEvent (List<StoreKitDownload> downloads)
	{
//		Debuger.Log( "paymentQueueUpdatedDownloadsEvent: " );
//		foreach( var dl in downloads )
//			Debuger.Log( dl );
	}
	
	//#endif
}


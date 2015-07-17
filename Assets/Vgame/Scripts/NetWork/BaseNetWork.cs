using UnityEngine;
using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using Junfine.Debuger;

public class BaseNetWork : BaseNet
{
	/// <summary>
	/// 请求超时时间
	/// </summary>
	const float TIMEOUT_TIME = 5f;
	/// <summary>
	/// 邮件命令
	/// </summary>
	public const int HEARTBEAT_PUSH_COMMAND_MAIL = 0;
	// 邮件命令
	/// <summary>
	/// 好友更新
	/// </summary>
	public const int HEARTBEAT_PUSH_COMMAND_FIRENDS = 1;
	// 好友更新
	/// <summary>
	/// 玩家数据
	/// </summary>
	public const int HEARTBEAT_PUSH_COMMAND_PLAYERDATA = 2;
	//

	#region 版本检测

	private CallBackWithParams<bool,rqback_checkVersion> checkVersionCallback;

	/// <summary>
	/// 版本检测
	/// </summary>
	/// <param name="call">Call.</param>
	public void checkVersion(CallBackWithParams<bool,rqback_checkVersion> call)
	{
		checkVersionCallback = call;
		string interfaceName = "app/checkversion";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_checkversion rq = new rq_checkversion();
		rq.playerid = Player.ID;
		rq.result.os = Util.getTarget;
		rq.result.softwareversion = Util.getSoftwareversion();
		rq.result.resversion = Util.getResversion();
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, onCheckVersionCallBack, args);

		Invoke("checkVersionTimeOut", TIMEOUT_TIME);
	}

	void checkVersionTimeOut()
	{
		Debuger.Log("网络连接超时");
		onCheckVersionCallBack("");
	}

	/// <summary>
	/// 版本检测回调
	/// </summary>
	/// <param name="json">Json.</param>
	void onCheckVersionCallBack(string json)
	{
		CancelInvoke("checkVersionTimeOut");

		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("版本检测回调:" + json);
		rqback_checkVersion rqback = JsonMapper.ToObject<rqback_checkVersion>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (checkVersionCallback != null) checkVersionCallback(succ, rqback);
		checkVersionCallback = null;
	}

	#endregion

	#region 微信登录

	public CallBackWithParams<bool,rqback_touristLogin> weoxinLoginCallback;

	/// <summary>
	/// 微信登录
	/// </summary>
	/// <param name="callback"></param>
	/// <param name="code">token</param>
	public void weoxinLoginCall(CallBackWithParams<bool,rqback_touristLogin> callback, string code)
	{
		if (IsInvoking("OnOnweixinLoginCallTimeOut")) return;
		weoxinLoginCallback = callback;
		if (!Util.NetAvailable)
		{
			OnweixinLoginCallback("");
			return;		
		}
		UIController.Instance.ShowWaiting();

		string interfaceName = "player/login";
		m_url = CommonResource.URL + interfaceName;

		rq_touristLogin data = new rq_touristLogin();
		data.playerid = Player.ID;
		data.time = CommonResource.TimeUnix;
		data.result.from = "weixin";
		data.result.udid = Util.DeviceID;
		data.result.code = code;
		data.xsrf = Util.GetKey(JsonMapper.ToJson(data.result), data.time); 

		string sb = JsonMapper.ToJson(data);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, OnweixinLoginCallback, args);
		Invoke("OnOnweixinLoginCallTimeOut", TIMEOUT_TIME);
	}

	void OnOnweixinLoginCallTimeOut()
	{
		Debuger.Log("微信登陆超时...");
		OnweixinLoginCallback("");
	}

	void OnweixinLoginCallback(string json)
	{
		CancelInvoke("OnOnweixinLoginCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("微信登录回调:" + json);
		rqback_touristLogin rqback = JsonMapper.ToObject<rqback_touristLogin>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
//		if (succ)
//		{
//			Player.ID = rqback.result.player_id;
//			PrefabManager.token = rqback.result.token;	
//		}
		if (weoxinLoginCallback != null) weoxinLoginCallback(succ, rqback);
		weoxinLoginCallback = null;
	}

	#endregion

	#region 游客登录

	public CallBackWithParams<bool> touristLoginCallback;

	/// <summary>
	/// 游客登录
	/// </summary>
	/// <param name="callback">Callback.</param>
	/// <param name="data">Data.</param>
	public void touristLoginCall(CallBackWithParams<bool> callback)
	{
		if (IsInvoking("touristLoginTimeOut")) return;
		touristLoginCallback = callback;
		if (!Util.NetAvailable)
		{
			onTouristLoginCallback("");
			return;		
		}					
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/login";     
		m_url = CommonResource.URL + interfaceName;

		rq_touristLogin data = new rq_touristLogin();
		data.playerid = Player.ID;
		data.time = CommonResource.TimeUnix;
		data.result.deviceid = Util.DeviceID;
		data.result.from = Util.getTarget;
		data.result.udid = GameController.Instance.UUID;//Util.DeviceID;
		data.xsrf = Util.GetKey(JsonMapper.ToJson(data.result), data.time);

		string sb = JsonMapper.ToJson(data);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, onTouristLoginCallback, args);

		Invoke("touristLoginTimeOut", TIMEOUT_TIME);
	}

	void touristLoginTimeOut()
	{
		Debuger.Log("登陆超时...");
		onTouristLoginCallback("");
	}

	/// <summary>
	/// 游客登录回调
	/// </summary>
	/// <param name="json">Json.</param>
	void onTouristLoginCallback(string json)
	{
		CancelInvoke("touristLoginTimeOut");

		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("游客登录回调:" + json);
		rqback_touristLogin rqback = JsonMapper.ToObject<rqback_touristLogin>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (succ)
		{
			Channel.CurChannel = Channel.CHANNEL_DEMO;
			GameController.Instance.UUID = GameController.Instance.DeviceID;
			PrefabManager.ingame = 2;
			Player.ID = rqback.result.player_id;
			PrefabManager.token = rqback.result.token;	
		}
		if (touristLoginCallback != null) touristLoginCallback(succ);
		touristLoginCallback = null;
	}

	#endregion

	#region 同步数据

	private CallBackWithParams<bool,rqback_getPlayerData> syncData2ServiceCallback;

	/// <summary>
	/// 同步数据
	/// </summary>
	/// <param name="callback">Callback.</param>
	/// <param name="data">Data.</param>
	public void SyncData2Service(CallBackWithParams<bool,rqback_getPlayerData> callback)
	{
		if (IsInvoking("SyncData2ServiceTimeOut")) return;
		syncData2ServiceCallback = callback;
		if (!Util.NetAvailable)
		{
			OnSyncData2ServiceBack("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/sync";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);

		rq_updataPlayerData data = new rq_updataPlayerData();
		data.playerid = Player.ID;
		data.result.coin = Player.pumpkin;
		data.result.coinAddition = Player.pumpkinAddition;
		data.result.decoration = Player.decoration;
		data.result.diamond = Player.tomato;
		data.result.distance = Player.distance;
		data.result.energy = Player.energy;
		data.result.equipment = Player.equipment;
		data.result.fragment = Player.fragment;
		data.result.hp = Player.hp;
		data.result.id = Player.ID;
		data.result.integral = Player.integral;
		data.result.integralAddition = Player.integralAddition;
		data.result.isNew = Player.isNew;
		data.result.level = Player.level;
		data.result.nickName = Player.nickName;
		data.result.props = Player.props;
		data.result.sigleintefral = Player.sigleintefral;
		data.result.singleDistance = Player.singleDistance;
        data.result.currentCheckPoint = Player.currentCheckPoint;
        data.result.havedCheckPoints = Player.havedCheckPoints;
		data.time = CommonResource.TimeUnix;
		
		data.xsrf = Util.GetKey(JsonMapper.ToJson(data.result), data.time);

		string sb = JsonMapper.ToJson(data);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, OnSyncData2ServiceBack, args);
		Invoke("SyncData2ServiceTimeOut", 3f);
	}

	void SyncData2ServiceTimeOut()
	{
		Debuger.Log("同步数据超时...");
		OnSyncData2ServiceBack("");
	}

	/// <summary>
	/// 同步数据回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OnSyncData2ServiceBack(string json)
	{
		CancelInvoke("SyncData2ServiceTimeOut");

		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("同步数据回调成功:" + json);
		rqback_getPlayerData rqback = JsonMapper.ToObject<rqback_getPlayerData>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		}
//        JsonData data = JsonMapper.ToObject<JsonData>(json);
//		bool succ = false;
//		if (data != null && (int)data["status"]==1) 
//		{
//			succ=true;
//		}
		if (syncData2ServiceCallback != null) syncData2ServiceCallback(succ, rqback);

		syncData2ServiceCallback = null;
	}

	#endregion

	#region 请求用户数据

	private CallBackWithParams<bool,rqback_getPlayerData> getPlayerDataCallback;

	/// <summary>
	/// 请求用户数据
	/// </summary>
	/// <param name="call">Call.</param>
	public void getPlayerData(CallBackWithParams<bool,rqback_getPlayerData> call)
	{
		if (IsInvoking("OnetPlayerDataTimeOut")) return;
						
		getPlayerDataCallback = call;
		if (!Util.NetAvailable)
		{
			OnGetPlayerDataCallback("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/sync";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		GetURL(m_url, OnGetPlayerDataCallback);
		Invoke("OnetPlayerDataTimeOut", TIMEOUT_TIME);
	}

	void OnetPlayerDataTimeOut()
	{
		Debuger.Log("请求用户数超时...");
		OnGetPlayerDataCallback("");
	}

	/// <summary>
	/// 请求用户数据回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OnGetPlayerDataCallback(string json)
	{
		CancelInvoke("OnetPlayerDataTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("请求用户数据回调成功:" + json);
		rqback_getPlayerData rqback = JsonMapper.ToObject<rqback_getPlayerData>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		} 
		if (getPlayerDataCallback != null) getPlayerDataCallback(succ, rqback);
		getPlayerDataCallback = null;
	}

	#endregion


	#region 商城列表

	private CallBackWithParams<bool,rqback_goodsData> getShoplistCallback;

	public void getShoplistCall(CallBackWithParams<bool,rqback_goodsData> call)
	{
		if (IsInvoking("OngetShoplistCallTimeOut")) return;
		getShoplistCallback = call;
		if (!Util.NetAvailable)
		{
			net_getShoplistEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "app/shopgoods";
		m_url = string.Format("{0}{1}", CommonResource.URL, interfaceName);
		GetURL(m_url, net_getShoplistEvent);
		Invoke("OngetShoplistCallTimeOut", TIMEOUT_TIME);
	}

	void OngetShoplistCallTimeOut()
	{
		Debuger.Log("请求商城列表超时...");
		net_getShoplistEvent("");
	}

	void net_getShoplistEvent(string json)
	{
		CancelInvoke("OngetShoplistCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("请求商城列表回调:" + json);
		rqback_goodsData rqback = JsonMapper.ToObject<rqback_goodsData>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		}
		if (getShoplistCallback != null) getShoplistCallback(succ, rqback);
		getShoplistCallback = null;
	}

	#endregion

	#region 签到

	private CallBackWithParams<bool,rqback_Signin> signinCallback;

	/// <summary>
	/// 签到
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="beta">Beta.</param>
	public void signinCall(CallBackWithParams<bool,rqback_Signin> call, int beta)
	{
		if (IsInvoking("onSigninCallTimeOut")) return;
						
		signinCallback = call;
		if (!Util.NetAvailable)
		{
			net_signinEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();

		string interfaceName = "app/signin";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_Signin rq = new rq_Signin();
		rq.playerid = Player.ID;
		rq.result.beta = beta;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_signinEvent, args);
		Invoke("onSigninCallTimeOut", TIMEOUT_TIME);
	}

	void onSigninCallTimeOut()
	{
		Debuger.Log("签到超时...");
		net_signinEvent("");
	}

	/// <summary>
	/// 签到回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_signinEvent(string json)
	{
		CancelInvoke("onSigninCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("签到回调:" + json);
		rqback_Signin rqback = JsonMapper.ToObject<rqback_Signin>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;		
		}
		if (signinCallback != null) signinCallback(succ, rqback);
		signinCallback = null;
	}

	#endregion

	#region 补签

	private CallBackWithParams<bool,rqback_Signin> signamendedCallback;

	/// <summary>
	/// 补签
	/// </summary>
	/// <param name="call">Call.</param>
	public void signamendedCall(CallBackWithParams<bool,rqback_Signin> call)
	{
		if (IsInvoking("OnSignamendedCallTimeOut")) return;
		signamendedCallback = call;		
		if (!Util.NetAvailable)
		{
			net_signamendedEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
	
		string interfaceName = "app/signamended";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_Signamended rq = new rq_Signamended();
		rq.playerid = Player.ID;
//        rq.result="";
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_signamendedEvent, args);
		Invoke("OnSignamendedCallTimeOut", TIMEOUT_TIME);
	}

	void OnSignamendedCallTimeOut()
	{
		Debuger.Log("补签请求超时...");
		net_signamendedEvent("");
	}

	/// <summary>
	/// 补签回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_signamendedEvent(string json)
	{
		CancelInvoke("OnSignamendedCallTimeOut");
		UIController.Instance.HideWaiting();

		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("签到回调:" + json);
		rqback_Signin rqback = JsonMapper.ToObject<rqback_Signin>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;		
		}
		if (signamendedCallback != null) signamendedCallback(succ, rqback);
		signamendedCallback = null;
	}

	#endregion

	#region 邮件列表

	public CallBackWithParams<bool,rqback_maillist> getEmaillistCallback;

	/// <summary>
	/// 获取邮件列表
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="lastId">Last identifier.</param>
	public void getEmaillist(CallBackWithParams<bool,rqback_maillist> call, int lastId)
	{
		if (IsInvoking("OnGetEmaillistTimeOut")) return;				
		getEmaillistCallback = call;
		if (!Util.NetAvailable)
		{
			OnGetMailListCallback("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "mail/list";
		m_url = string.Format("{0}{1}?sid={2}&last={3}", CommonResource.URL, interfaceName, PrefabManager.token, lastId);
		GetURL(m_url, OnGetMailListCallback);
		Invoke("OnGetEmaillistTimeOut", TIMEOUT_TIME);
	}

	void OnGetEmaillistTimeOut()
	{
		Debuger.Log("邮件列表请求超时...");
		OnGetMailListCallback("");
	}

	/// <summary>
	/// 获取邮件列表回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OnGetMailListCallback(string json)
	{
		CancelInvoke("OnGetEmaillistTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("邮件列表:" + json);
		rqback_maillist rqback = JsonMapper.ToObject<rqback_maillist>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		}
		if (getEmaillistCallback != null) getEmaillistCallback(succ, rqback);
		getEmaillistCallback = null;
	}

	#endregion

	public CallBackWithParams<bool> deletMailCallback;

	/// <summary>
	/// 删除邮件
	/// </summary>
	/// <param name="callback">Callback.</param>
	/// <param name="mailId">Mail identifier.</param>
	public void deletMail(CallBackWithParams<bool> callback, int mailId)
	{
		if (IsInvoking("OnDeletMailCallTimeOut")) return;
		deletMailCallback = callback;		
		if (!Util.NetAvailable)
		{
			OnDeletMailCallBack("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "mail/delete";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);

		rq_DeletMail rq = new rq_DeletMail();
		rq.result.mail_id = mailId;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, OnDeletMailCallBack, args);
		Invoke("OnDeletMailCallTimeOut", TIMEOUT_TIME);
	}

	void OnDeletMailCallTimeOut()
	{
		Debuger.Log("删除邮件请求超时...");
		OnDeletMailCallBack("");
	}

	/// <summary>
	/// 删除邮件回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OnDeletMailCallBack(string json)
	{
		CancelInvoke("OnDeletMailCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("删除邮件回调:" + json);
		JsonData jd = JsonMapper.ToObject(json);
		bool succ = false;
		if (jd != null && (int)jd["status"] == 1)
		{
			succ = true;	
		}
		if (deletMailCallback != null) deletMailCallback(succ);
		deletMailCallback = null;
	}

	public CallBackWithParams<bool> onReadMailCallback;

	/// <summary>
	/// 设置邮件读取状态
	/// </summary>
	/// <param name="callback">Callback.</param>
	/// <param name="mailId">Mail identifier.</param>
	/// <param name="fState">F state.</param>
	/// <param name="sState">S state.</param>
	public void readMail(CallBackWithParams<bool> callback, int mailId, int fState, int sState)
	{
		if (IsInvoking("OnReadMailTimeOut")) return;
						
		onReadMailCallback = callback;
		if (!Util.NetAvailable)
		{
			OnReadMailCallBack("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "mail/read";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);

		rq_ReadMail rq = new rq_ReadMail();
		rq.result.mail_id = mailId;
		rq.result.mail_status_first = fState;
		rq.result.mail_status_second = sState;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, OnReadMailCallBack, args);
		Invoke("OnReadMailTimeOut", TIMEOUT_TIME);
	}

	void OnReadMailTimeOut()
	{
		Debuger.Log("设置邮件读取状态请求超时...");
		OnReadMailCallBack("");
	}

	/// <summary>
	/// 设置邮件读取状态回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OnReadMailCallBack(string json)
	{
		CancelInvoke("OnReadMailTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("设置邮件读取状态回调:" + json);
		JsonData jd = JsonMapper.ToObject(json);
		bool succ = false;
		if (jd != null && (int)jd["status"] == 1)
		{
			succ = true;	
		}
		if (onReadMailCallback != null) onReadMailCallback(succ);
		onReadMailCallback = null;
	}

	#region 总排行

	private CallBackWithParams<bool,rqback_Rank> getAllRanklistCallback;

	/// <summary>
	/// 总排行
	/// </summary>
	/// <param name="call">Call.</param>
	public void getAllRanklist(CallBackWithParams<bool,rqback_Rank> call)
	{
		if (IsInvoking("OnGetAllRanklistTimeOut")) return;
						
		getAllRanklistCallback = call;
		if (!Util.NetAvailable)
		{
			net_getAllRanklistEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/topcharts";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		GetURL(m_url, net_getAllRanklistEvent);
		Invoke("OnGetAllRanklistTimeOut", TIMEOUT_TIME);
	}

	void OnGetAllRanklistTimeOut()
	{
		Debuger.Log("总排行请求超时...");
		net_getAllRanklistEvent("");
	}

	/// <summary>
	/// 总排行回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_getAllRanklistEvent(string json)
	{
		CancelInvoke("OnGetAllRanklistTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("总排行回调:" + json);
		rqback_Rank rqback = JsonMapper.ToObject<rqback_Rank>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (getAllRanklistCallback != null) getAllRanklistCallback(succ, rqback);
		getAllRanklistCallback = null;
	}

	#endregion

	#region 周排行

	private CallBackWithParams<bool,rqback_Rank> getWeekRanklistCallback;

	/// <summary>
	/// 周排行
	/// </summary>
	/// <param name="call">Call.</param>
	public void getWeekRanklist(CallBackWithParams<bool,rqback_Rank> call)
	{
		if (IsInvoking("OnGetWeekRanklistTimeOut")) return;
						
		getWeekRanklistCallback = call;
		if (!Util.NetAvailable)
		{
			net_getWeekRanklistEvent("");
			return;
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/weeklytop";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		GetURL(m_url, net_getWeekRanklistEvent);
		Invoke("OnGetWeekRanklistTimeOut", TIMEOUT_TIME);
	}

	void OnGetWeekRanklistTimeOut()
	{
		Debuger.Log("周排行请求超时...");
		net_getWeekRanklistEvent("");
	}

	/// <summary>
	/// 周排行回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_getWeekRanklistEvent(string json)
	{
		CancelInvoke("OnGetWeekRanklistTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("周排行回调:" + json);
		rqback_Rank rqback = JsonMapper.ToObject<rqback_Rank>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (getWeekRanklistCallback != null) getWeekRanklistCallback(succ, rqback);
		getWeekRanklistCallback = null;
	}

	#endregion

	#region 活动排行

	private CallBackWithParams<bool,rqback_Rank> getActRanklistCallback;

	/// <summary>
	/// 活动排行
	/// </summary>
	/// <param name="call">Call.</param>
	public void getActRanklist(CallBackWithParams<bool,rqback_Rank> call)
	{
		if (IsInvoking("OnGetActRanklistTimeOut")) return;
						
		getActRanklistCallback = call;
		if (!Util.NetAvailable)
		{
			net_getActRanklistEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/topactivities";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		GetURL(m_url, net_getActRanklistEvent);
		Invoke("OnGetActRanklistTimeOut", TIMEOUT_TIME);
	}

	void OnGetActRanklistTimeOut()
	{
		Debuger.Log("活动排行请求超时...");
		net_getActRanklistEvent("");
	}

	/// <summary>
	/// 活动排行回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_getActRanklistEvent(string json)
	{
		CancelInvoke("OnGetActRanklistTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("活动排行回调:" + json);
		rqback_Rank rqback = JsonMapper.ToObject<rqback_Rank>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (getActRanklistCallback != null) getActRanklistCallback(succ, rqback);
		getActRanklistCallback = null;
	}

	#endregion

	#region 申请好友

	private CallBackWithParams<bool> applyFriendCallback;

	/// <summary>
	/// 申请好友
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="id">Identifier.</param>
	public void applyFriendCall(CallBackWithParams<bool> call, int id)
	{
		if (IsInvoking("OnApplyFriendCallTimeOut")) return;
						
		applyFriendCallback = call;
		if (!Util.NetAvailable)
		{
			OnApplyFriendCallback("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "firends/apply";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_ApplyFriend rq = new rq_ApplyFriend();
		rq.playerid = Player.ID;
		rq.result.pid = id;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, OnApplyFriendCallback, args);
		Invoke("OnApplyFriendCallTimeOut", TIMEOUT_TIME);
	}

	void OnApplyFriendCallTimeOut()
	{
		Debuger.Log("申请好友请求超时...");
		OnApplyFriendCallback("");
	}

	/// <summary>
	/// 申请好友回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OnApplyFriendCallback(string json)
	{
		CancelInvoke("OnApplyFriendCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("申请好友回调:" + json);
		if (string.IsNullOrEmpty(json))
		{
			UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH, "提示", "网络请求失败！");
			return;	
		}
		JsonData data = JsonMapper.ToObject<JsonData>(json);
		bool succ = false;
		if (data != null && (int)data["status"] == 1)
		{
			succ = true;
		}
		if (applyFriendCallback != null) applyFriendCallback(succ);
		applyFriendCallback = null;
	}

	#endregion

	#region 请求好友列表

	public CallBackWithParams<bool,rqback_FriendList> getFriendListCallback;

	/// <summary>
	/// 请求好友列表
	/// </summary>
	/// <param name="call">Call.</param>
	public void getFriendListCall(CallBackWithParams<bool,rqback_FriendList> call)
	{
		if (IsInvoking("OnGetFriendListTimeOut")) return;
						
		getFriendListCallback = call;
		if (!Util.NetAvailable)
		{
			OngetFriendlistCallback("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "firends/list";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_FriendList rq = new rq_FriendList();
		rq.playerid = Player.ID;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, OngetFriendlistCallback);
		Invoke("OnGetFriendListTimeOut", TIMEOUT_TIME);
	}

	void OnGetFriendListTimeOut()
	{
		Debuger.Log("好友列表请求超时...");
		OngetFriendlistCallback("");
	}

	/// <summary>
	/// 请求好友列表回调
	/// </summary>
	/// <param name="json">Json.</param>
	void OngetFriendlistCallback(string json)
	{
		CancelInvoke("OnGetFriendListTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("请求好友列表回调:" + json);
		rqback_FriendList rqback = JsonMapper.ToObject<rqback_FriendList>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (succ)
		{
			if (rqback.result == null)
			{
				Util.ClearFriendData();
			}
			else
			{
				Util.UpdataFriendData(rqback.result);
			}		
		}
		if (getFriendListCallback != null) getFriendListCallback(succ, rqback);
		getFriendListCallback = null;
	}

	#endregion

	#region 确认添加好友

	private CallBackWithParams<bool,rqback_sureAddFriend> sureAddFriendCallback;

	/// <summary>
	/// 确认添加好友
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="MailId">Mail identifier.</param>
	/// <param name="accept">Accept.</param>
	public void sureAddFriendCall(CallBackWithParams<bool,rqback_sureAddFriend> call, int MailId, int accept)
	{
		if (IsInvoking("OnSureAddFriendCallTimeOut")) return;
		sureAddFriendCallback = call;
		if (!Util.NetAvailable)
		{
			net_sureAddFriendEvent("");
			return;
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "firends/add";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_sureAddFriend rq = new rq_sureAddFriend();
		rq.playerid = Player.ID;
		rq.result.mail_id = MailId;
		rq.result.accept = accept;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, net_sureAddFriendEvent, args);
		Invoke("OnSureAddFriendCallTimeOut", TIMEOUT_TIME);
	}

	void OnSureAddFriendCallTimeOut()
	{
		Debuger.Log("确认添加好友请求超时...");
		net_sureAddFriendEvent("");
	}

	/// <summary>
	/// 确认添加好友回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_sureAddFriendEvent(string json)
	{
		CancelInvoke("OnSureAddFriendCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("确认添加好友回调:" + json);
		rqback_sureAddFriend rqback = JsonMapper.ToObject<rqback_sureAddFriend>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (sureAddFriendCallback != null) sureAddFriendCallback(succ, rqback);
		sureAddFriendCallback = null;
	}

	#endregion

	#region 删除好友

	private CallBackWithParams<bool> deleteFriendCallback;

	/// <summary>
	/// 删除好友
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="friendID">Friend I.</param>
	public void deleteFriendCall(CallBackWithParams<bool> call, int friendID)
	{
		if (IsInvoking("OnDeleteFriendCallTimeOut")) return;
		deleteFriendCallback = call;
		if (!Util.NetAvailable)
		{
			net_deleteFriendEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "firends/delete";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_DeleteFriend rq = new rq_DeleteFriend();
		rq.playerid = Player.ID;
		rq.result.firend_player_id = friendID;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, net_deleteFriendEvent, args);
		Invoke("OnDeleteFriendCallTimeOut", TIMEOUT_TIME);
	}

	void OnDeleteFriendCallTimeOut()
	{
		Debuger.Log("删除好友请求超时...");
		net_deleteFriendEvent("");
	}

	/// <summary>
	/// 删除好友回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_deleteFriendEvent(string json)
	{
		CancelInvoke("OnDeleteFriendCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("删除好友回调:" + json);
		rqback_deleteFriend rqback = JsonMapper.ToObject<rqback_deleteFriend>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (deleteFriendCallback != null) deleteFriendCallback(succ);
		deleteFriendCallback = null;
	}

	#endregion

	#region 掠夺复仇开启

	private CallBackWithParams<bool> openPlunderRevengeCallback;

	/// <summary>
	/// 掠夺复仇开启
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	public void openPlunderRevengeCall(CallBackWithParams<bool> call, int playerId)
	{
		if (IsInvoking("OnOpenPlunderRevengeCallTimeOut")) return;
		openPlunderRevengeCallback = call;
		if (!Util.NetAvailable)
		{
			net_openPlunderRevengeEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/plunder";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_openPlunderRevenge rq = new rq_openPlunderRevenge();
		rq.playerid = Player.ID;
		rq.result.plunder_player_id = playerId;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, net_openPlunderRevengeEvent, args);
		Invoke("OnOpenPlunderRevengeCallTimeOut", TIMEOUT_TIME);
	}

	void OnOpenPlunderRevengeCallTimeOut()
	{
		Debuger.Log("掠夺复仇开启请求超时...");
		net_openPlunderRevengeEvent("");
	}

	/// <summary>
	/// 掠夺复仇开启回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_openPlunderRevengeEvent(string json)
	{
		CancelInvoke("OnOpenPlunderRevengeCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("掠夺复仇开启回调:" + json);
		rqback_openPlunderRevenge rqback = JsonMapper.ToObject<rqback_openPlunderRevenge>(json);
		bool succ = false;
		if (rqback != null)
		{
			succ = rqback.status == 1;
			if (!succ)
			{
				int errno = rqback.errno;
				//			17	玩家处于保护期不能进行掠夺或挑战
				//			18	不能掠夺自己
				//			19	已经挑战过对方了
				//			20	挑战已过期
				switch (errno)
				{
					case 17:
						UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH, "提示", "玩家处于保护期不能进行掠夺或挑战");
						break;
					case 18:
						UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH, "提示", "不能掠夺自己");
						break;
				}
			}
		}
		if (openPlunderRevengeCallback != null) openPlunderRevengeCallback(succ);
		openPlunderRevengeCallback = null;
	}

	#endregion

	#region 掠夺复仇结算

	private CallBackWithParams<bool> getPlunderRevengeResultCallback;

	/// <summary>
	/// 掠夺复仇结算
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="distance">Distance.</param>
	/// <param name="revenge">Revenge.</param>
	public void getPlunderRevengeResultCall(CallBackWithParams<bool> call, int playerId, int distance, int revenge)
	{
		if (IsInvoking("OnGetPlunderRevengeResultCallTimeOut")) return;
						
		getPlunderRevengeResultCallback = call;
		if (!Util.NetAvailable)
		{
			net_getPlunderRevengeResultEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/plundersettle";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_getPlunderRevengeResult rq = new rq_getPlunderRevengeResult();
		rq.playerid = Player.ID;
		rq.result.plunder_player_id = playerId;
		rq.result.distance = distance;
		rq.result.revenge = revenge;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_getPlunderRevengeResultEvent, args);
		Invoke("OnGetPlunderRevengeResultCallTimeOut", TIMEOUT_TIME);
	}

	void OnGetPlunderRevengeResultCallTimeOut()
	{
		Debuger.Log("掠夺复仇结算请求超时...");
		net_getPlunderRevengeResultEvent("");
	}

	/// <summary>
	/// 掠夺复仇结算回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_getPlunderRevengeResultEvent(string json)
	{
		CancelInvoke("OnGetPlunderRevengeResultCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("掠夺复仇结算回调:" + json);
		rqback_getPlunderRevengeResult rqback = JsonMapper.ToObject<rqback_getPlunderRevengeResult>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		}
		if (getPlunderRevengeResultCallback != null) getPlunderRevengeResultCallback(succ);
	}

	#endregion

	#region 发起挑战

	public CallBackWithParams<bool,bool> canChallengeCallback;

	/// <summary>
	/// 是否发起挑战
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="id">Identifier.</param>
	public void canChallengeCall(CallBackWithParams<bool,bool> call, int id)
	{
		if (IsInvoking("OnCanChallengeCallTimeOut")) return;						
		canChallengeCallback = call;
		if (!Util.NetAvailable)
		{
			net_canChallengeEvent("");
			return;		
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/canchallenge";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_challenge rq = new rq_challenge();
		rq.playerid = Player.ID;
		rq.result.pid = id;
		rq.result.integral = Player.integral;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		
		GetURL(m_url, net_canChallengeEvent, args);
		Invoke("OnCanChallengeCallTimeOut", TIMEOUT_TIME);
	}

	void OnCanChallengeCallTimeOut()
	{
		Debuger.Log("是否发起挑战请求超时...");
		net_canChallengeEvent("");
	}

	void net_canChallengeEvent(string json)
	{
		CancelInvoke("OnCanChallengeCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("是否可以发起挑战回调:" + json);
		JsonData jd = JsonMapper.ToObject(json);
		bool succ = false;
		bool can = false;
		int status = (int)jd["status"];
		if (status == 1)
		{
			succ = true;
			can = (bool)jd["result"];
		}
		if (canChallengeCallback != null) canChallengeCallback(succ, can);
		canChallengeCallback = null;
	}

	private CallBackWithParams<bool> challengeCallback;

	/// <summary>
	/// 发起挑战
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	public void challengeCall(CallBackWithParams<bool> call, int playerId, int integral)
	{
		if (IsInvoking("OnChallengeCallTimeOut")) return;
		challengeCallback = call;
		if (!Util.NetAvailable)
		{
			net_challengeEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/challenge";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_challenge rq = new rq_challenge();
		rq.playerid = Player.ID;
		rq.result.pid = playerId;
		rq.result.integral = integral;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);

		GetURL(m_url, net_challengeEvent, args);
		Invoke("OnChallengeCallTimeOut", TIMEOUT_TIME);
	}

	void OnChallengeCallTimeOut()
	{
		Debuger.Log("发起挑战请求超时...");
		net_challengeEvent("");
	}

	/// <summary>
	/// 发起挑战回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_challengeEvent(string json)
	{
		CancelInvoke("OnChallengeCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("发起挑战回调:" + json);
		rqback_challenge rqback = JsonMapper.ToObject<rqback_challenge>(json);
		bool succ = false;
		if (rqback != null)
		{
			succ = rqback.status == 1;
			if (!succ)
			{
				int errno = rqback.errno;
				//			19	已经挑战过对方了
				switch (errno)
				{
					case 19:
						UIController.Instance.ShowAlertUI(UIController.ALERT_YESUI_PATH, "提示", "已经挑战过对方了");
						break;
				}
			}
		}
		if (challengeCallback != null) challengeCallback(succ);
		challengeCallback = null;
	}

	#endregion

	#region 接受挑战

	private CallBackWithParams<bool> acceptChallengeCallback;

	/// <summary>
	/// 接受挑战
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="mail_id">Mail_id.</param>
	public void acceptChallengeCall(CallBackWithParams<bool> call, int mailId, int integral)
	{
		if (IsInvoking("OnAcceptChallengeCallTimeOut")) return;
		acceptChallengeCallback = call;
		if (!Util.NetAvailable)
		{
			net_acceptChallengeEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/acceptchallenge";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_acceptChallenge rq = new rq_acceptChallenge();
		rq.playerid = Player.ID;
		rq.result.mail_id = mailId;
		rq.result.integral = integral;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_acceptChallengeEvent, args);
		Invoke("OnAcceptChallengeCallTimeOut", TIMEOUT_TIME);
	}

	void OnAcceptChallengeCallTimeOut()
	{
		Debuger.Log("接受挑战请求超时...");
		net_acceptChallengeEvent("");
	}

	/// <summary>
	/// 接受挑战回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_acceptChallengeEvent(string json)
	{
		CancelInvoke("OnAcceptChallengeCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("接受挑战回调:" + json);
		rqback_acceptChallenge rqback = JsonMapper.ToObject<rqback_acceptChallenge>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (acceptChallengeCallback != null) acceptChallengeCallback(succ);
		acceptChallengeCallback = null;
	}

	#endregion

	#region 赠送

	private CallBackWithParams<bool> giveCallback;

	/// <summary>
	/// 赠送
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	public void giveCall(CallBackWithParams<bool> call, int playerId, int propId, int num = 1)
	{
		if (IsInvoking("OnGiveCallTimeOut")) return;
		giveCallback = call;
		if (!Util.NetAvailable)
		{
			net_giveEvent("");
			return;
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/handsel";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_give rq = new rq_give();
		rq.playerid = Player.ID;
		rq.result.to_player_id = playerId;

		rq_give_result_attachment attachment = new rq_give_result_attachment();
		attachment.goods_id = propId;
		attachment.goods_num = num;
		rq.result.attachment = JsonMapper.ToJson(attachment);

		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_giveEvent, args);
		Invoke("OnGiveCallTimeOut", TIMEOUT_TIME);
	}

	void OnGiveCallTimeOut()
	{
		Debuger.Log("赠送请求超时...");
		net_giveEvent("");
	}

	/// <summary>
	/// 赠送回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_giveEvent(string json)
	{
		CancelInvoke("OnGiveCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("赠送回调:" + json);
		rqback_give rqback = JsonMapper.ToObject<rqback_give>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (giveCallback != null) giveCallback(succ);
		giveCallback = null;
	}

	#endregion

	#region 求助

	private CallBackWithParams<bool> seekHelpCallback;

	/// <summary>
	/// 求助
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="propId">Property identifier.</param>
	public void seekHelpCall(CallBackWithParams<bool> call, int playerId, int propId)
	{
		if (IsInvoking("OnSeekHelpCallTimeOut")) return;
		seekHelpCallback = call;
		if (!Util.NetAvailable)
		{
			net_seekHelpEvent("");
			return;	
		}
		UIController.Instance.HideWaiting();
		string interfaceName = "player/seekhelp";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_seekHelp rq = new rq_seekHelp();
		rq.playerid = Player.ID;
		rq.result.to_player_id = playerId;

		rq_give_result_attachment attachment = new rq_give_result_attachment();
		attachment.goods_id = propId;
		attachment.goods_num = 1;
		rq.result.attachment = JsonMapper.ToJson(attachment);

		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_seekHelpEvent, args);
		Invoke("OnSeekHelpCallTimeOut", TIMEOUT_TIME);
	}

	void OnSeekHelpCallTimeOut()
	{
		Debuger.Log("求助请求超时...");
		net_seekHelpEvent("");
	}

	/// <summary>
	/// 求助回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_seekHelpEvent(string json)
	{
		CancelInvoke("OnSeekHelpCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("求助回调:" + json);
		rqback_seekHelp rqback = JsonMapper.ToObject<rqback_seekHelp>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		}
		if (seekHelpCallback != null) seekHelpCallback(succ);
		seekHelpCallback = null;
	}

	#endregion

	#region 答谢

	private CallBackWithParams<bool> thanksCallback;

	/// <summary>
	/// 答谢
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="propId">Property identifier.</param>
	public void thanksCall(CallBackWithParams<bool> call, int playerId, int propId)
	{
		if (IsInvoking("OnThanksCallTimeOut")) return;
		thanksCallback = call;
		if (!Util.NetAvailable)
		{
			net_thanksEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/acknowledge";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_thanks rq = new rq_thanks();
		rq.playerid = Player.ID;
		rq.result.to_player_id = playerId;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_thanksEvent, args);
		Invoke("OnThanksCallTimeOut", TIMEOUT_TIME);
	}

	void OnThanksCallTimeOut()
	{
		Debuger.Log("答谢请求超时...");
		net_thanksEvent("");
	}

	/// <summary>
	/// 答谢回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_thanksEvent(string json)
	{
		CancelInvoke("OnThanksCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("答谢回调:" + json);
		rqback_thanks rqback = JsonMapper.ToObject<rqback_thanks>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;	
		}
		if (thanksCallback != null) thanksCallback(succ);
		thanksCallback = null;
	}

	#endregion

	#region 碎片合成

	private CallBackWithParams<bool> fragmentssyntheticCallback;

	/// <summary>
	/// 碎片合成
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="propId">Property identifier.</param>
	public void fragmentssyntheticCall(CallBackWithParams<bool> call, int fragment_id, int num = 1)
	{
		if (IsInvoking("OnFragmentssyntheticCallTimeOut")) return;
		fragmentssyntheticCallback = call;
		if (!Util.NetAvailable)
		{
			net_fragmentssyntheticEvent("");
			return;	
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "player/fragmentssynthetic";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_fragmentssynthetic rq = new rq_fragmentssynthetic();
		rq.playerid = Player.ID;
		rq.result.fragment_id = fragment_id;
		rq.result.num = num;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_fragmentssyntheticEvent, args);
		Invoke("OnFragmentssyntheticCallTimeOut", TIMEOUT_TIME);
	}

	void OnFragmentssyntheticCallTimeOut()
	{
		Debuger.Log("碎片合成请求超时...");
		net_fragmentssyntheticEvent("");
	}

	/// <summary>
	/// 碎片合成回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_fragmentssyntheticEvent(string json)
	{
		CancelInvoke("OnFragmentssyntheticCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("碎片合成回调:" + json);
		JsonData jd = JsonMapper.ToObject(json);
		bool succ = false;
		if (!string.IsNullOrEmpty(json) && jd != null && (int)jd["status"] == 1)
		{
			succ = true;
		}
		if (fragmentssyntheticCallback != null) fragmentssyntheticCallback(succ);
		fragmentssyntheticCallback = null;
	}

	#endregion

	#region 邀请码

	private CallBackWithParams<bool,int> invitationcodeCallback;

	/// <summary>
	/// 兑换礼物
	/// </summary>
	/// <param name="callback">Callback.</param>
	/// <param name="codeStr">Code string.</param>
	public void invitationcodeCall(CallBackWithParams<bool,int> callback, string codeStr)
	{
		if (IsInvoking("OnInvitationcodeCallTimeOut")) return;
		invitationcodeCallback = callback;
		if (!Util.NetAvailable)
		{
			net_invitationcodeEvent("");
			return;		
		}
		UIController.Instance.ShowWaiting();
		string interfaceName = "app/invitationcode";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_invitationcode rq = new rq_invitationcode();
		rq.playerid = Player.ID;
		rq.result.code = codeStr;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_invitationcodeEvent, args);
		Invoke("OnInvitationcodeCallTimeOut", TIMEOUT_TIME);
	}

	void OnInvitationcodeCallTimeOut()
	{
		Debuger.Log("邀请码请求超时...");
		net_invitationcodeEvent("");
	}

	void net_invitationcodeEvent(string json)
	{
		CancelInvoke("OnInvitationcodeCallTimeOut");
		UIController.Instance.HideWaiting();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("邀请码回调:" + json);
		JsonData jd = JsonMapper.ToObject(json);
		bool succ = false;
		int rewardId = 0;
		if (!string.IsNullOrEmpty(json) && jd != null && (int)jd["status"] == 1)
		{
			rewardId = (int)jd["reward_id"];
			succ = true;
		}
		if (invitationcodeCallback != null) invitationcodeCallback(succ, rewardId);
		invitationcodeCallback = null;
	}

	#endregion

	#region 心跳

	private CallBackWithParams<bool,rqback_heartbeat> heartbeatCallback;

	/// <summary>
	/// 心跳
	/// </summary>
	/// <param name="call">Call.</param>
	public void heartbeatCall(CallBackWithParams<bool,rqback_heartbeat> call)
	{
		heartbeatCallback = call;

		string interfaceName = "app/heartbeat";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_heartbeat rq = new rq_heartbeat();
		rq.playerid = Player.ID;
		rq.result = "";
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey("\"\"", rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_heartbeatEvent, args);
	}

	/// <summary>
	/// 心跳回调
	/// </summary>
	/// <param name="json">Json.</param>
	void net_heartbeatEvent(string json)
	{
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("心跳回调:" + json);
		rqback_heartbeat rqback = JsonMapper.ToObject<rqback_heartbeat>(json);
		bool succ = false;
		if (rqback != null && rqback.status == 1)
		{
			succ = true;
		}
		if (heartbeatCallback != null) heartbeatCallback(succ, rqback);
		heartbeatCallback = null;
	}

	#endregion

	#region 跑马灯

	private CallBackWithParams<bool,string[]> marqueeCallback;

	/// <summary>
	/// 跑马灯
	/// </summary>
	/// <param name="call">Call.</param>
	public void marqueeCall(CallBackWithParams<bool,string[]> call)
	{
		if (IsInvoking("OnMarqueeCallTimeOut")) return;
		marqueeCallback = call;
		if (!Util.NetAvailable)
		{
			net_marqueeEvent("");
			return;		
		}
		string interfaceName = "app/marqueeannouncement";
		m_url = string.Format("{0}{1}", CommonResource.URL, interfaceName);
		GetURL(m_url, net_marqueeEvent);
		Invoke("OnMarqueeCallTimeOut", TIMEOUT_TIME);
	}

	void OnMarqueeCallTimeOut()
	{
		Debuger.Log("跑马灯请求超时...");
		net_marqueeEvent("");
	}

	void net_marqueeEvent(string json)
	{
		CancelInvoke("OnMarqueeCallTimeOut");
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("跑马灯回调:" + json);
		rqback_marquee rqback = JsonMapper.ToObject<rqback_marquee>(json);
		bool succ = false;
		string[] queue = null;
		if (rqback != null && rqback.status == 1)
		{
			queue = rqback.result;
//			Array.Resize(ref queue,queue.Length+1);
//			queue.SetValue("即可来得及萨克的骄傲了",1);
			succ = true;
		}
		if (marqueeCallback != null) marqueeCallback(succ, queue);
		marqueeCallback = null;
	}

	#endregion

	#region 苹果内购

	private CallBackWithParams<bool> storekitCallback;

	/// <summary>
	/// 苹果内购
	/// </summary>
	/// <param name="call">Call.</param>
	/// <param name="data">Data.</param>
	public void storekitCall(CallBackWithParams<bool> call, rq_storekit_result data)
	{
		if (IsInvoking("OnStorekitCallTimeOut")) return;
		storekitCallback = call;
		if (!Util.NetAvailable)
		{
			net_storekitEvent("");
			return;		
		}
//		UIController.Instance.ShowWaiting ();
		string interfaceName = "app/payment";
		m_url = string.Format("{0}{1}?sid={2}", CommonResource.URL, interfaceName, PrefabManager.token);
		rq_storekit rq = new rq_storekit();
		rq.playerid = Player.ID;
		rq.result = data;
		rq.time = CommonResource.TimeUnix;
		rq.xsrf = Util.GetKey(JsonMapper.ToJson(rq.result), rq.time);
		string sb = JsonMapper.ToJson(rq);
		args = System.Text.Encoding.UTF8.GetBytes(sb);
		GetURL(m_url, net_storekitEvent, args);
		Invoke("OnStorekitCallTimeOut", TIMEOUT_TIME);
	}

	void OnStorekitCallTimeOut()
	{
		Debuger.Log("内购请求超时...");
		net_storekitEvent("");
	}

	void net_storekitEvent(string json)
	{
		CancelInvoke("OnStorekitCallTimeOut");
//		UIController.Instance.HideWaiting ();
		json = json.Replace("\"result\":\"\"", "\"result\":null");
		Debuger.Log("内购回调:" + json);
		JsonData jd = JsonMapper.ToObject(json);
		bool succ = false;
		if (jd != null && (int)jd["status"] == 1)
		{
			succ = true;
		}
		if (storekitCallback != null) storekitCallback(succ);
		storekitCallback = null;
	}

	#endregion
}


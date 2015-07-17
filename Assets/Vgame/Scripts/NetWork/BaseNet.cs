using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

public class BaseNet : MonoBehaviour
{
	public Action NotNetConnect;

	protected Action<string> connectionAction;

	private float timeoutvalue = 3;

	protected string m_url = "";

	protected byte[] args;

	protected MultipleLoader mLoader;
	WWW www;

	void Awake ()
	{
		DontDestroyOnLoad (this);

		mLoader = MultipleLoader.GetInstance ();
	}

	void timeout ()
	{
		NotNetConnect ();
	}

	public IEnumerator GetConnect (requestType type)
	{
		Invoke ("timeout", timeoutvalue);

		if (type == requestType.POST)
		{
			www = new WWW (m_url, args);
		}
		else
		{
			www = new WWW (m_url);
		}
        
		yield return www;
		if (www.error != null)
		{
		}
		else
		{
			CancelInvoke ("timeout");
			if (this.connectionAction != null)
			{
				connectionAction (www.text);
			}
		}    
	}

	/// <summary>
	/// 获取url 后回掉callBack
	/// </summary>
	/// <param name="url"></param>
	/// <param name="callBack"></param>
	/// <param name="args"></param>
	public void GetURL (string url, System.Action<String> callBack, byte[] args = null)
	{
		CRequest req = new CRequest (url);
		req.OnComplete += req_OnComplete;
		req.OnEnd += req_OnEnd;
		req.head = args;
		req.userData = callBack;
		mLoader.LoadReq (req);
	}

	void req_OnEnd (CRequest req)
	{
		((System.Action<String>)req.userData) (string.Empty);//回掉
	}

	void req_OnComplete (CRequest req)
	{
		WWW ww = req.data as WWW;
		string context = string.Empty;
		if (ww != null)
		{
			context = ww.text;
		}
		//Debug.Log("url "+req.url+":"+context);
		((System.Action<String>)req.userData) (context);//回掉

		//throw new NotImplementedException();
	}
}

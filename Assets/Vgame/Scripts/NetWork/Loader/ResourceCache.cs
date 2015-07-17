using UnityEngine;
using System.Collections.Generic;
using System;

public class ResourceCache  {
	
	public ResourceCache()
	{
		resdic=new Dictionary<string, object>();
        multipleLader = MultipleLoader.GetInstance();
		multipleLader.OnAllComplete+=OnAllCompleteHandle;
		multipleLader.OnSharedComplete+=OnSharedCompleted;
		multipleLader.OnProgress+=OnProgressHandle;
        multipleLader.cache = this.resdic;
	}
	
	private void OnAllCompleteHandle(MultipleLoader mLoader)
	{
		if(OnAllComplete!=null)
			OnAllComplete(mLoader);
		
		OnAllComplete=null;
	}

	private void OnProgressHandle(MultipleLoader mulitLoader,LoaderEventArg arg)
	{
		if(OnProgress!=null)
			OnProgress(mulitLoader,arg);
	}

	private void OnSharedCompleted(CRequest req)
	{
		Debug.Log(" shared url load "+req.url);
		if(OnSharedComplete!=null)
			OnSharedComplete(req);		
	}

	public event OnAllCompleteHandle OnAllComplete;
	public event OnProgressHandle	OnProgress;
	public event CompleteHandle OnSharedComplete;

	/// <summary>
	/// Gets the resource.
	/// </summary>
	/// <returns>
	/// The resource.
	/// </returns>
	/// <param name='url'>
	/// URL.
	/// </param>
	/// <param name='oncomplete'>
	/// Oncomplete.
	/// </param>
	/// <param name='priority'>
	/// Priority.
	/// </param>
	/// <param name='head'>
	/// Head.
	/// </param>
	public void GetResource(string url,CompleteHandle oncomplete,int priority=0,object head=null,bool cache=true)
	{
		CRequest req=new CRequest(url,priority);
		req.head=head;
		req.OnComplete+=oncomplete;
		GetResource(req,cache);
	}
	
	/// <summary>
	/// Gets the resource.
	/// </summary>
	/// <param name='url'>
	/// URL.
	/// </param>
	/// <param name='oncomplete'>
	/// Oncomplete.
	/// </param>
	/// <param name='onError'>
	/// On error.
	/// </param>
	/// <param name='priority'>
	/// Priority.
	/// </param>
	/// <param name='head'>
	/// Head.
	/// </param>
	/// <param name='cache'>
	/// Cache.
	/// </param>
	public void GetResource(string url,CompleteHandle oncomplete,CompleteHandle onError,int priority=0,object head=null,bool cache=true)
	{
		CRequest req=new CRequest(url,priority);
		req.head=head;
		req.OnComplete+=oncomplete;
		req.OnEnd+=onError;
		GetResource(req,cache);
	}
	
	/// <summary>
	/// Gets the resource.
	/// </summary>
	/// <param name='req'>
	/// Req.
	/// </param>
	/// <param name='oncomplete'>
	/// Oncomplete.
	/// </param>
	/// <param name='priority'>
	/// Priority.
	/// </param>
	/// <param name='head'>
	/// Head.
	/// </param>
	public void GetResource(CRequest req,bool cache=true)
	{

		string key=req.key;
        req.cache = cache;
		if(cache && resdic.ContainsKey(key))
		{
			req.data=resdic[key];
			req.DispatchComplete();
			return;
		}
		multipleLader.LoadReq(req);
	}	
	
	public void GetResource(IList<CRequest> reqs,bool cache=true)//,int priority=0,object head=null
	{
//		multipleLader.loadReq(reqs);
		foreach(CRequest req in reqs)
		{
			GetResource(req,cache);
		}
	}
	
	public void Unload(string url)
	{
		if(!System.String.IsNullOrEmpty(url))
		{
			string key=CUtils.GetURLFullFileName(url);
			Clear(key);
		}
	}
	
	public void Clear(string key)
	{
		if(resdic.ContainsKey(key))
		{
			object o=resdic[key];
			if(o is WWW)
			{
				WWW ww=o as WWW;
				if(ww.assetBundle!=null)ww.assetBundle.Unload(false);
				ww.Dispose();
				ww=null;
			}
			resdic.Remove(key);
		}
	}
	
	public void Clear()
	{
		foreach(string key in resdic.Keys)
		{
			Clear(key);
		}
		
		resdic.Clear();
	}
	
	public MultipleLoader multipleLader;
	
	public IDictionary<string,object> resdic;
	
	private static ResourceCache _resourcecach;
	
	public static ResourceCache instance
	{
		get{
		
			if(_resourcecach==null)
			{
				_resourcecach=new ResourceCache();
			}
			
			return _resourcecach;
		}
	}
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;

public class MultipleLoader  {
	
	public MultipleLoader()
	{
		requestCallBackList=new Dictionary<string,List<CRequest>>();
		queue=new CQueueRequest();
		loader=new Dictionary<string,CLoader>();
		maxLoading=2;
		loadingEvent=new LoaderEventArg();
		loaderPool=new List<GameObject>();
	}
	
		/**
	   * 将多个资源加载到本地，不放入资源缓存。</br>
	   * 参数:</br>
	   * <b>req:Array</b> 请求的队列Request类型  </br>
	   * <b>onComplete:Function(e:Multiple):void</b> 全部加载完成时回调函数  </br>
	   * <b>onProgress：Function(e:ProgressEvent):void</b> 加载进度调用函数  </br>
	   */
	  public void LoadReq(IList<CRequest>  req)//onAllCompleteHandle onAllCompletehandle=null,onProgressHandle onProgresshandle=null
	  {
          for(int i=0;i<req.Count;i++)
              AddReqToQueue(req[i]);
			BeginQueue();
	  }

      //public void LoadReq(ArrayList reqs)
      //{
      //    foreach (CRequest reqi in reqs)
      //    {
      //        AddReqToQueue(reqi);
      //    }
      //    BeginQueue();
      //}
	
	public void LoadReq(CRequest req)
	{
        AddReqToQueue(req);
        BeginQueue();
	}

    protected void AddReqToQueue(CRequest req)
	{
		string key=req.udKey;
		if(requestCallBackList.ContainsKey(key))
		{
			requestCallBackList[key].Add(req);
		}else
		{
			requestCallBackList[key]=new List<CRequest>();
			requestCallBackList[key].Add(req);
			queue.Add(req);
			totalLoading++;
		}
	}
	
	public void InitProgressState()
	{
		if (this.currentLoading<=0)
		{
			totalLoading=0;
		  	//this.totalLoading=this.queue.size();
			this.currentWillLoading=0;
			this.currentLoaded=0;
            this.loadingEvent.current = currentLoaded;
		}else
		{
			//this.totalLoading=totalLoading+this.queue.size();
		}
	}

    protected void BeginQueue()
	{
		CRequest req1;
		while(this.currentLoading<this.maxLoading && queue.Size()>0)
		{
			req1=queue.First();
			if(req1!=null)
			{
				LoadRequest(req1);
			}
		}
	}

    protected void LoadRequest(CRequest req)
	{
		req.times++;
		_currentLoading++;
		currentWillLoading++;
		
		string key=req.udKey;
//		string name=key+".CLoader";
		GameObject loaderparent= this.GetFreeLoader(); // new GameObject();
		CLoader load=loaderparent.AddComponent<CLoader>();
		load.req=req;
		load.key=key;
		load.OnComplete+=LoadComplete;
		load.OnError+=LoadError;
		load.OnWaitDependencies+=WaitDependencies;
		load.OnDependencyComplete+=OnDependencyComp;
		load.OnProcess+=OnProcess;
		loader[key]=load;
		load.number=currentWillLoading;
//#if UNITY_EDITOR
//		Debug.Log("-----------beginLoad <<:"+key+">>  shared="+req.isShared+" now:"+CUtils.getDateTime()+" ,currentLoading="+this.currentLoading+"  max="+this.maxLoading);
//#endif
		load.BeginLoad();

	}
	
	protected void RemoveRequest(CRequest req)
	{
		string key=req.udKey;
		CLoader load =loader[key];
		loader.Remove(key);
		load.Dispose();
		load=null;
		if(requestCallBackList.ContainsKey(key))
		{
			requestCallBackList[key].Clear();
			requestCallBackList.Remove(key);
		}
	
	}
	
	#region interface

    protected void OnProcess(CLoader loader, float progress)
	{
		loadingEvent.target=this;
		loadingEvent.total=totalLoading;
        if(loadingEvent.current<currentLoaded)loadingEvent.current=currentLoaded;
		loadingEvent.progress=progress;
		loadingEvent.number=loader.number;
		if(OnProgress!=null && totalLoading>0)
		{
			OnProgress(this,loadingEvent);
		}
	}
	
	protected virtual void WaitDependencies(CLoader loader,IList<CRequest> reqs)
	{
        //CRequest req = loader.req;
        string key = "";
        IList<CRequest> reqsAdd = new List<CRequest>();
        IDictionary<string, object> resdic = this._cache;// req.cache as IDictionary<string, object>;
        //foreach (CRequest item in reqs)
        for(int i=0;i<reqs.Count;i++)
        {
            CRequest item = reqs[i];
            key = item.key;
            //item.cache = resdic;
            if(resdic!=null && resdic[key]!=null )//.ContainsKey(key))
            {
                item.data=resdic[key];
                item.DispatchComplete();
                continue;
            }
            reqsAdd.Add(item);
        }

		loader.isWait=true;
		_currentLoading--;
        this.LoadReq(reqsAdd);
	}

    protected GameObject GetFreeLoader()
	{
		GameObject loader=null;
		
		if(loaderPool.Count<_maxLoading)
		{
			for(int i=loaderPool.Count;i<_maxLoading;i++)
			{
				GameObject obj=new GameObject("loader"+i);
				MonoBehaviour.DontDestroyOnLoad(obj);
				loaderPool.Add(obj);
			}
		}
		
		for(int i=0;i<_maxLoading;i++)
		{
			loader=this.loaderPool[i];
			loader.name="loader"+i;
			CLoader cloader=loader.GetComponent<CLoader>();
			if(cloader==null)
				return loader;
		}
		
		if(loader==null)
		{
			loader=new GameObject("loader0");
			MonoBehaviour.DontDestroyOnLoad(loader);
			loaderPool.Add(loader);
		}
		return loader;
	}

    protected void OnDependencyComp(CRequest req)
    {

	}
	
	protected virtual void LoadComplete(CLoader loader)
	{
		CRequest req=loader.req;
//#if UNITY_EDITOR
//		if(req.isShared)
//			Debug.Log("_______loadComplete  <<"+req.key+">> is dependencyItem  thread="+Thread.CurrentThread.GetHashCode()+" now:"+CUtils.getDateTime());
//		else
//			Debug.Log("______loadComplete  <<"+req.key+">> now:"+CUtils.getDateTime());
//#endif
		if(loader.isWait==false)
			_currentLoading--;
		
		currentLoaded++;

		object data=req.data;

        if ((req.isShared || req.cache) && this._cache!=null) this._cache[req.key] = data;
			
		IList<CRequest> callbacklist=requestCallBackList[req.udKey];
		if(callbacklist!=null)
		{
			requestCallBackList.Remove(req.udKey);
			int count=callbacklist.Count;
			CRequest reqitem;
			for(int i=0;i<count;i++)// reqitem in  callbacklist)
			{
				reqitem=callbacklist[i];
				reqitem.data=data;
				reqitem.DispatchComplete();
			}
		}

		RemoveRequest(req);

		BeginQueue();

		CheckAllComplete();	

		if(req.isShared && OnSharedComplete!=null)OnSharedComplete(req);
		
	}
	
	protected void CheckAllComplete()
	{
		if(currentLoading<=0)
		{
			loadingEvent.target=this;
			loadingEvent.total=totalLoading;
			loadingEvent.progress=1;
			loadingEvent.current=currentLoaded;
			loadingEvent.number=totalLoading;
			if(OnProgress!=null && totalLoading>0)
			{
				OnProgress(null,loadingEvent);
			}
			
			if(this.OnAllComplete!=null)
				this.OnAllComplete(this);
			
			//Debug.Log("AllComplete"+currentLoading);
			//initProgressState();
		}

	}
	
	public void LoadError(CLoader cloader)
	{
		_currentLoading--;
		CRequest req=cloader.req;
		
		RemoveRequest(req);		
#if	UNITY_EDITOR
		Debug.LogWarning("load Error : times="+req.times+" url="+req.url);
#endif
		if(req.times<2)
		{
			this.AddReqToQueue(req);
			this.BeginQueue();
		}else
		{
			req.DispatchEnd();
			BeginQueue();		
			CheckAllComplete();	
		}
		
	}
	
	#endregion

    protected CQueueRequest queue;

    protected IDictionary<string, List<CRequest>> requestCallBackList;
	
	protected IDictionary<string,CLoader> loader;
		
	#region memeber
	
	public int currentWillLoading{private set;get;}	
	
	protected int _currentLoading;
	
	public int currentLoading{
		get{
				return _currentLoading;
		}
	}
	
	private int _maxLoading;
	
	public int maxLoading{
		get{
				return _maxLoading;
		}
        protected set
		{
			_maxLoading=value;
		}
	}
	
	private int _totalLoading=0;
	
	public int totalLoading{
		get{
				return _totalLoading;
		}
		set
		{
			_totalLoading=value;
		}
	}
	private int _currentLoaded=0;
	
	public int currentLoaded{
		get{
			return _currentLoaded;
		}
		protected set
		{
			_currentLoaded=value;
		}
	}

    protected LoaderEventArg loadingEvent;
    protected IList<GameObject> loaderPool;

    private IDictionary<string, object> _cache;

    public virtual object cache { 
        get{return _cache;}
        set{
            if(value is IDictionary)
             _cache=(IDictionary<string, object>)value;
        }
    }

	#endregion

	#region  delegate and event
	
	public event OnAllCompleteHandle OnAllComplete;
	public event OnProgressHandle	OnProgress;
	public event CompleteHandle OnSharedComplete;
	#endregion
	
    private static MultipleLoader _instance;

    public static MultipleLoader GetInstance()
    {
        if (_instance == null)
            _instance = new MultipleLoader();
        return _instance;
    }
    
}

public class LoaderEventArg
{
	//public object loader;
	public int number;//current loading number
	public object target;
	public int total;
	public int current;
	public float progress;
}

public delegate void OnAllCompleteHandle(MultipleLoader loader);
public delegate void OnProgressHandle(MultipleLoader loader,LoaderEventArg arg);
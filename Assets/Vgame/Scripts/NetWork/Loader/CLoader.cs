using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System;

public class CLoader : MonoBehaviour {
	
	public CLoader()
	{
		
	}
	
	public void BeginLoad()
	{
		isDone=false;
	 	StartCoroutine(Loadres());
	}
	
	void Update () {
		
		if(www!=null && isDone==false)
		{

			float pro=www.progress;

			if(OnProcess!=null)
				OnProcess(this,pro);
		}
	}
	
	IEnumerator Loadres()
	{
		beginT=DateTime.Now;
		if(req.head is WWWForm)
		{
			www=new WWW(req.url,(WWWForm)req.head);
		}else if(req.head is byte[])
        {
            www = new WWW(req.url, (byte[])req.head);
        }
        else
		{
			www=new WWW(req.url); //WWW.LoadFromCacheOrDownload(req.url,1);  
		}
		yield return www;
		isDone=true;
		endT=DateTime.Now;
		TimeSpan ds=endT-beginT;
		castTime=ds.TotalMilliseconds;
		if(www.error!=null)
		{
			Debug.LogWarning("error : url "+www.url+" \n error:"+www.error);
			DispatchErrorEvent();
#if UNITY_EDITOR
			//Debug.LogError("url "+www.url+" \n error:"+www.error );
#endif
			
		}else
		{
			if(OnProcess!=null)
				OnProcess(this,1);

            if (req.isShared)
            {
                req.data = www;
                var asset =www.assetBundle.mainAsset;
                // asset = null;
               // AssetBundleCreateRequest ab = AssetBundle.CreateFromMemory(www.bytes);
               // yield return ab;
               // req.data = ab;
               //var aseet= ab.assetBundle.mainAsset;
#if UNITY_EDITOR
                Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~isShared~" + req.key + "~~~~~~->" + req.url);
#endif
                //                www.assetBundle.Unload(true);
//                www.Dispose();
                this.DispatchCompleteEvent();
            }
            else
            {
                try
                {
                    req.data = www;

                    //if (req.suffix.Equals("u3d")
                    //    && www.assetBundle != null
                    //    && www.assetBundle.Contains(Common.DEPENDENCIES_OBJECT_NAME)
                    //)
                    //{
                    //    GameObject gameob = (GameObject)www.assetBundle.Load(Common.DEPENDENCIES_OBJECT_NAME);
                    //    CDependencies script = gameob.GetComponent<CDependencies>();
                    //    BeginLoadDependencies(script);
                    //}
                    //else
                    //{
                        this.DispatchCompleteEvent();
                    //}
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    if (www != null) Debug.Log("log Error:" + req.url);
#endif
                    Debug.LogError(e);
                }

            }

			
		}
    }

    //private void BeginLoadDependencies(CDependencies script)
    //{
	
    //    string paths=script.paths;
    //    string[] strs=paths.Split(',');

    //    IList<CRequest> reqs=new List<CRequest>();
    //    CRequest item;
    //    _dependencies=0;_curretDenp=0;
    //    string depstr="";
    //    int priority=this.req.priority+1;
    //    _dependencies=strs.Length;
    //    foreach(string p in strs)
    //    {
    //        item=new CRequest(CUtils.GetFileFullPath(p),priority);
    //        item.isShared=true;
    //        item.OnComplete+=OnDependenItemComplete;
    //        item.OnEnd+=OnDependenItemComplete;
    //        reqs.Add(item);
    //        depstr+=" "+item.key;
    //    }
    //    this.beginT=DateTime.Now;
    //    if(reqs.Count>0)
    //    {
    //        DispatchWaitDependenciesEvent(reqs);
    //    }
    //}
	
    //private void OnDependenItemComplete(CRequest reqd)
    //{
    //    _curretDenp++;

    //    DispatchOnDependencyComplete(reqd);
		
    //    if(_curretDenp>=_dependencies)
    //    {
    //        this.endT=DateTime.Now;
    //        TimeSpan ts=this.endT-this.beginT;
    //        castTime=ts.TotalMilliseconds;
    //        this.DispatchCompleteEvent();
    //    }
    //}
	
	
	public void Dispose()
	{
		OnProcess=null;
		OnComplete=null;
		OnError=null;
		OnWaitDependencies=null;
		req=null;
		www=null;
		Destroy(this);
	}
	
	#region dispatchEvent
	
	private void DispatchCompleteEvent()
	{
		if(OnComplete!=null)
			OnComplete(this);
		//Resources.UnloadUnusedAssets();
	}
		

	private void DispatchErrorEvent()
	{
		if(OnError!=null)
			OnError(this);
	}
	
	private void DispatchWaitDependenciesEvent(IList<CRequest> reqs)
	{
		if(OnWaitDependencies!=null)
		{
			OnWaitDependencies(this,reqs);
		}
	}
	
	
	private void DispatchOnDependencyComplete(CRequest req)
	{
		if(OnDependencyComplete!=null)
			OnDependencyComplete(req);
	}
	
	#endregion
	
	private DateTime beginT;
	private DateTime endT;
	
	/// <summary>
	/// The cast time .
	/// </summary>
	public double castTime; 
	public int number;
	public CRequest req;
	public string key;
	public bool isDone{private set;get;}
	
	private WWW www;	
	private int _dependencies=0;
	private int _curretDenp=0;
	
	public int curretDenp
	{
		get{
			return _curretDenp;
		}
	}
	
	public int dependencies
	{
		get{
			return _dependencies;
		}
	}
	
	public bool isWait=false;
	
	#region event
	public event ProcessHandle OnProcess;
	public event LoadCompleteHandle OnComplete;
	public event LoadErroHandle OnError;
	public event WaitDependenciesHandle OnWaitDependencies;
	public event DependencyCompleteHandle OnDependencyComplete;
	#endregion
	
}

	#region delegate
	public delegate void WaitDependenciesHandle(CLoader loader,IList<CRequest> reqs);
	public delegate void LoadCompleteHandle(CLoader loader);
	public delegate void LoadErroHandle(CLoader loader);
	public delegate void ProcessHandle(CLoader loader,float progress);
	public delegate void DependencyCompleteHandle(CRequest req);
	#endregion
	
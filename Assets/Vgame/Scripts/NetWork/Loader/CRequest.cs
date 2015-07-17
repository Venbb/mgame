using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Request.
/// </summary>
/**
  * 加载资源的请求
  */
 public class CRequest // IDispose
 {
  /**
   * 加载请求</br>
   * 参数:</br>
   * <b>url:*</b> 文件地址 可是二进制</br>
   * <b>onComplete:Function(data:*,key:String)</b> 加载完成后的回调函数 参数为二个data加载的数据,key加载是用的key</br>
   * key:文件名</br>
   * <b>priority:int</b> 优先等级  值越大优先级越高
   */
  public CRequest(string url,int priority=0,string key="",string type="")
  {
   this.url=url;
   this.priority=priority;
   this.key=key;
   this.suffix=type;
  }


  public void Dispose()
  {
   this.url=null;
   this.priority=0;
   this.key=null;
   this.suffix=null;
   _data=null;
   _head=null;
  }
	  
  private object _data;
  
  private object _head;
  
  private string _key,_udKey;
  
  private string  _relative;
  
  private string _domain="";
  
  /**
   * 加载请求
   */
  private string _url;
  
  private string _suffix;
  
 // private IDictionary<string,object> _cache;
  private bool _cache = false;
  /**
   * 文件后缀类型</br>
   * 默认根据url后缀来生成</br>
   * 如果设定以设定值为准.解码的时候是根据type来解码的.例如 解码后成相应的对象类型
   */
  public string suffix
  {
		get{
		   if(_suffix=="") 
		    	_suffix=CUtils.GetURLFileSuffix(_url);
		   return _suffix;
		}
		set
		{
			_suffix = value;
		}
  }


  /**
   * 加载的头信息
   */
  public object head
	{
		get{return this._head;}
		set{this._head=value;}
	}


  /**
   * 加载的数据
   */
  public  object data
  {
		get{
   			return _data;
		}
		set{
			_data = value;
		}
  }
	/// <summary>
	/// The user data.
	/// </summary>
	public object userData;
	
	public event CompleteHandle OnEnd;
	
	public event CompleteHandle OnComplete;
	
	public void DispatchComplete()
	{
		if(OnComplete!=null)
			OnComplete(this);
	}
	
	public void DispatchEnd()
	{
		if(OnEnd!=null)
			OnEnd(this);		
	}
	
	public bool isShared{get;set;}

  /**
   * 加载完成回调</br>
   * <b>onComplete:Function(req:Request)</b> 
   * 加载完成后的回调函数
   * 参数为为当前请求</br>
   */
 // public event onComplete;
  
  /**
   * 重实n次加载失败后回调</br>
   * <b>onError:Function(req:Request)</b> 
   * 加载完成后的回调函数
   * 参数为为当前请求</br>
   */
  //public var onEnd:Function;
  
  /**
   * 请求地址 网址，相对路径，绝对路径
   */
  public  string url
  {
		get{
   			return _url;
		}
		set
		{
				 _url = value;
			/**
			   if(_url.indexOf(":")>0)//如果有协议 http:// app:/  app-storage:/
			   {
			    var di=_url.indexOf("/");
			    if(_url.toLocaleLowerCase().indexOf("http")==0)
			    {
			     di=_url.indexOf("/",di+1);
			     di=_url.indexOf("/",di+1);
			    }
			    var http=_url.substring(0,di+1);
			    var context=_url.substring(di+1,_url.length);
			    context=context.split("?")[0];
			    relative=context;
			    _domain=http;
			   }else
			   {
			    this.relative=_url.split("?")[0];
			    
			   }
			   */
		}
  }



  /**
   *缓存用的关键字
   * 如果没有设定 则为默认key生成规则
   */
  public string key
  {
		get
		{
		   if(string.IsNullOrEmpty(_key))
               _key = CUtils.GetKeyURLFileName(url);		    	
		   return _key;
		}
		set
		{
			_key = value;
		}
  }
	
/// <summary>
/// Sets the U dkey.
/// </summary>
/// <value>
/// The U dkey.
/// </value>
public string udKey
{
	get
		{
		   if(string.IsNullOrEmpty(_udKey))
				_udKey=CryptographHelper.CrypfString(this.url);//_key=CUtils.getURLFullFileName(url);		    	
		   return _udKey;
		}
		set
		{
			_udKey = value;
		}
}


  
  /**
   * 相对根目录路径
   */
  public string relative
  {
		get
		{
   			return _relative;
		}
		set
		{
			_relative = value;
		}
  }

  
  /**
   * 当前请求的域名 在设置url属性的时候自动设置
   */
  public string domain
  {
		get{
   			return _domain;
		}
  }
  
  /**
   * 缓存对应的字典类型
   */
  //public IDictionary<string,object> cache
  public bool cache
  {
		get
		{
  			 return _cache;
		}
		set
		{
			_cache=value;
		}
  }


  //回调
  /**
   * 优先等级
   * 降序</br>
   *  <b>priority:int</b> 优先等级  值越大优先级越高
   */
  public int priority=0;//优先级
  
  /**
   * 加载次数
   * 加载失败的时候记录
   */
  public int times=0;
 }

public delegate  void CompleteHandle(CRequest req);

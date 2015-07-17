
namespace Vgame
{
	public delegate void CallBack ();
	public delegate void CallBackWithParams<T> (T t);
	public delegate void CallBackWithParams<T,U> (T t,U u);
	public delegate void CallBackWithParams<T,U,V> (T t,U u,V v);
}
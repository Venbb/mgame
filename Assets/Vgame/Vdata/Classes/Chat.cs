using System;
using System.Collections;
using System.Collections.Generic;
using Vgame.Data;

public class Chat:DataObject
{
	/// <summary>
	/// 喵星人ID
	/// </summary>
	public int Cat_id;
	/// <summary>
	/// 喵星人名字
	/// </summary>
	public string Cat_name;
	/// <summary>
	/// 类型
	/// </summary>
	public int Bullet_type;
	/// <summary>
	/// 一次发射数量
	/// </summary>
	public int Shoot_num;
	/// <summary>
	/// 初始子弹数量
	/// </summary>
	public int Bullet_num;
	/// <summary>
	/// 子弹变大体积倍数
	/// </summary>
	public int Bullet_volume;
	/// <summary>
	/// 子弹攻击力
	/// </summary>
	public int Bullet_atk;
	/// <summary>
	/// 子弹速度
	/// </summary>
	public int Bullet_speed;
	/// <summary>
	/// 子弹反弹次数
	/// </summary>
	public int Rebound;
	/// <summary>
	/// 穿透（子弹可穿透攻击敌人次数）
	/// </summary>
	public int Bullet_across;
	/// <summary>
	/// 蓄能点数
	/// </summary>
	public int Atk_pionts;
	/// <summary>
	/// 作用时间
	/// </summary>
	public int Atk_time;
	/// <summary>
	/// 攻击指引辅助
	/// </summary>
	public int Atk_point;
}

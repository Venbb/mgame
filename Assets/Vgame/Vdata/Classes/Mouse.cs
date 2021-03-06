using System;
using System.Collections;
using System.Collections.Generic;
using Vgame.Data;

public class Mouse:DataObject
{
	/// <summary>
	/// 老鼠ID
	/// </summary>
	public string Mouse_id;
	/// <summary>
	/// 名称
	/// </summary>
	public string Name;
	/// <summary>
	/// 类型
	/// </summary>
	public int Type;
	/// <summary>
	/// 防御力
	/// </summary>
	public float Def;
	/// <summary>
	/// 出现概率
	/// </summary>
	public float Probability;
	/// <summary>
	/// 移动速度
	/// </summary>
	public List<int> Move_speed;
	/// <summary>
	/// 移动类型:(1.左，2.右，3.上，4.曲线，5.特殊（跑出来又跑回去）)
	/// </summary>
	public Mouse_move_type Move_type;
	/// <summary>
	/// 掠夺奶酪几率
	/// </summary>
	public float Plunder_probability;
	/// <summary>
	/// 金币掉落概率
	/// </summary>
	public float Gold_probability;
}
public class Mouse_move_type
{
	public int Right;
	public int Left;
	public int Down;
	public int Curve;
}


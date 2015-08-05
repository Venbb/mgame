using UnityEditor;
using UnityEngine;

namespace Vgame.ToolKit.Editor
{
	public static class VgameMenu
	{
		[MenuItem ("Vgame/ToolKit/Excel/Excel To CSV")]
		static void ExcelToCSV ()
		{
			ExcelTools.ExcelToCSV ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To JSON")]
		static void ExcelToJSON ()
		{
			ExcelTools.ExcelToJSON ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To Class")]
		static void ExcelToClass ()
		{
			ExcelTools.ExcelToClass ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/")]
		static void ExcelBreaker ()
		{
			//分割线
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To Lua")]
		static void ExcelToLua ()
		{
			ExcelTools.ExcelToLua ();
		}

		[MenuItem ("Vgame/")]
		static void Breaker ()
		{
			//分割线
		}

		[MenuItem ("Vgame/Help")]
		static void Help ()
		{
			Application.OpenURL ("https://github.com/Venbb/mgame");
		}

		[MenuItem ("Assets/Vgame/Lua Script", false, 1)]
		static void CreateLuaScript ()
		{

		}
	}
}

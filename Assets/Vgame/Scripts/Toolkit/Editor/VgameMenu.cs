using UnityEditor;
using UnityEngine;

namespace VgameEditor
{
	public static class VgameMenu
	{
		[MenuItem ("Vgame/ToolKit/Excel/Excel To CSV")]
		static void ExcelToCSV ()
		{
			ExcelEditor.ExcelToCSV ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To JSON")]
		static void ExcelToJSON ()
		{
			ExcelEditor.ExcelToJSON ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To Class")]
		static void ExcelToClass ()
		{
			ExcelEditor.ExcelToClass ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/")]
		static void ExcelBreaker ()
		{
			//分割线
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To Lua")]
		static void ExcelToLua ()
		{
			ExcelEditor.ExcelToLua ();
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

		[MenuItem ("Assets/Vgame/Create Lua Script", false, 1)]
		static void CreateLuaScript ()
		{
			ScriptEditor.CreateLuaScript ();
		}
	}
}

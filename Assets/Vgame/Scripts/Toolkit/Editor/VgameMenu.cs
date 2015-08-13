using UnityEditor;
using UnityEngine;
using Vgame.ToolKit;

namespace VgameEditor
{
	public static class VgameMenu
	{
		[MenuItem ("Vgame/ToolKit/Excel/Excel To CSV", false, 1)]
		static void ExcelToCSV ()
		{
			ExcelEditor.ExcelToCSV ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To JSON", false, 1)]
		static void ExcelToJSON ()
		{
			FileEx.CreateZipFile (Application.persistentDataPath, Application.persistentDataPath);
			//ExcelEditor.ExcelToJSON ();
		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To Class", false, 1)]
		static void ExcelToClass ()
		{
//			ExcelEditor.ExcelToClass ();
			FileEx.UnZipFile (Application.persistentDataPath + "/vgame.zip");
		}

		//		[MenuItem ("Vgame/ToolKit/Excel/")]
		//		static void ExcelBreaker ()
		//		{
		//			//分割线
		//		}

		[MenuItem ("Vgame/ToolKit/Excel/Excel To Lua", false, 12)]
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

		[MenuItem ("Assets/Vgame/Create/Lua Script", false, 1)]
		static void CreateLuaScript ()
		{
			ScriptEditor.CreateLuaScript ();
		}

		[MenuItem ("Assets/Vgame/Create/C# Script", false, 1)]
		static void CreateCSharpScript ()
		{
			ScriptEditor.CreateCSharpScript ();
		}

		[MenuItem ("Assets/Vgame/Create/ZIP", false, 12)]
		static void CreateZipFile ()
		{
			FileEditor.CreateZipFile ();
		}
	}
}

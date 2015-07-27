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
	}
}

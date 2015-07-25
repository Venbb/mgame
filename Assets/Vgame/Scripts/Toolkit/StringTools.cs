using System.Collections.Generic;

namespace Vgame.ToolKit
{
	public static class StringTools
	{
		/// <summary>
		/// 首写字母大写
		/// </summary>
		/// <param name="str">String.</param>
		public static void ToUpperFirstChar (ref string str)
		{
			if (string.IsNullOrEmpty (str)) return;
			char c1 = char.ToUpper (str [0]);
			if (str.Length > 1)
			{
				str = c1 + str.Substring (1);
			}
		}

		/// <summary>
		/// 首字母小写
		/// </summary>
		/// <param name="str">String.</param>
		public static void ToLowerFirstChar (ref string str)
		{
			if (string.IsNullOrEmpty (str)) return;
			char c1 = char.ToLower (str [0]);
			if (str.Length > 1)
			{
				str = c1 + str.Substring (1);
			}
		}
	}
}
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

		/// <summary>
		/// 获取字符类型
		/// </summary>
		/// <returns>The string type string.</returns>
		/// <param name="str">String.</param>
		public static string GetStrTypeStr (string str)
		{
			if (string.IsNullOrEmpty (str)) return "string";
			if (str.StartsWith ("\"")) return "string";
			if (str.Equals ("false") || str.Equals ("true")) return "bool";
			string arrStr = GetListStr (str);
			if (!string.IsNullOrEmpty (arrStr))
			{
				
			}
			if (str.StartsWith ("{") && str.EndsWith ("}")) return "object";
			if (str.Contains ("."))
			{
				float f;
				if (float.TryParse (str, out f))
				{
					return "float";
				}
				double d;
				if (double.TryParse (str, out d))
				{
					return "double";
				}
			}
			else
			{
				int i;
				if (int.TryParse (str, out i))
				{
					return "int";
				}
			}
			return "string";
		}

		public static string GetListStr (string str)
		{
			if (str.StartsWith ("[") && str.EndsWith ("]"))
			{
				string t = "";
				int len = str.Length;
				if (len > 2)
				{
					str = str.Substring (str.IndexOf ("[") + 1, len - 2);

					string[] vs = str.Split (',');
					foreach (string s in vs)
					{
						string tt = GetStrTypeStr (s);
						if (string.IsNullOrEmpty (t))
						{
							t = tt;
						}
						else
						{
							if (!t.Equals (tt))
							{
								t = "ArrayList";
								break;
							}
						}
					}
				}
				if (string.IsNullOrEmpty (t)) t = "ArrayList";
				return "List<" + t + ">";	
			}
			return "";
		}
	}
}
using System.Collections.Generic;

namespace Vgame.ToolKit
{
	public static class StrType
	{
		public const string NONE = "none";
		public const string INT = "int";
		public const string STRING = "string";
		public const string BOOL = "bool";
		public const string OBJECT = "object";
		public const string FLOAT = "float";
		public const string DOUBLE = "double";
		public const string LONG = "long";
		public const string ARRAYLIST = "ArrayList";
		public const string LIST_INT = "list<int>";
		public const string LIST_STRING = "list<string>";
		public const string LIST_FLOAT = "list<float>";
		public const string LIST_DOUBLE = "list<double>";
		public const string LIST_LONG = "list<long>";
	}

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
		public static string GetStrType (string str)
		{
			if (string.IsNullOrEmpty (str)) return StrType.STRING;
			if (str.StartsWith ("\"") && str.EndsWith ("\"")) return StrType.STRING;
			if (str.Equals ("false") || str.Equals ("true")) return StrType.BOOL;
			string arrTypeStr;
			if (IsList (str, out arrTypeStr))
			{
				return arrTypeStr;
			}
			if (str.StartsWith ("{") && str.EndsWith ("}"))
			{
				char[] chars = str.ToCharArray ();
				int a = 0;
				int b = 0;
				foreach (char c in chars)
				{
					if (c.Equals ('{')) a += 1;
					if (c.Equals ('}')) b += 1;
				}
				if (a == b) return StrType.OBJECT;	
			}
			if (str.Contains ("."))
			{
				float f;
				if (float.TryParse (str, out f))
				{
					return StrType.FLOAT;
				}
				double d;
				if (double.TryParse (str, out d))
				{
					return StrType.DOUBLE;
				}
			}
			else
			{
				int i;
				if (int.TryParse (str, out i))
				{
					return StrType.INT;
				}
			}
			return StrType.NONE;
		}

		/// <summary>
		/// 是否数组
		/// </summary>
		/// <returns><c>true</c> if is list the specified str typeStr; otherwise, <c>false</c>.</returns>
		/// <param name="str">要判断的字符串</param>
		/// <param name="type">输出类型的字符串格式</param>
		public static bool IsList (string str, out string type)
		{
			type = StrType.NONE;
			if (str.StartsWith ("[") && str.EndsWith ("]"))
			{
				int len = str.Length;
				if (len > 2)
				{
					char[] chars = str.ToCharArray ();
					List<string> list_s = new List<string> ();
					List<int> lis_i = new List<int> ();
					List<float> list_f = new List<float> ();
					List<double> list_d = new List<double> ();
					List<long> list_l = new List<long> ();

					string p = "";
					bool isStr = false;
					for (int i = 1; i < chars.Length - 1; i++)
					{
						char c = chars [i];
						if (c.Equals ('['))
						{
							type = StrType.ARRAYLIST;
							return true;
						}
						if (c.Equals (','))
						{
							if (isStr)
							{
								list_s.Add (p);
							}
							else
							{
								if (p.Contains ("."))
								{
									float f;
									if (float.TryParse (p, out f))
									{
										list_f.Add (f);
									}
									double d;
									if (double.TryParse (p, out d))
									{
										list_d.Add (d);
									}
								}
								else
								{
									int si;
									if (int.TryParse (p, out si))
									{
										lis_i.Add (si);
									}
								}
							}
							p = "";
						}
						else
						{
							isStr = c.Equals ('\"');
							p += c;
						}
					}
					if (list_l.Count > 0 && list_d.Count == 0 && list_f.Count == 0 && list_s.Count == 0 && lis_i.Count == 0)
					{
						type = StrType.LIST_LONG;
						return true;
					}
					if (list_l.Count == 0 && list_d.Count > 0 && list_f.Count == 0 && list_s.Count == 0 && lis_i.Count == 0)
					{
						type = StrType.LIST_DOUBLE;
						return true;
					}
					if (list_l.Count == 0 && list_d.Count == 0 && list_f.Count > 0 && list_s.Count == 0 && lis_i.Count == 0)
					{
						type = StrType.LIST_FLOAT;
						return true;
					}
					if (list_l.Count == 0 && list_d.Count == 0 && list_f.Count == 0 && list_s.Count > 0 && lis_i.Count == 0)
					{
						type = StrType.LIST_STRING;
						return true;
					}
					if (list_l.Count == 0 && list_d.Count == 0 && list_f.Count == 0 && list_s.Count == 0 && lis_i.Count > 0)
					{
						type = StrType.LIST_INT;
						return true;
					}
				}
				type = StrType.ARRAYLIST;
			}
			return false;
		}
	}
}
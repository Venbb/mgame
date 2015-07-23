using System.Text;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace Vgame.ToolKit.Editor
{
	public static class JSONTools
	{
		/// <summary>
		/// 转换成实体类（字符串）
		/// </summary>
		/// <returns>The class.</returns>
		/// <param name="jsonStr">Json string.</param>
		/// <param name = "className"></param>
		public static string ToClass (string jsonStr, string className)
		{
			if (string.IsNullOrEmpty (jsonStr)) return null;
			if (string.IsNullOrEmpty (className))
			{
				Debug.LogError ("类名不能为空！");
				return null;
			}
			bool isErr = true;
			if (jsonStr.StartsWith ("{") && jsonStr.EndsWith ("}"))
			{
				int len = jsonStr.Length;
				if (len > 2)
				{
					jsonStr = jsonStr.Substring (jsonStr.IndexOf ("{") + 1, len - 2);
					isErr = false;
				}
			}
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("public class " + className);
			sb.AppendLine ("{");
			string[] jsons = jsonStr.Split (',');
			foreach (string str in jsons)
			{
				if (!Regex.IsMatch (str, "\"(.*?)\":(.*?)"))
				{
					isErr = true;
					break;
				}
				string[] kvs = str.Split (':');
				string key = kvs [0];
				string value = kvs [1];
				StringTools.ToUpperFirstChar (ref key);
				string type = "string";
				if (!value.StartsWith ("\""))
				{
					if (value.StartsWith ("["))
					{
						value = value.Substring (1, value.Length - 2);	
						string[] arr = value.Split (',');
						string at = "";
						foreach (string a in arr)
						{
							if (!a.StartsWith ("\"") && !a.StartsWith ("{") && a.StartsWith ("["))
							{
								
							}
							else
							{
								at = "List<>";
							}
						}
						type = at;
					}
					else
					{
						if (value.StartsWith ("{"))
						{
							string cname = className + "_" + key;
							string classStr = ToClass (value, cname);
							if (string.IsNullOrEmpty (classStr))
							{
								isErr = true;
								break;
							}
							else
							{
								type = cname;
							}
						}
						else
						{
							if (value.Contains ("."))
							{
								type = "double";
							}
							else
							{
								type = "int";
							}
						}
					}
				}
				sb.AppendLine ("public " + type + " " + key + ";");
			}
			if (isErr)
			{
				Debug.Log ("错误的JSON格式！");
				return null;
			}
			sb.Append ("}");
			return sb.ToString ();
		}
	}
}

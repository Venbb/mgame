using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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


		public static bool IsJson (string str, out List<string> keys, out List<string> values)
		{
			bool isJson = true;
			keys = new List<string> ();
			values = new List<string> ();
			char[] chars = str.ToCharArray ();
			int len = chars.Length;
			if (len < 2) return false;
			char firstChar = chars [0];
			char endChar = chars [len - 1];
			string tmpStr = "";
			char c = '\0';
			char cn = '\0';
			bool getKey = true;
			string isErrorMs = "";
			if (firstChar == '{' && endChar == '}')
			{
				int i = 1;
				//当前截取字符串长度
				int tmpStrLen = 0;
				while (i < len - 1)
				{
					tmpStrLen = tmpStr.Length;
					//当前字符
					c = chars [i];
					//第一个字符应该是引号'\"'
					if (i == 1)
					{
						if (c != '\"')
						{
							isErrorMs = "" + c;	
							break;	
						}
						else tmpStr += c;
						continue;
					}
					//下一个字符
					if (i + 1 < len)
					{
						cn = chars [i + 1];
					}
					else
					{
						cn = '\0';
					}
					switch (c)
					{
					case '\"':
						int index = tmpStr.IndexOf ('\"');
						if (index != 0)
						{
							isErrorMs = "key:" + c;
							break;
						}
						if (getKey)
						{
							if (tmpStrLen > 1)
							{
								if (cn == ':')
								{
									keys.Add (tmpStr + c);	
									getKey = false;
									i += 2;
								}
								else isErrorMs = "" + cn;
							}
							else isErrorMs = "key:" + c;
							tmpStr = "";
						}
						else
						{
							if (cn == ',')
							{
								values.Add (tmpStr);	
								getKey = true;
							}
							else isErrorMs = "" + cn;
						}
						break;
					case ']':
						if (getKey)
						{
							isErrorMs = "" + c;
							break;
						}
						if (tmpStr.IndexOf ('[') != 0)
						{
							isErrorMs = "" + c;
							break;
						}
						if (cn == ',')
						{
							values.Add (tmpStr);	
							getKey = true;
							tmpStr = "";
						}
						else isErrorMs = "" + cn;
						break;
					case '}':
						if (getKey)
						{
							isErrorMs = "" + c;
							break;
						}
						if (tmpStr.IndexOf ('{') != 0)
						{
							isErrorMs = "" + c;
							break;
						}
						if (cn == ',')
						{
							values.Add (tmpStr);	
							getKey = true;
							tmpStr = "";
						}
						else isErrorMs = "" + cn;
						break;
					}
					if (!string.IsNullOrEmpty (isErrorMs)) break;
				}
				if (!string.IsNullOrEmpty (isErrorMs))
				{
					isJson = false;
					Debug.LogError ("无效的字符:" + c);
				}
			}
			else isJson = false;
			return isJson;
		}
	}
}

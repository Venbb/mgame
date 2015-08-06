using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using Vgame.ToolKit;

namespace VgameEditor
{
	public static class JSONEditor
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
			List<string> classes = new List<string> ();
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("public class " + className);
			sb.AppendLine ("{");
			JsonData jd = JsonMapper.ToObject (jsonStr);
			foreach (DictionaryEntry de in jd)
			{
				JsonData value = de.Value as JsonData;
				string key = de.Key.ToString ();
				StringTools.ToUpperFirstChar (ref key);
				if (value.IsInt)
				{
					sb.AppendLine ("\tpublic int " + key + ";");	
				}
				if (value.IsString)
				{
					sb.AppendLine ("\tpublic string " + key + ";");
				}
				if (value.IsDouble)
				{
					sb.AppendLine ("\tpublic double " + key + ";");
				}
				if (value.IsLong)
				{
					sb.AppendLine ("\tpublic long " + key + ";");
				}
				if (value.IsBoolean)
				{
					sb.AppendLine ("\tpublic bool " + key + ";");
				}
				if (value.IsObject)
				{
					sb.AppendLine ("\tpublic " + (className + "_" + key) + " " + key);
					classes.Add (ToClass (value.ToJson (), className + "_" + key));
				}
				if (value.IsArray)
				{
					int count = value.Count;
					string tstr = "ArrayList";
					int intNum = 0;
					int strNum = 0;
					int doubleNum = 0;
					int longNum = 0;
					for (int i = 0; i < count; i++)
					{
						JsonData jda = jd [i];
						if (jda.IsInt)
						{
							intNum++;
							continue;
						}
						if (jda.IsString)
						{
							strNum++;
							continue;
						}
						if (jda.IsLong)
						{
							longNum++;
							continue;
						}
						if (jda.IsDouble) doubleNum++;
					}
					if (count != 0 && intNum == count) tstr = "List<int>";
					if (count != 0 && strNum == count) tstr = "List<string>";
					if (count != 0 && doubleNum == count) tstr = "List<double>";
					if (count != 0 && longNum == count) tstr = "List<long>";
					sb.AppendLine ("\tpublic " + tstr + " " + key + ";");
				}
			}
			sb.AppendLine ("}");
			foreach (string str in classes)
			{
				sb.AppendLine (str);
			}
			return sb.ToString ();
		}
	}
}

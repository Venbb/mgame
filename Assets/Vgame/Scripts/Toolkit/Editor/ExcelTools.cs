using UnityEngine;
using UnityEditor;
using System.IO;
using System.Data;
using Excel;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;

namespace Vgame.ToolKit.Editor
{
	public static class ExcelTools
	{
		/// <summary>
		/// 逗号分隔符
		/// </summary>
		const char CHAR_COMMA = ',';
		/// <summary>
		/// 换行符
		/// </summary>
		const char CHAR_WRAP = '\n';
		/// <summary>
		/// 制表分隔符
		/// </summary>
		const char CHAR_TAB = '\t';
		/// <summary>
		/// Excel扩展名xlsx
		/// </summary>
		const string EX_XLSX = "^.*.(?i)xlsx$";
		/// <summary>
		/// Excel扩展名xls
		/// </summary>
		const string EX_XLS = "^.*.(?i)xls$";
		/// <summary>
		/// 保存为JSON数据格式的扩展名
		/// </summary>
		const string SAVE_EXT_JSON = ".json.baytes";
		/// <summary>
		/// 保存为CSV数据格式的扩展名
		/// </summary>
		const string SAVE_EXT_CSV = ".csv.baytes";
		/// <summary>
		/// 生成的实体类扩展名
		/// </summary>
		const string SAVE_EXT_CLASS = ".cs";
		/// <summary>
		/// 读取EXCEl的文件路径片段
		/// </summary>
		const string READ_DIR_NAME_EXCEL = "Vdata/Excel";
		/// <summary>
		/// 保存JSON数据的文件路径片段
		/// </summary>
		const string SAVE_DIR_NAME_JSON = "Vdata/Json";
		/// <summary>
		/// 保存CSV数据的文件路径片段
		/// </summary>
		const string SAVE_DIR_NAME_CSV = "Vdata/CSV";
		/// <summary>
		/// 保存CLASS实体类的文件路径片段
		/// </summary>
		const string SAVE_DIR_NAME_CLASS = "Vdata/Classes";

		/// <summary>
		/// 文件保存扩展名
		/// </summary>
		static string SAVE_EXT = "";
		/// <summary>
		/// 文件存储目录名
		/// </summary>
		static string SAVE_DIR_NAME = "";

		static DataSet ds = new DataSet ();

		[MenuItem ("Vgame/ToolKit/Excel To CSV")]
		static void ExcelToCSV ()
		{
			SAVE_EXT = SAVE_EXT_CSV;
			SAVE_DIR_NAME = SAVE_DIR_NAME_CSV;
			OnBegin (ConvertToCSV);
		}

		[MenuItem ("Vgame/ToolKit/Excel To JSON")]
		static void ExcelToJSON ()
		{
//			LitJson.JsonData jd = LitJson.JsonMapper.ToObject ("{\"jj\":[1.0]}");
//			foreach (System.Collections.DictionaryEntry de in jd)
//			{
//				Debug.Log (de.Key);
//				LitJson.JsonData lj = de.Value as LitJson.JsonData;
//				int count = lj.Count;
//				Debug.Log (count);
//				for (int i = 0; i < count; i++)
//				{
//					Debug.Log (lj [i].GetType ());
//				}
//			}
			SAVE_EXT = SAVE_EXT_JSON;
			SAVE_DIR_NAME = SAVE_DIR_NAME_JSON;
			OnBegin (ConvertToJSON);
		}

		[MenuItem ("Vgame/ToolKit/Excel To Class")]
		static void ExcelToClass ()
		{
			SAVE_EXT = SAVE_EXT_CLASS;
			SAVE_DIR_NAME = SAVE_DIR_NAME_CLASS;
			OnBegin (ConvertToClass);
		}

		static void OnBegin (CallBackWithParams<DataTable,List<string>> ckFun)
		{
			if (ckFun == null) return;
			ClearData ();
			List<string> readPathes = GetExcelPathes ();
			if (readPathes.Count == 0)
			{
				Debug.LogError ("未能找到Excel文件路径，请确保在Assets文件夹下面包含有正确Excel文件格式的“Vdata/Excel”文件夹。");
				return;
			}
			List<string> writePathes = GetWriteDirectories ();
			if (writePathes.Count == 0)
			{
				Debug.LogError ("未能找到" + SAVE_DIR_NAME + "输出路径，请确保在Assets文件夹下面包含有" + SAVE_DIR_NAME + "”文件夹。");
				return;
			}
			List<DataTable> tables = new List<DataTable> ();
			foreach (string path in readPathes)
			{
				tables.AddRange (GetExcelData (path));
			}

			foreach (DataTable table in tables)
			{
				ckFun (table, writePathes);
			}
			AssetDatabase.Refresh ();
		}

		/// <summary>
		/// 读取Excel路径
		/// </summary>
		static List<string> GetExcelPathes ()
		{
			return FileTools.GetFiles (READ_DIR_NAME_EXCEL, EX_XLSX, EX_XLS);
		}

		/// <summary>
		/// 解析Excel数据
		/// </summary>
		/// <param name="path">Path.</param>
		static List<DataTable> GetExcelData (string path)
		{
			Debug.Log ("读取Excel:" + path);
			List<DataTable> tables = new List<DataTable> ();
			FileStream stream = File.Open (path, FileMode.Open, FileAccess.Read);

			IExcelDataReader reader = null;
			if (Regex.IsMatch (path, EX_XLSX))
			{
				reader = ExcelReaderFactory.CreateOpenXmlReader (stream);
			}
			if (Regex.IsMatch (path, EX_XLS))
			{
				reader = ExcelReaderFactory.CreateBinaryReader (stream);
			}
			ds = reader.AsDataSet ();
			reader.Close ();	

			int tableCount = ds.Tables.Count;
			for (int i = 0; i < tableCount; i++)
			{
				tables.Add (ds.Tables [i]);
			}
			return tables;
		}

		/// <summary>
		/// 获取数据输出路径
		/// </summary>
		/// <returns>The write pathes.</returns>
		static List<string> GetWriteDirectories ()
		{
			return FileTools.GetDirectories (SAVE_DIR_NAME);
		}

		/// <summary>
		/// 清除生成的数据
		/// </summary>
		static void ClearData ()
		{
			List<string> files = FileTools.GetFiles (GetWriteDirectories ());
			foreach (string file in files)
			{
				Debug.Log ("删除:" + file);
				File.Delete (file);	
			}
		}

		/// <summary>
		/// 转换成CSV
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name = "pathes"></param>
		static void ConvertToCSV (DataTable table, List<string> pathes)
		{
			int rows = table.Rows.Count;
			int columus = table.Columns.Count;
			StringBuilder sb = new StringBuilder ();
			for (int i = 1; i < rows; i++)
			{
				if (table.Rows [i].IsNull (0)) continue;
				for (int j = 0; j < columus; j++)
				{
					sb.Append (table.Rows [i] [j].ToString ());
					if (j + 1 < columus) sb.Append (CHAR_TAB);
				}
				if (i + 1 < rows) sb.Append (CHAR_WRAP);
			}
			foreach (string str in pathes)
			{
				string path = Path.Combine (str, table.TableName + SAVE_EXT);
				StreamWriter sw = new StreamWriter (path, false, Encoding.Unicode);
				sw.Write (sb);
				sw.Close ();
				Debug.Log ("生成CSV成功:" + path);	
			}
		}

		/// <summary>
		/// 转换成JSON
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name="pathes">Pathes.</param>
		static void ConvertToJSON (DataTable table, List<string> pathes)
		{
			int rows = table.Rows.Count;
			int columus = table.Columns.Count;
			//字段名
			string[] fields = ReadFields (table);
			//字段类型
			string[] fieldTypes = ReadFieldTypes (table);

			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("[");
			for (int i = 3; i < rows; i++)
			{
				sb.AppendLine (" {");
				for (int j = 0; j < columus; j++)
				{
					string valueStr = table.Rows [i] [j].ToString ();
					string type = fieldTypes [j];
					if (type.Equals ("string"))
					{
						sb.AppendLine (string.Format ("  \"{0}\":{1}{2}", fields [j], "\"" + valueStr + "\"", (j + 1 < columus ? "," : "")));
					}
					else
					{
						switch (type)
						{
						case "int":
							if (string.IsNullOrEmpty (valueStr))
							{
								valueStr = "0";	
							}
							else
							{
								if (valueStr.Contains (".")) valueStr = valueStr.Substring (0, 1);
							}
							break;
						case "float":
							if (string.IsNullOrEmpty (valueStr))
							{
								valueStr = "0";	
							}
							else
							{
								if (!valueStr.Contains (".")) valueStr += ".0";
							}
							break;
						case "double":
							if (string.IsNullOrEmpty (valueStr))
							{
								valueStr = "0";	
							}
							else
							{
								if (!valueStr.Contains (".")) valueStr += ".0";
							}
							break;
						default:
							if (string.IsNullOrEmpty (valueStr)) valueStr = "null";
							break;
						}
						if (string.IsNullOrEmpty (valueStr))
						{

						}
						sb.AppendLine (string.Format ("  \"{0}\":{1}{2}", fields [j], valueStr, (j + 1 < columus ? "," : "")));
					}
				}
				sb.AppendLine (i + 1 < rows ? " }," : " }");
			}
			sb.Append ("]");
			foreach (string str in pathes)
			{
				string path = Path.Combine (str, table.TableName + SAVE_EXT);
				StreamWriter sw = new StreamWriter (path, false, Encoding.Unicode);
				sw.Write (sb);
				sw.Close ();
				Debug.Log ("生成JSON成功:" + path);
			}
		}

		/// <summary>
		/// 生成实体类
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name="pathes">Pathes.</param>
		static void ConvertToClass (DataTable table, List<string> pathes)
		{
			// 字段描述
			string[] fieldDes = ReadFieldDes (table);
			//字段名
			string[] fields = ReadFields (table);
			//字段类型
			string[] fieldTypes = ReadFieldTypes (table);
			//首行值
			ArrayList values = ReadFielValuesTop (table);

			List<string> classes = new List<string> ();

			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("using System;");
			sb.AppendLine ("using System.Collections;");
			sb.AppendLine ("using System.Collections.Generic;");
			sb.AppendLine ("using Vgame.Data;");
			sb.AppendLine ("");
			string className = table.TableName;
			StringTools.ToUpperFirstChar (ref className);
			sb.AppendLine ("public class " + className + ":DataBase");
			sb.AppendLine ("{");
			for (int i = 0; i < fields.Length; i++)
			{
				sb.AppendLine ("\t/// <summary>");
				sb.AppendLine ("\t/// " + fieldDes [i]);
				sb.AppendLine ("\t/// </summary>");
				string type = fieldTypes [i];
				string field = fields [i];
				if (type.Equals ("json"))
				{
					type = "string";
					if (values.Count > i)
					{
						string value = values [i].ToString ();
						type = className + "_" + field;
						classes.Add (JSONTools.ToClass (value, className + "_" + field));	
					}
				}
				StringTools.ToUpperFirstChar (ref field);
				sb.AppendLine ("\tpublic " + type + " " + field + ";");	
			}
			sb.AppendLine ("}");
			foreach (string str in classes)
			{
				sb.AppendLine (str);
			}
			foreach (string str in pathes)
			{
				string path = Path.Combine (str, className + SAVE_EXT);
				StreamWriter sw = new StreamWriter (path, false, Encoding.Unicode);
				sw.Write (sb);
				sw.Close ();
				Debug.Log ("生成Class成功:" + path);
			}
		}

		/// <summary>
		/// 读取表字段描述
		/// </summary>
		/// <returns>The field DES.</returns>
		/// <param name="table">Table.</param>
		static string[] ReadFieldDes (DataTable table)
		{
			int columus = table.Columns.Count;
			//字段描述
			string[] fieldDecs = new string[columus];
			//读取字段描述
			for (int i = 0; i < columus; i++)
			{
				if (table.Rows [0].IsNull (0))
				{
					Debug.LogError ("读取表字段描述出错:" + table.TableName);
					return null;	
				}
				fieldDecs.SetValue (table.Rows [0] [i].ToString (), i);
			}
			return fieldDecs;
		}

		/// <summary>
		/// 读取字段名
		/// </summary>
		/// <returns>The fields.</returns>
		/// <param name="table">Table.</param>
		static string[] ReadFields (DataTable table)
		{
			int columus = table.Columns.Count;
			//字段描述
			string[] fields = new string[columus];
			//读取字段描述
			for (int i = 0; i < columus; i++)
			{
				if (table.Rows [1].IsNull (0))
				{
					Debug.LogError ("读取字段名出错:" + table.TableName);
					return null;	
				}
				fields.SetValue (table.Rows [1] [i].ToString (), i);
			}
			return fields;
		}

		/// <summary>
		/// 读取字段类型
		/// </summary>
		/// <returns>The field types.</returns>
		/// <param name="table">Table.</param>
		static string[] ReadFieldTypes (DataTable table)
		{
			int columus = table.Columns.Count;
			//字段描述
			string[] types = new string[columus];
			//读取字段描述
			for (int i = 0; i < columus; i++)
			{
				if (table.Rows [2].IsNull (0))
				{
					Debug.LogError ("读取字段类型出错:" + table.TableName);
					return null;	
				}
				types.SetValue (table.Rows [2] [i].ToString (), i);
			}
			return types;
		}

		/// <summary>
		/// 获取第一行表数据
		/// </summary>
		/// <returns>The fiel values top.</returns>
		/// <param name="table">Table.</param>
		static ArrayList ReadFielValuesTop (DataTable table)
		{
			int columus = table.Columns.Count;
			//字段描述
			ArrayList values = new ArrayList ();
			//读取字段描述
			for (int i = 0; i < columus; i++)
			{
				values.Add (table.Rows [3] [i]);
			}
			return values;
		}
	}
}
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Data;
using Excel;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;
using System.Collections.Generic;

namespace Vgame.ToolKit
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
		/// 文件保存扩展名
		/// </summary>
		static string SAVE_EXT = ".json.baytes";
		/// <summary>
		/// 文件存储目录名
		/// </summary>
		static string SAVE_DIR_NAME = "Json";
		/// <summary>
		/// 匹配Excel配置文件
		/// </summary>
		static string[] readExt = { "^.*.(?i)xlsx$", "^.*.(?i)xls$" };

		static DataSet ds = new DataSet ();

		[MenuItem ("Vgame/ToolKit/Excel To CSV")]
		static void ExcelToCSV ()
		{
			SAVE_EXT = ".csv.baytes";
			SAVE_DIR_NAME = "CSV";
			OnBegin (ConvertToCSV);
		}

		[MenuItem ("Vgame/ToolKit/Excel To JSON")]
		static void ExcelToJSON ()
		{
			SAVE_EXT = ".json.baytes";
			SAVE_DIR_NAME = "Json";
			OnBegin (ConvertToJSON);
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
				Debug.LogError ("未能找到" + SAVE_DIR_NAME + "输出路径，请确保在Assets文件夹下面包含有“Vdata/" + SAVE_DIR_NAME + "”文件夹。");
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
			List<string> pathes = new List<string> ();
			string[] directories = Directory.GetDirectories (Application.dataPath, "Excel", SearchOption.AllDirectories);
			foreach (string dir in directories)
			{
				if (!dir.Contains ("Vdata/Excel")) continue;
				string[] excels = Directory.GetFiles (dir);
				foreach (string path in excels)
				{
					string regexStr = string.Format (@"{0}", string.Join ("|", readExt));
					if (Regex.IsMatch (path, regexStr))
					{
						pathes.Add (path);
					}
				}	
			}
			return pathes;
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
			if (Regex.IsMatch (path, "^.*.(?i)xlsx$"))
			{
				reader = ExcelReaderFactory.CreateOpenXmlReader (stream);
			}
			if (Regex.IsMatch (path, "^.*.(?i)xls$"))
			{
				reader = ExcelReaderFactory.CreateBinaryReader (stream);
			}
			ds = reader.AsDataSet ();
			reader.Close ();	

			int tableCount = ds.Tables.Count;
			for (int i = 1; i < tableCount; i++)
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
			List<string> pathes = new List<string> ();
			string[] files = Directory.GetDirectories (Application.dataPath, SAVE_DIR_NAME, SearchOption.AllDirectories);
			foreach (string file in files)
			{
				if (!file.Contains ("Vdata/" + SAVE_DIR_NAME)) continue;
				pathes.Add (file);
			}
			return pathes;
		}

		/// <summary>
		/// 清除JSON
		/// </summary>
		static void ClearData ()
		{
			List<string> directories = GetWriteDirectories ();
			foreach (string dir in directories)
			{
				string[] pathes = Directory.GetFiles (dir);
				foreach (string path in pathes)
				{
					Debug.Log ("删除:" + path);
					File.Delete (path);	
				}
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
			string[] fields = new string[columus];
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("[");
			for (int i = 1; i < rows; i++)
			{
				if (table.Rows [i].IsNull (0)) continue;
				if (i == 1)
				{
					for (int j = 0; j < columus; j++)
					{
						fields.SetValue (table.Rows [i] [j].ToString (), j);
					}
					continue;
				}
				sb.AppendLine (" {");
				for (int j = 0; j < columus; j++)
				{
					object o = table.Rows [i] [j];
					int a;
					Debug.Log (int.TryParse (o.ToString (), out a));
					Debug.Log ("...." + a);
					Debug.Log (o + "================" + (o is string));
					if (o is string)
					{
						sb.AppendLine (string.Format ("  \"{0}\":\"{1}\"{2}", fields [j], o, (j + 1 < columus ? "," : "")));

					}
					else
					{
						sb.AppendLine (string.Format ("  \"{0}\":{1}{2}", fields [j], table.Rows [i] [j], (j + 1 < columus ? "," : "")));

					}
					Debug.Log (fields [j] + ":" + table.Rows [i] [j].GetType ());

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

	}
}
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

public class ExcelToCSV
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
	const string SAVE_EXT = ".csv.baytes";

	static string readPath = Application.dataPath + "/Config/Excel";
	static string[] readExt = { "^.*.(?i)xlsx$", "^.*.(?i)xls$" };
	static string writePath = Application.dataPath + "/Config/Data";
	static string[] writeExt = { "^.*.(?i)bytes$", "^.*.(?i)csv$" };

	static DataSet ds = new DataSet ();

	[MenuItem ("Tools/Excel To CSV")]
	static void OnClick ()
	{
		ClearData ();
		ReadExcel ();
		AssetDatabase.Refresh ();
	}

	/// <summary>
	/// 读取Excel
	/// </summary>
	static void ReadExcel ()
	{
		Debug.Log ("开始读取...");
		string[] excels = Directory.GetFiles (readPath);
		foreach (string path in excels)
		{
			string regexStr = string.Format (@"{0}", string.Join ("|", readExt));
			if (Regex.IsMatch (path, regexStr))
			{
				Debug.Log ("读取Excel:" + path);
				GetExcelData (path);
			}
		}
	}

	/// <summary>
	/// 清除CSV
	/// </summary>
	static void ClearData ()
	{
		if (!Directory.Exists (writePath)) return;
		string[] csvs = Directory.GetFiles (writePath);
		foreach (string path in csvs)
		{
			string regexStr = string.Format (@"{0}", string.Join ("|", writeExt));
			if (Regex.IsMatch (path, regexStr))
			{
				Debug.Log ("删除:" + path);
				File.Delete (path);
			}
		}
	}

	/// <summary>
	/// 解析Excel数据
	/// </summary>
	/// <param name="path">Path.</param>
	static void GetExcelData (string path)
	{
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
			string name = ds.Tables[i].TableName;
			Debug.Log ("解析配置:" + name);
			ConvertToJSON (ds.Tables[i]);
		}
		Debug.Log ("转换成功!");
	}

	/// <summary>
	/// 转换成CSV
	/// </summary>
	/// <param name="table">Table.</param>
	static void ConvertToCSV (DataTable table)
	{
		int rows = table.Rows.Count;
		int columus = table.Columns.Count;
		StringBuilder sb = new StringBuilder ();
		for (int i = 1; i < rows; i++)
		{
			if (table.Rows[i].IsNull (0)) continue;
			for (int j = 0; j < columus; j++)
			{
				sb.Append (table.Rows[i][j].ToString ());
				if (j + 1 < columus) sb.Append (CHAR_TAB);
			}
			if (i + 1 < rows) sb.Append (CHAR_WRAP);
		}
		if (!Directory.Exists (writePath))
		{
			Directory.CreateDirectory (writePath);
		}
		string path = Path.Combine (writePath, table.TableName + SAVE_EXT);
		StreamWriter sw = new StreamWriter (path, false, Encoding.Unicode);
		sw.Write (sb);
		sw.Close ();
		Debug.Log ("生成CSV成功:" + path);
	}

	static void ConvertToJSON (DataTable table)
	{
		int rows = table.Rows.Count;
		int columus = table.Columns.Count;
		string[] fields = new string[columus];
		StringBuilder sb = new StringBuilder ();
		sb.AppendLine ("[");
		for (int i = 1; i < rows; i++)
		{
			if (table.Rows[i].IsNull (0)) continue;
			if (i == 1)
			{
				for (int j = 0; j < columus; j++)
				{
					fields.SetValue (table.Rows[i][j].ToString (), j);
				}
				continue;
			}
			sb.AppendLine (" {");
			for (int j = 0; j < columus; j++)
			{
				object o = table.Rows[i][j];
				Debug.Log (o + "================" + (o.GetType () == typeof(string)));
				if (o.GetType () == typeof(string))
				{
					sb.AppendLine (string.Format ("  \"{0}\":\"{1}\"{2}", fields[j], o, (j + 1 < columus ? "," : "")));

				}
				else
				{
					sb.AppendLine (string.Format ("  \"{0}\":{1}{2}", fields[j], table.Rows[i][j], (j + 1 < columus ? "," : "")));

				}
				Debug.Log (fields[j] + ":" + table.Rows[i][j].GetType ());

			}
			sb.AppendLine (i + 1 < rows ? " }," : " }");
		}
		sb.Append ("]");
		if (!Directory.Exists (writePath))
		{
			Directory.CreateDirectory (writePath);
		}
		string path = Path.Combine (writePath, table.TableName + ".json.baytes");
		StreamWriter sw = new StreamWriter (path, false, Encoding.Unicode);
		sw.Write (sb);
		sw.Close ();
		JsonData jd = JsonMapper.ToObject (sb.ToString ());
		Debug.Log (jd);
//		List<Mouse> mouses = JsonMapper.ToObject<List<Mouse>> (sb.ToString ());
//		foreach (Mouse m in mouses)
//		{
//			Debug.Log (m.name);
//		}
		Debug.Log ("生成JSON成功:" + path);
	}
}

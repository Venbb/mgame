using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.XCodeEditor
{
	public class XClass
	{
		string classPath;

		public XClass (string classPath)
		{
			this.classPath = classPath;
		}

		public void Process (ArrayList classes)
		{
			foreach (object o in classes)
			{
				Hashtable table = o as Hashtable;
				ArrayList arrr = table ["replace"] as ArrayList;
				ArrayList arra = table ["append"] as ArrayList;
				if (arrr != null && arrr.Count > 0)
				{
					XCodeClass xcc = new XCodeClass (classPath + table ["name"]);	
					xcc.Replace (arrr [0].ToString (), arrr [1].ToString (), arrr [2].ToString ());
				}
				if (arra != null && arra.Count > 0)
				{
					XCodeClass xcc = new XCodeClass (classPath + table ["name"]);	
					xcc.WriteLine (arra [0].ToString (), arra [1].ToString ());
				}
			}
		}
	}

	public class XCodeClass
	{
		string path;

		public XCodeClass (string path)
		{
			this.path = path;
			Debug.Log ("Class path:" + path);
		}

		public void WriteLine (string last, string append)
		{
			if (!File.Exists (path))
			{
				Debug.Log ("未找到类文件:" + path);
				return;
			}
			StreamReader streamReader = new StreamReader (path);
			string text_all = streamReader.ReadToEnd ();
			streamReader.Close ();
			
			int beginIndex = text_all.IndexOf (last);
			if (beginIndex == -1)
			{
				Debug.LogError (path + "中没有找到标致" + last);
				return; 
			}
			
			int endIndex = text_all.LastIndexOf ("\n", beginIndex + last.Length);
			
			text_all = text_all.Substring (0, endIndex) + "\n" + append + "\n" + text_all.Substring (endIndex);
			
			StreamWriter streamWriter = new StreamWriter (path);
			streamWriter.Write (text_all);
			streamWriter.Close ();
		}

		public void Replace (string oldStr, string newStr, string method = "")
		{
			if (!File.Exists (path))
			{
				Debug.Log ("未找到类文件:" + path);
				return;
			}
			bool getMethod = false;
			string[] codes = File.ReadAllLines (path);
			for (int i = 0; i < codes.Length; i++)
			{
				string str = codes [i].ToString ();
				if (string.IsNullOrEmpty (method))
				{
					if (str.Contains (oldStr)) codes.SetValue (newStr, i);
				}
				else
				{
					if (!getMethod)
					{
						getMethod = str.Contains (method);
					}
					if (!getMethod) continue;
					if (str.Contains (oldStr))
					{
						codes.SetValue (newStr, i);
						break;
					}
				}
			}
			File.WriteAllLines (path, codes);
		}
	}
}

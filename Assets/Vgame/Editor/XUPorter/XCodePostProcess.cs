using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCodePostProcess
{

	#if UNITY_EDITOR
	[PostProcessBuild (999)]
	public static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
	{
		if (target != BuildTarget.iOS)
		{
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		// Create a new project object from build target
		XCProject project = new XCProject (pathToBuiltProject);

		// Find and run through all projmods files to patch the project.
		// Please pay attention that ALL projmods files in your project folder will be excuted!
		string[] files = Directory.GetFiles (Application.dataPath, "*.projmods", SearchOption.AllDirectories);
		foreach (string file in files)
		{
			UnityEngine.Debug.Log ("ProjMod File: " + file);
			project.ApplyMod (file);
		}

		//TODO implement generic settings as a module option
		//project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", "iPhone Distribution", "Release");

		CopyIcons (pathToBuiltProject);
		// Finally save the xcode project
		project.Save ();

	}
	#endif
	/// <summary>
	/// 拷贝应用图标
	/// </summary>
	public static void CopyIcons (string pathToBuiltProject)
	{
		string appIconPath = Application.dataPath + "/Editor/XUPorter/AppIcon/";
		string[] iconflies = null;
		if (Directory.Exists (appIconPath))
		{
			iconflies = Directory.GetFiles (appIconPath);	
		}
		if (iconflies != null && iconflies.Length > 0)
		{
			appIconPath = pathToBuiltProject + "/Unity-iPhone/Images.xcassets/AppIcon.appiconset/";
			string[] icons = Directory.GetFiles (appIconPath);
			foreach (string file in icons)
			{
				Debug.Log ("Delete icon:" + file);
				File.Delete (file);
			}
			foreach (string file in iconflies)
			{
				string fileName = file.Substring (file.LastIndexOf ("/") + 1);
				if (fileName.EndsWith (".png") || fileName.EndsWith (".json"))
				{
					Debug.Log ("Icon Name:" + fileName);	
					File.Copy (file, Path.Combine (appIconPath, fileName));
				}
			}
		}
	}

	public static void Log (string message)
	{
		UnityEngine.Debug.Log ("PostProcess: " + message);
	}
}

using UnityEngine;
using System.Collections;
//using System.Security.Cryptography;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

public class CryptographHelper {
	
	/// <summary>
	/// Md5s  base64 string.
	/// </summary>
	/// <returns>
	/// The base64 string.
	/// </returns>
	/// <param name='source'>
	/// Source.
	/// </param>
	public static string CrypfString(string source,string key="")
	{
		byte[] inputs=Encoding.UTF8.GetBytes(source);
        byte[] hash = inputs;//Md5Instance.ComputeHash(inputs);
		string outStr=System.Convert.ToBase64String(hash);
		outStr=outStr.Replace("=","");
		outStr=outStr.Replace(@"/","-");
		return outStr;
	}
	
    ///// <summary>
    ///// Gets the zip Password
    ///// </summary>
    ///// <value>
    ///// The zip PW.
    ///// </value>
    //internal static string zipPWD
    //{
    //    get{
    //        return CrypfString("tap4fun99@@","99@");
    //    }
    //}
	
	#region memeber 
    //private static MD5 md5;
    //public static MD5 Md5Instance
    //{
    //    get{
    //        if(md5==null)
    //            md5=MD5.Create();
    //        return md5;
    //    }
    //}
	
	#endregion
}


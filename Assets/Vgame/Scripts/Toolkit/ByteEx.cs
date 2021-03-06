using UnityEngine;
using Vgame;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace Vgame.ToolKit
{
	/// <summary>
	/// 作者:Venbb
	/// 创建时间:2015/08/10 22:26:23
	/// 描述:字节扩展
	/// </summary>
	public static class ByteEx
	{
		/// <summary>
		/// 解压字节数组
		/// </summary>
		/// <param name="inputBytes">Input bytes.</param>
		public static byte[] Decompress (byte[] inputBytes)
		{
			using (var inputStream = new MemoryStream (inputBytes))
			{
				using (var zipStream = new GZipInputStream (inputStream))
				{
					using (var outStream = new MemoryStream ())
					{
						int size = 2048;
						var outBytes = new byte[size];
						while (size > 0)
						{
							size = zipStream.Read (outBytes, 0, size);
							if (size > 0) outStream.Write (outBytes, 0, size);
						}
						zipStream.Close ();
						return outStream.ToArray ();
					}
				}
			}
		}

		/// <summary>
		/// 压缩字节数组
		/// </summary>
		/// <param name="inputBytes">Input bytes.</param>
		public static byte[] Compress (byte[] inputBytes)
		{
			using (var outStream = new MemoryStream ())
			{
				using (var zipStream = new GZipOutputStream (outStream))
				{
					zipStream.Write (inputBytes, 0, inputBytes.Length);
					zipStream.Close ();	
				}
				return outStream.ToArray ();
			}	
		}
	}
}
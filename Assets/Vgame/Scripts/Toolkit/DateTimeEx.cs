using UnityEngine;
using Vgame;
using System;

namespace Vgame.ToolKit
{
	/// <summary>
	/// 作者:Venbb
	/// 创建时间:2015/08/12 21:21:52
	/// 描述:
	/// </summary>
	public class DateTimeEx
	{
		/// <summary>
		/// 1秒钟ticks
		/// </summary>
		public const long TIME_SECOND_TICKS = 10000000;
		/// <summary>
		/// 1秒
		/// </summary>
		public const long TIME_SECOND = 1;
		/// <summary>
		/// 60秒
		/// </summary>
		public const long TIME_MINUTE = 60;
		/// <summary>
		/// 3600秒
		/// </summary>
		public const long TIME_HOUR = 3600;
		/// <summary>
		/// 86400秒
		/// </summary>
		public const long TIME_DAY = 86400;
		/// <summary>
		/// 604800秒
		/// </summary>
		public const long TIME_WEEK = 604800;
		/// <summary>
		/// 220752000秒
		/// </summary>
		public const long TIME_YEAR = 220752000;
		/// <summary>
		/// 计算机起源时间
		/// </summary>
		public readonly static DateTime UNIX_TIME = new DateTime (1970, 1, 1, 0, 0, 0);
		/// <summary>
		/// 计算机起源时间（东八区区时）
		/// </summary>
		public readonly static DateTime UNIX_CHINA_TIME = UNIX_TIME.AddHours (8);

		public static long ToSeconds (DateTime dt)
		{
			return dt.Ticks / TIME_SECOND_TICKS;
		}

		public static long SecondSpan (DateTime dt1, DateTime dt2)
		{
			return ToSeconds (dt2) - ToSeconds (dt1);
		}

		public static DateTime ToDateTime (long sec)
		{
			return new DateTime (sec * TIME_SECOND_TICKS);
		}

		public static string ToTime (long sec)
		{
			int day = 0, hour = 0, minute = 0, second = 0;
			string retstr = "";

			long remainder;
			day = (int)(sec / TIME_DAY);
			retstr = (day == 0) ? "" : day + ":";

			remainder = sec % TIME_DAY;
			if (remainder != 0)
			{
				hour = (int)(remainder / TIME_HOUR);
			}
			//hour += day * 24;
			retstr += ((retstr.Length > 0 || hour > 0) ? (hour < 10 ? "0" + hour + ":" : hour + ":") : "00:");

			remainder = remainder % TIME_HOUR;
			if (remainder != 0)
			{
				minute = (int)(remainder / TIME_MINUTE);
			}
			retstr += ((retstr.Length > 0 || minute > 0) ? (minute < 10 ? "0" + minute + ":" : minute + ":") : "00:");

			second = (int)(remainder % TIME_MINUTE);

			retstr += (second < 10 ? "0" + second : second + "");
			return retstr;
		}
	}
}
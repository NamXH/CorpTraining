using System;
using Android.Content;
using Android.Views;
using Android.Runtime;
using Java.Util;
using Android.Text.Format;


namespace CorpTraining.Droid
{
	public static class Utils
	{
		public static int getWindowWidth (Context context)
		{
			IWindowManager windowManager = context.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ();
			return windowManager.DefaultDisplay.Width;
		}

		public static int getWindowHeight (Context context)
		{
			IWindowManager windowManager = context.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ();
			return windowManager.DefaultDisplay.Height;
		}

		public static string EncodeURL (string str)
		{
			String url = str.Replace (" ", "%20");
			return url;
		}

		public static string formatMillis (long time)
		{
			Calendar calendar = Calendar.Instance;
			calendar.Clear ();
			calendar.Add (Calendar.Millisecond, (int)time);
			String pattern = time / Constants.hourMillis > 0 ? "kk:mm:ss" : "mm:ss";
			return DateFormat.Format (pattern, calendar);
		}
	}
}


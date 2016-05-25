using System;
using Java.Security;
using System.Collections.Generic;

namespace CorpTraining.Droid
{
	public static class Constants
	{
		//toast dialog
		public const int REGISTER_SUCCESS = 1;
		public const int RETURN_LIST = 2;
		public const int TIME_UP = 3;

		public const int CHANGE_COLOR = 2;

		//toastpositiveandnegative
		public const int EXIT_CHOICE = 1;
		public const int SUBMIT_ANSWERS = 2;
		public const int RETURN_LIST_NEG = 3;

		public const string VIDEO_URL = "video_url";
		public const string LESSON_TITLE = "lesson_title";
		public const string LESSON_DES = "lesson_des";
		/** update system time */
		public static readonly int UPDATE_SYSTEM_TIME = 0;
		/** update current play-time */
		public static readonly int UPDATE_CURRENT_POSITION = 1;
		/** hide control layout */
		public static readonly int HIDE_CTRL_LAYOUT = 2;

		public static readonly long secondMillis = 1000;
		public static readonly long minuteMillis = 60 * secondMillis;
		public static readonly long hourMillis = 60 * minuteMillis;

		//Screens of a specific lesson
		public static List<Screen> screens;

		//sharedpreference name
		public static readonly string PREFERENCE_NAME = "answer";

	}
}


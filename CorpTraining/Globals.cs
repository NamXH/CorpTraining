﻿using System;

namespace CorpTraining
{
	public static class Globals
	{
		public static string AUTH_URL="http://mgl.usask.ca:8080/ct/api/auth/";

		public static string LOGIN_URL= AUTH_URL +"login";

		public static string PROFILE_URL= AUTH_URL +"profile/";

		public static string REGISTER_URL= AUTH_URL +"register/";

		public static string LOGOUT_URL= AUTH_URL +"logout/";

		//Lesson

		public static string LESSONS_URL="http://mgl.usask.ca:8080/ct/api/lessons/";

		public static string SCREENS_URL="screens/";

		public static string OPTIONS_URL="options/";

		public static string IMAGES_URL="images/";

		public static string NEXT_SCREEN_URL="screens/nextscreen/";

		public static string POSITION_SCREEN_URL="screens/position/";

	}
}


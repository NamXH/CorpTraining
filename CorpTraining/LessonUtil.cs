using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Json;

namespace CorpTraining
{
	public static class LessonUtil
	{
		private static string LESSONS_URL="http://mgl.usask.ca:8080/usis/rest/lessons/";

		private static string SCREENS_URL="screens/";

		private static string NEXT_SCREEN_URL="screens/nextscreen/";

		private static string POSITION_SCREEN_URL="screens/position/";



		/// <summary>Get the lesson list.
		/// <para>Returns a list of lessons</para>
		/// </summary>
		public static async Task<IList<Lesson>> GetLessons ()
		{
			IList<Lesson> lessonList;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (LESSONS_URL)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					JArray lessonArray = JArray.Parse(jsonDoc.ToString());
					lessonList = JsonConvert.DeserializeObject<IList<Lesson>>(lessonArray.ToString());
				}
			}
			return lessonList;
		}

		/// <summary>
		/// Get a simple lesson with basic parameters.
		/// </summary>
		public static async Task<Lesson> GetLessonById (int id)
		{
			Lesson lesson;
			JObject lessonJson;

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (LESSONS_URL+id)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					lessonJson = JObject.Parse (jsonDoc.ToString ());
				}
			}
			lesson = lessonJson.ToObject<Lesson> ();
			return lesson;
		}

		/// <summary>
		/// Get a Screen list by lesson
		/// </summary>
		public static async Task<List<Screen>> GetScreensByLesson(int lessonId){

			List<Screen> screenList = new List<Screen>();
			JArray screenArray;
			IList<Option> optionList = new List<Option>();
			IList<Image> imageList = new List<Image>();

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (LESSONS_URL+lessonId+"/"+SCREENS_URL)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					screenArray = JArray.Parse(jsonDoc.ToString());
				}
			}

			foreach (JObject screenJson in screenArray){  
				optionList = await GetOptionsByUrl (screenJson["optionsUrl"].ToString());
				imageList = await GetImagesByUrl (screenJson["imagesUrl"].ToString());
				screenJson ["options"] = JToken.FromObject (optionList);
				screenJson ["images"] = JToken.FromObject (imageList);
				screenList.Add(JsonConvert.DeserializeObject<Screen>(screenJson.ToString()));
			}
			return screenList;
		}

		public static async Task<Screen> GetNextScreenById (int lessonId, int currentScreenId){

			Screen screen;
			JObject screenJson;

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (LESSONS_URL+lessonId+"/"+NEXT_SCREEN_URL+currentScreenId)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					screenJson = JObject.Parse (jsonDoc.ToString ());
				}
			}
			if (screenJson.GetValue("id").ToString().Equals("") || screenJson.GetValue("id") == null)
				return null;
			screen = screenJson.ToObject<Screen> ();
			return screen;
		}

		public static async Task<Screen> GetScreenByPosition(int lessonId, int position){
			Screen screen;
			JObject screenJson;

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (LESSONS_URL+lessonId+"/"+POSITION_SCREEN_URL+position)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					screenJson = JObject.Parse (jsonDoc.ToString ());
				}
			}
			if (screenJson.GetValue("id").ToString().Equals("") || screenJson.GetValue("id") == null)
				return null;
			screen = screenJson.ToObject<Screen> ();
			return screen;

		}

		public static async Task<Screen> GetScreenById(int lessonId, int screenId){
			Screen screen;
			JObject screenJson;

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (LESSONS_URL+lessonId+"/"+SCREENS_URL+screenId)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					screenJson = JObject.Parse (jsonDoc.ToString ());
				}
			}
			if (screenJson.GetValue("id").ToString().Equals("") || screenJson.GetValue("id") == null)
				return null;
			screen = screenJson.ToObject<Screen> ();
			return screen;
		}


		public static async Task<IList<Option>> GetOptionsByUrl(string optionUrl){
			IList<Option> optionList;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (optionUrl)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					JArray optionArray = JArray.Parse(jsonDoc.ToString());
					optionList = JsonConvert.DeserializeObject<IList<Option>>(optionArray.ToString());
				}
			}
			return optionList;
		}

		public static async Task<IList<Image>> GetImagesByUrl (string imageUrl){
			IList<Image> imageList;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (imageUrl)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					JArray imageArray = JArray.Parse(jsonDoc.ToString());
					imageList = JsonConvert.DeserializeObject<IList<Image>>(imageArray.ToString());
				}
			}
			return imageList;
		}






		/// <summary>Get a specific screen.
		/// <para>Returns a screen object from the screen Rest url</para>
		/// </summary>
		public static async Task<Screen> GetScreenByUrl (string screenUrl){

			Screen screen;
			List<Option> optionList = new List<Option>();
			List<Image> imageList = new List<Image>();
			JObject screenJson;

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (screenUrl)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					screenJson = JObject.Parse (jsonDoc.ToString ());
				}
			}

			foreach (string optionUrl in screenJson ["questions"]){  
				Option option = await GetOptionByUrl (optionUrl);
				optionList.Add(option);
			}

			screenJson.Remove ("questions");
			screenJson ["options"] = JToken.FromObject (optionList);

			foreach (string imageUrl in screenJson ["images"]){  
				Image image = await GetImageByUrl (imageUrl);
				imageList.Add(image);
			}

			screenJson.Remove ("questions");
			screenJson ["options"] = JToken.FromObject (optionList);

			screen = screenJson.ToObject<Screen> ();
			return screen;
		}

		/// <summary>Get a specific lesson.
		/// <para>Returns a lesson object from the lesson Rest url</para>
		/// </summary>
		public static async Task<Lesson> GetLessonByUrl (string lessonUrl)
		{
			Lesson lesson;
			List<Screen> screenList = new List<Screen>();
			JObject lessonJson;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (lessonUrl)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					lessonJson = JObject.Parse (jsonDoc.ToString ());
				}
			}

			foreach (string screenUrl in lessonJson ["screenList"]){  
				Screen screen = await GetScreenByUrl (screenUrl);
				screenList.Add(screen);
			}
			lessonJson.Remove ("screenList");
			lessonJson ["screens"] = JToken.FromObject (screenList);
			lesson = lessonJson.ToObject<Lesson> ();
			return lesson;
		}




		/// <summary>Get a specific image.
		/// <para>Returns a image object from the image Rest urle</para>
		/// </summary>
		public static async Task<Image> GetImageByUrl (string imageUrl)
		{
			Image image;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (imageUrl)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					image = JsonConvert.DeserializeObject<Image>(jsonDoc.ToString());
				}
			}
			return image;
		}

		/// <summary>Get a specific option.
		/// <para>Returns an option object from the option Rest url</para>
		/// </summary>
		public static async Task<Option> GetOptionByUrl (string optionUrl)
		{
			Option option;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (optionUrl)); 
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					option = JsonConvert.DeserializeObject<Option>(jsonDoc.ToString());
				}
			}
			return option;
		}
	}
}


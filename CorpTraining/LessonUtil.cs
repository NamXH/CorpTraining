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
		private static string LESSONS_URL="http://mgl.usask.ca:8080/ct/api/lessons/";

		private static string SCREENS_URL="screens/";

		private static string OPTIONS_URL="options/";

		private static string IMAGES_URL="images/";

		private static string NEXT_SCREEN_URL="screens/nextscreen/";

		private static string POSITION_SCREEN_URL="screens/position/";

		private static async Task<JsonValue> MakeServerRequest(string url){
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url)); 
			request.ContentType = "application/json";
			request.Method = "GET";
			JsonValue jsonDoc;
			try{
				using (WebResponse response = await request.GetResponseAsync ())
				{
					using (Stream stream = response.GetResponseStream ())
					{
						jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					}
				}
			}catch (WebException e){
				if(e.Response==null)
					throw new WebException ("Error connecting to the server: " + url +" Possible Internet problems");

				throw new WebException ("Error connecting to the server: " + url +" Status code: " +((HttpWebResponse)e.Response).StatusCode);
			}
			return jsonDoc;

		}


		/// <summary>Get the lesson list.
		/// <para>Returns a list of lessons</para>
		/// </summary>
		public static async Task<IList<Lesson>> GetLessonsAsync ()
		{
			IList<Lesson> lessonList;

			JsonValue jsonDoc = await MakeServerRequest(LESSONS_URL);

			JArray lessonArray = JArray.Parse(jsonDoc.ToString());
			lessonList = JsonConvert.DeserializeObject<IList<Lesson>>(lessonArray.ToString());

			return lessonList;
		}

		/// <summary>
		/// Get a simple lesson with basic parameters.
		/// </summary>
		public static async Task<Lesson> GetLessonByIdAsync (int id)
		{
			Lesson lesson;
			JObject lessonJson;

			JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + id);
			try{
				lessonJson = JObject.Parse (jsonDoc.ToString ());
			}catch(JsonSerializationException){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			lesson = lessonJson.ToObject<Lesson> ();
			return lesson;
		}

		/// <summary>
		/// Get a Screen list by lesson
		/// </summary>
		public static async Task<List<Screen>> GetScreensByLessonAsync (int lessonId){

			List<Screen> screenList = new List<Screen>();
			JArray screenArray;
			IList<Option> optionList = new List<Option>();
			IList<Image> imageList = new List<Image>();
			try
			{
				JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + lessonId + "/" + SCREENS_URL);
				screenArray = JArray.Parse(jsonDoc.ToString());

				foreach (JObject screenJson in screenArray){  
					optionList = await GetOptionsByScreenAsync (lessonId, screenJson["id"].ToString());
					imageList = await GetImagesByScreenAsync (lessonId, screenJson["id"].ToString());
					screenJson ["options"] = JToken.FromObject (optionList);
					screenJson ["images"] = JToken.FromObject (imageList);
					screenList.Add(JsonConvert.DeserializeObject<Screen>(screenJson.ToString()));
				}
			}
			catch(JsonSerializationException)
			{
				throw new JsonSerializationException ("Json couldn't be serialized. ");
			}

			return screenList;
		}

		public static async Task<Screen> GetNextScreenByIdAsync (int lessonId, int currentScreenId){

			Screen screen;
			JObject screenJson;

			JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + lessonId + "/" + NEXT_SCREEN_URL + currentScreenId);

			try{
				screenJson = JObject.Parse (jsonDoc.ToString ());
			}catch(JsonSerializationException){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			if (screenJson.GetValue("id").ToString().Equals("") || screenJson.GetValue("id") == null)
				return null;
			screen = screenJson.ToObject<Screen> ();
			return screen;
		}

		public static async Task<Screen> GetScreenByPositionAsync (int lessonId, int position){
			Screen screen;
			JObject screenJson;

			JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + lessonId + "/" + POSITION_SCREEN_URL + position);

			try{
				screenJson = JObject.Parse (jsonDoc.ToString ());
			}catch(JsonSerializationException){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			if (screenJson.GetValue("id").ToString().Equals("") || screenJson.GetValue("id") == null)
				return null;
			screen = screenJson.ToObject<Screen> ();
			return screen;

		}

		public static async Task<Screen> GetScreenByIdAsync (int lessonId, int screenId){
			Screen screen;
			JObject screenJson;

			JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + lessonId + "/" + SCREENS_URL + screenId);

			try{
				screenJson = JObject.Parse (jsonDoc.ToString ());
			}catch(JsonSerializationException){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			if (screenJson.GetValue("id").ToString().Equals("") || screenJson.GetValue("id") == null)
				return null;
			screen = screenJson.ToObject<Screen> ();
			return screen;
		}

		public static async Task<IList<Option>> GetOptionsByScreenAsync (int lessonId, string screenId){
			IList<Option> optionList;

			JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + lessonId + "/" + SCREENS_URL + screenId + "/" + OPTIONS_URL);

			try{
				JArray optionArray = JArray.Parse(jsonDoc.ToString());
				optionList = JsonConvert.DeserializeObject<IList<Option>>(optionArray.ToString());
			}catch(JsonSerializationException){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			return optionList;
		}

		public static async Task<IList<Image>> GetImagesByScreenAsync (int lessonId, string screenId){
			IList<Image> imageList;

			JsonValue jsonDoc = await MakeServerRequest (LESSONS_URL + lessonId + "/" + SCREENS_URL + screenId + "/" + IMAGES_URL);
			JArray imageArray = JArray.Parse(jsonDoc.ToString());
			imageList = JsonConvert.DeserializeObject<IList<Image>>(imageArray.ToString());

			return imageList;
		}




		/// <summary>Get a specific screen.
		/// <para>Returns a screen object from the screen Rest url</para>
		/// </summary>
		public static async Task<Screen> GetScreenByUrlAsync (string screenUrl){

			Screen screen;
			List<Option> optionList = new List<Option>();
			List<Image> imageList = new List<Image>();
			JObject screenJson;


			JsonValue jsonDoc = await MakeServerRequest (screenUrl);
			try{
				screenJson = JObject.Parse (jsonDoc.ToString ());
			}catch(JsonSerializationException){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			foreach (string optionUrl in screenJson ["questions"]){  
				Option option = await GetOptionByUrlAsync (optionUrl);
				optionList.Add(option);
			}

			screenJson.Remove ("questions");
			screenJson ["options"] = JToken.FromObject (optionList);

			foreach (string imageUrl in screenJson ["images"]){  
				Image image = await GetImageByUrlAsync (imageUrl);
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
		public static async Task<Lesson> GetLessonByUrlAsync (string lessonUrl)
		{
			Lesson lesson;
			List<Screen> screenList = new List<Screen>();
			JObject lessonJson;

			JsonValue jsonDoc = await MakeServerRequest (lessonUrl);
			try{
				lessonJson = JObject.Parse (jsonDoc.ToString ());
			}catch(JsonSerializationException ){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			foreach (string screenUrl in lessonJson ["screenList"]){  
				Screen screen = await GetScreenByUrlAsync (screenUrl);
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
		public static async Task<Image> GetImageByUrlAsync (string imageUrl)
		{
			Image image;

			JsonValue jsonDoc = await MakeServerRequest (imageUrl);
			try{
				image = JsonConvert.DeserializeObject<Image>(jsonDoc.ToString());
			}catch(JsonSerializationException ){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}
			return image;
		}

		/// <summary>Get a specific option.
		/// <para>Returns an option object from the option Rest url</para>
		/// </summary>
		public static async Task<Option> GetOptionByUrlAsync (string optionUrl)
		{
			Option option;

			JsonValue jsonDoc = await MakeServerRequest (optionUrl);

			try{
				option = JsonConvert.DeserializeObject<Option>(jsonDoc.ToString());
			}catch(JsonSerializationException ){
				throw new JsonSerializationException ("Json couldn't be serialized. " + jsonDoc);
			}

			return option;
		}
	}
}


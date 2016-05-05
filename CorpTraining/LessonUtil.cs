using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Json;
using System.Net.Http;
using System.Text;

namespace CorpTraining
{
	public static class LessonUtil
	{
		

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

		private static async Task<HttpResponseMessage> MakeServerPostRequest(string url, StringContent content){
			HttpClient client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;

			var uri = new Uri (string.Format (url));

			HttpResponseMessage response = null;
			try{

				response = await client.PostAsync (uri, content);

			}catch (WebException e){
				if(e.Response==null)
					throw new WebException ("Error connecting to the server: " + url +" Possible Internet problems");

				throw new WebException ("Error connecting to the server: " + url +" Status code: " +((HttpWebResponse)e.Response).StatusCode);
			}
			return response;

		}

		/// <summary>Get the lesson list.
		/// <para>Returns a list of lessons</para>
		/// </summary>
		public static async Task<IList<Lesson>> GetLessonsAsync ()
		{
			IList<Lesson> lessonList;

			JsonValue jsonDoc = await MakeServerRequest(Globals.LESSONS_URL);

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

			JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + id);
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

			try
			{
				JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + lessonId + "/" + Globals.SCREENS_URL);
				screenArray = JArray.Parse(jsonDoc.ToString());

				foreach (JObject screenJson in screenArray){  //TODO Once the server returns every screen parameter in the screen list, this should be modified
					Screen screen = await GetScreenByIdAsync(lessonId,(int)screenJson["id"]);
					screenList.Add(screen);
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

			JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + lessonId + "/" + Globals.NEXT_SCREEN_URL + currentScreenId);

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

			JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + lessonId + "/" + Globals.POSITION_SCREEN_URL + position);

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

			JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + lessonId + "/" + Globals.SCREENS_URL + screenId);

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

			JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + lessonId + "/" + Globals.SCREENS_URL + screenId + "/" + Globals.OPTIONS_URL);

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

			JsonValue jsonDoc = await MakeServerRequest (Globals.LESSONS_URL + lessonId + "/" + Globals.SCREENS_URL + screenId + "/" + Globals.IMAGES_URL);
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

		/// <summary>Sends the answers for a specific lesson.
		/// <para>It will return success if it was succesfull, and null if it wasn't</para>
		/// </summary>
		public static async Task<String> SendLessonAnswers (int lessonId, List<ScreenAnswer> screenAnswers)
		{//TODO handle the answer response from the server when its done

			var jsonAnswers = JsonConvert.SerializeObject (screenAnswers);
			var content = new StringContent (jsonAnswers, Encoding.UTF8, "application/json");

			HttpResponseMessage response = null;

			response = await MakeServerPostRequest(Globals.LESSONS_URL+lessonId+"/"+Globals.ANSWER_URL, content);

			if (response.IsSuccessStatusCode)
				return JsonObject.Parse (response.Content.ReadAsStringAsync ().Result) ["result"];

			return null;
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


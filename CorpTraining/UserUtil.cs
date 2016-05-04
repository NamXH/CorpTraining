using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Json;
using System.Net;

namespace CorpTraining
{
	public class UserUtil
	{

		public UserUtil ()
		{
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

		private static async Task<HttpResponseMessage> MakeServerGetRequest(string url){
			HttpClient client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;

			var uri = new Uri (string.Format (url));

			HttpResponseMessage response = null;
			try{
				
				response = await client.GetAsync (uri);

			}catch (WebException e){
				if(e.Response==null)
					throw new WebException ("Error connecting to the server: " + url +" Possible Internet problems");

				throw new WebException ("Error connecting to the server: " + url +" Status code: " +((HttpWebResponse)e.Response).StatusCode);
			}
			return response;

		}



		/// <summary>Authenticate a user.
		/// <para>Returns the token when the authentication is succesfull and null when it fails</para>
		/// </summary>
		public static async Task<String> AuthenticateUserAsync (string email, string password)
		{

			User user = new User{ Email = email, Password = password };

			var jsonUser = JsonConvert.SerializeObject (user);
			var content = new StringContent (jsonUser, Encoding.UTF8, "application/json");

			HttpResponseMessage response = null;

			response = await MakeServerPostRequest (Globals.LOGIN_URL, content);

			if (response.IsSuccessStatusCode){
				string token = JsonObject.Parse (response.Content.ReadAsStringAsync ().Result) ["token"];
				UserDB userDB = new UserDB();

				userDB.InsertToken (token);
				return token;
			}
			
			return null;
		}

		/// <summary>Get a user Profile by token.
		/// <para></para>
		/// </summary>
		public static async Task<User> GetUserProfileByTokenAsync (string token)
		{
			HttpResponseMessage response = null;
			string url = Globals.PROFILE_URL + token;

			response = await MakeServerGetRequest (url);

			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode)	
				return JsonConvert.DeserializeObject<User> (content);
			
			return null;
		}

		/// <summary>Register a new user
		/// <para>It returns the result that comes from the server: Success, fail or duplicate (duplicated email)</para>
		/// </summary>
		public static async Task<String> RegisterUserAsync (User user)
		{

			var jsonUser = JsonConvert.SerializeObject (user);
			var content = new StringContent (jsonUser, Encoding.UTF8, "application/json");

			HttpResponseMessage response = null;

			response = await MakeServerPostRequest (Globals.REGISTER_URL, content);

			return JsonObject.Parse (response.Content.ReadAsStringAsync ().Result) ["result"];

		}

		/// <summary>Logout a user
		/// <para></para>
		/// </summary>
		public static async Task<String> LogOutUserByTokenAsync (string token)
		{

			HttpResponseMessage response = null;
			string url = Globals.LOGOUT_URL + token;

			response = await MakeServerGetRequest (url);

			UserDB userDB = new UserDB();

			userDB.DeleteToken ();

			return JsonObject.Parse (response.Content.ReadAsStringAsync ().Result) ["result"];

		}

		/// <summary>Get the current application token.
		/// <para>If a token is retrieved, it means a user is logged, if not, the user need to log in</para>
		/// </summary>
		public static  String GetCurrentToken ()
		{

			UserDB userDB = new UserDB();
			return userDB.GetToken ();

		}




	}
}


using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Json;
using System.Net;

namespace CorpTraining
{
    public static class UserUtil
    {
        public static User CurrentUser { get; set; }

        // Workaround
        public static User CurrentUserTemp { get; set; }

        public static async Task<User> GetCurrentUserAsync()
        {
            if (CurrentUserTemp == null)
            {
                var tk = GetCurrentToken();
                var response = await GetUserProfileByTokenAsync(GetCurrentToken());
                if (response.Item1)
                {
                    CurrentUserTemp = response.Item2;
                }
            }

            return CurrentUserTemp;
        }

        private static async Task<HttpResponseMessage> MakeServerPostRequest(string url, StringContent content)
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            var uri = new Uri(string.Format(url));

            HttpResponseMessage response = null;
            try
            {

                response = await client.PostAsync(uri, content);

            }
            catch (WebException e)
            {
                if (e.Response == null)
                    throw new WebException("Error connecting to the server: " + url + " Possible Internet problems");

                throw new WebException("Error connecting to the server: " + url + " Status code: " + ((HttpWebResponse)e.Response).StatusCode);
            }
            return response;

        }

        private static async Task<HttpResponseMessage> MakeServerGetRequest(string url)
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            var uri = new Uri(string.Format(url));

            HttpResponseMessage response = null;
            try
            {

                response = await client.GetAsync(uri);

            }
            catch (WebException e)
            {
                if (e.Response == null)
                    throw new WebException("Error connecting to the server: " + url + " Possible Internet problems");

                throw new WebException("Error connecting to the server: " + url + " Status code: " + ((HttpWebResponse)e.Response).StatusCode);
            }
            return response;

        }

        // Not usable now. See To do!!
        private static async Task<bool> StoreUserDataLocally(string token)
        {
            var response = await GetUserProfileByTokenAsync(token);
            CurrentUser = response.Item1 ? response.Item2 : null; // To do: store the data in the Database instead of this variable !!
            return response.Item1;
        }

        private static async Task<bool> StoreUserDataLocally()
        {
            var token = GetCurrentToken();
            return await StoreUserDataLocally(token);
        }

        /// <summary>Authenticate a user.
        ///<para>Returns Tuple<result, token></para>
        /// </summary>
        public static async Task<Tuple<string, string>> AuthenticateUserAsync(string email, string password)
        {

            User user = new User{ Email = email, Password = password };

            var jsonUser = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await MakeServerPostRequest(Globals.LOGIN_URL, content);

            string result = JsonObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];

            UserDB userDB = new UserDB();

            if (response.IsSuccessStatusCode)
            {
                string token = JsonObject.Parse(response.Content.ReadAsStringAsync().Result)["token"]; // @Juan: Why ReadAsStringAsync twice?? Why blocking the thread with .Result??
                userDB.InsertToken(token);

                await StoreUserDataLocally(token); // The server should return user data right away so we don't have to make another request!!
                return new Tuple<string, string>(result, token);
            }

            userDB.DeleteToken();
            return new Tuple<string, string>(result, null);
        }

        /// <summary>Get a user Profile by token.
        /// <para>Returns Tuple<true, User> when succesfull and Tuple<false, null> when not </para>
        /// </summary>
        public static async Task<Tuple<bool, User>> GetUserProfileByTokenAsync(string token)
        {
            if (token == null)
                return new Tuple<bool, User>(false, null);
			
            HttpResponseMessage response = null;
            string url = Globals.PROFILE_URL + token;

            response = await MakeServerGetRequest(url);

            var content = await response.Content.ReadAsStringAsync();

            User user = JsonConvert.DeserializeObject<User>(content);
//            if (user.Email != null && user.FirstName != null && user.Id != null && user.LastName != null && user.Phone != null)
//                return new Tuple<bool, User>(true, user);
            if (user.Id != null)
            {
                return new Tuple<bool, User>(true, user);
            }

            return new Tuple<bool, User>(false, null);
        }

        /// <summary>Register a new user
        /// <para>It returns the result that comes from the server in a tuple with a bool saying if it was succesfull or not</para>
        /// </summary>
        public static async Task<Tuple<bool, string>> RegisterUserAsync(User user)
        {

            var jsonUser = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await MakeServerPostRequest(Globals.REGISTER_URL, content);

            string result = JsonObject.Parse(response.Content.ReadAsStringAsync().Result)["result"];
            if (response.IsSuccessStatusCode)
            {
                return new Tuple<bool, string>(true, result);

            }
            else
            {
                return new Tuple<bool, string>(false, result);
            }
        }

        // Should make this function make less requests to server!! We want to login user right after successfully register!!
        // Right now the registration is successful but I cannot authenticate that user??? They don't use the password provided?
        public static async Task<Tuple<bool, string>> RegisterUserThenLoginAsync(User user)
        {
            var registrationResponse = await RegisterUserAsync(user);

            if (registrationResponse.Item1)
            {
                var authResponse = await AuthenticateUserAsync(user.Email, user.Password);

            }

            return registrationResponse;
        }


        /// <summary>Logout a user
        /// <para></para>
        /// </summary>
        public static void LogOutUserByTokenAsync(string token)
        {

            UserDB userDB = new UserDB();
            userDB.DeleteToken();

        }

        /// <summary>Get the current application token.
        /// <para>If a token is retrieved, it means a user is logged, if not, the user need to log in</para>
        /// </summary>
        public static  String GetCurrentToken()
        {

            UserDB userDB = new UserDB();
            return userDB.GetToken();

        }




    }
}


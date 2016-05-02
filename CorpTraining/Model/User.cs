using System;
using Newtonsoft.Json;

namespace CorpTraining
{
	public class User
	{
		[JsonProperty(PropertyName = "id",NullValueHandling=NullValueHandling.Ignore)]
		public int? Id { get; set;}

		[JsonProperty(PropertyName = "firstName",NullValueHandling=NullValueHandling.Ignore)]
		public string FirstName { get; set;}

		[JsonProperty(PropertyName = "lastName",NullValueHandling=NullValueHandling.Ignore)]
		public string LastName { get; set;}

		[JsonProperty(PropertyName = "email",NullValueHandling=NullValueHandling.Ignore)]
		public string Email { get; set;}

		[JsonProperty(PropertyName = "phone",NullValueHandling=NullValueHandling.Ignore)]
		public string Phone { get; set;}

		[JsonProperty(PropertyName = "password",NullValueHandling=NullValueHandling.Ignore)]
		public string Password { get; set;}


		public User ()
		{
		}
	}
}


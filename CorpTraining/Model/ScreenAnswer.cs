using System;
using Newtonsoft.Json;

namespace CorpTraining
{
	public class ScreenAnswer
	{
		[JsonProperty(PropertyName = "screenId")]
		public int ScreenId { get; set;}

		[JsonProperty(PropertyName = "option")]
		public string Option { get; set;}

		[JsonProperty(PropertyName = "userId")]
		public int UserId{ get; set;}

		public ScreenAnswer ()
		{
		}
	}
}


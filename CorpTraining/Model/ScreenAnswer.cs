using System;
using Newtonsoft.Json;

namespace CorpTraining
{
	public class ScreenAnswer
	{
		[JsonProperty(PropertyName = "optionId")]
		public int OptionId { get; set;}

		[JsonProperty(PropertyName = "screenId")]
		public int ScreenId { get; set;}

		[JsonProperty(PropertyName = "userId")]
		public int UserId{ get; set;}

		public ScreenAnswer ()
		{
		}
	}
}


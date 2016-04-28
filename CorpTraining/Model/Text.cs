using System;
using Newtonsoft.Json;

namespace CorpTraining
{
	public class Text
	{
		public int Order {get; set;}

		[JsonProperty(PropertyName = "text")]
		public string TextValue { get; set;}

		public Text ()
		{
		}
	}
}


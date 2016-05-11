using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CorpTraining
{
	public class Screen
	{
		public int Id { get; set;}

		public string Type { get; set;}

		public string Text { get; set;}

		public IList<Image> Images { get; set;}

		public string Question { get; set;}

		[JsonProperty(PropertyName = "Audio_url")]
		public string AudioUrl {get; set;}

		[JsonProperty(PropertyName = "Video_url")]
		public string VideoUrl { get; set;}

		public IList<Option> Options { get; set;} 

		public int Position { get; set;} 

		public string title { get; set;}

		[JsonProperty(PropertyName = "textList")]
		public IList<Text> Texts { get; set;} 

		public string Media { get; set;}

		public Screen ()
		{
		}
	}
}


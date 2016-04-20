using System;
using System.Collections.Generic;

namespace CorpTraining
{
	public class Screen
	{
		public int id { get; set;}
		public string type { get; set;}
		public string text { get; set;}
		public IList<Image> images { get; set;}
		public string question { get; set;}
		public string audio_url {get; set;}
		public string video_url { get; set;}
		public IList<Option> options { get; set;} 
		public int position { get; set;} 

		public Screen ()
		{
		}
	}
}


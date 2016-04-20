using System;
using System.Collections.Generic;

namespace CorpTraining
{
	public class Lesson
	{

		public int id { get; set;}
		public string description { get; set;}
		public IList<Screen> screens { get; set;}
		public int screenCount { get; set;}
		public string title { get; set;}
		public string url { get; set;}

		public Lesson ()
		{
		}
	}
}


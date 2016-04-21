using System;
using System.Collections.Generic;

namespace CorpTraining
{
	public class Lesson
	{

		public int Id { get; set;}
		public string Description { get; set;}
		public IList<Screen> Screens { get; set;}
		public int ScreenCount { get; set;}
		public string Title { get; set;}
		public string Url { get; set;}

		public Lesson ()
		{
		}
	}
}


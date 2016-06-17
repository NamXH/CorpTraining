using System;

namespace CorpTraining.Droid
{

	public class SortModel:Java.Lang.Object,IComparable
	{
		private String name;
		//data
		private string sortLetters;
		//user_id
		private int lessonId;

		private int screenCount;

		public int ScreenCount {
			get {
				return this.screenCount;
			}
			set {
				screenCount = value;
			}
		}
		//first letter of data
		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}

		public int LessonId {
			get {
				return this.lessonId;
			}
			set {
				lessonId = value;
			}
		}

		public string SortLetters {
			get {
				return this.sortLetters;
			}
			set {
				sortLetters = value;
			}
		}

		public int CompareTo (object obj)
		{
			var sort = obj as SortModel;
			if (sort.sortLetters == "@" || this.sortLetters == "#")
				return -1;
			else if (sort.sortLetters == "#" || this.sortLetters == "@")
				return 1;
			else {
				return this.sortLetters.CompareTo (sort.sortLetters);
			}
		}
	}
}


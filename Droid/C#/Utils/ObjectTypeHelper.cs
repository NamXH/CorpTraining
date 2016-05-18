using System;

namespace CorpTraining.Droid
{
	public class JavaObjectWrapper<T> : Java.Lang.Object
	{
		public T Obj { get; set; }
	}
}


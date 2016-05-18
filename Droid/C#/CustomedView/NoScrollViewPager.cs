using System;
using Android.Support.V4.View;

namespace CorpTraining.Droid
{
	public class NoScrollViewPager:ViewPager
	{
		public NoScrollViewPager (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}


		public NoScrollViewPager (Android.Content.Context context) : base (context)
		{
		}


		public NoScrollViewPager (Android.Content.Context context, Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
		}

		public override bool OnTouchEvent (Android.Views.MotionEvent e)
		{
			return false;
		}

		public override Boolean OnInterceptTouchEvent (Android.Views.MotionEvent ev)
		{
			return false;
		}

	}
}


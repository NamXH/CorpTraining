using System;
using Android.Support.V4.View;
using Android.App;
using Android.Views;

namespace CorpTraining.Droid
{
	public class MyPagerAdapter:PagerAdapter
	{
		private HomeActivity hactivity;

		public MyPagerAdapter (HomeActivity activity)
		{
			hactivity = activity;
		}

		public override void DestroyItem (Android.Views.ViewGroup container, int position, Java.Lang.Object objectValue)
		{
			container.RemoveView ((View)objectValue);
		}

		public override Java.Lang.Object InstantiateItem (Android.Views.ViewGroup container, int position)
		{
			BasePager pager = hactivity.pagerList [position];
			container.AddView (pager.rootView);
			return pager.rootView;
		}

		public override bool IsViewFromObject (Android.Views.View view, Java.Lang.Object objectValue)
		{
			return view == objectValue;
		}

		public override int Count {
			get {
				return hactivity.pagerList.Count;
			}
		}
		
	}
}


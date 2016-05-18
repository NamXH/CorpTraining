using System;
using Android.Views;

namespace CorpTraining.Droid
{
	public class SettingsPager:BasePager
	{
		public SettingsPager (Android.App.Activity activity) : base (activity)
		{
		}

		public override void initData ()
		{
			View view = View.Inflate (activity, Resource.Layout.viewpager_settingspager, null);
			fl_content.AddView (view);
		}
	}
}


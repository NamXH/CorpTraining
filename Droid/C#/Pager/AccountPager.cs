using System;
using Android.Views;
using System.Security.Cryptography;

namespace CorpTraining.Droid
{
	public class AccountPager:BasePager
	{
		public AccountPager (Android.App.Activity activity) : base (activity)
		{
		}

		public override void initData ()
		{
			View view = View.Inflate (activity, Resource.Layout.viewpager_accountpager, null);
			fl_content.AddView (view);
		}
	}
}


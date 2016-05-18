using System;
using Android.App;
using Android.Views;
using System.Security.Cryptography;
using Android.Widget;

namespace CorpTraining.Droid
{
	public abstract class BasePager
	{
		public Activity activity;
		public View rootView;
		public FrameLayout fl_content;

		public BasePager (Activity activity)
		{
			this.activity = activity;
			initViews ();
		}

		public void initViews ()
		{
			rootView = View.Inflate (activity, Resource.Layout.base_pager, null);
			fl_content = rootView.FindViewById<FrameLayout> (Resource.Id.fl_content);
		}

		public abstract void initData ();
	}
}


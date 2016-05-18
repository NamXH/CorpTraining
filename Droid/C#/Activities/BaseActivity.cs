
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining")]			
	public abstract class BaseActivity : Activity,UIOperation
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature (WindowFeatures.NoTitle);
			SetContentView (getLayoutResource ());
			initView ();
			initListner ();
			initData ();

		}

		/// <summary>
		/// Initiate the listner
		/// </summary>
		public abstract  void initListner ();

		/// <summary>
		/// initiate data
		/// </summary>
		public abstract void initData ();

		/// <summary>
		/// Inits the view.
		/// </summary>
		public abstract void initView ();

		/// <summary>
		/// Gets the layout resource.
		/// </summary>
		/// <returns>The layout resource.</returns>
		public abstract int getLayoutResource ();
	}
}


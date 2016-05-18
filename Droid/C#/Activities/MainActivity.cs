using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Android.Views.Animations;
using Android.Graphics;

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]	
	public class LandingActivity : BaseActivity
	{
		private Button btn_start;
		private Handler handler = new Handler ();
		private TextView tv_maintitle;
		private LinearLayout ll_root;

		public override void initListner ()
		{
			btn_start.Click += delegate(object sender, EventArgs e) {
				btn_start.SetTextColor (Color.Black);
				handler.RemoveCallbacksAndMessages (null);
				enterLoginActivity ();	
			};
		}

		public override void initData ()
		{
			handler.PostDelayed (delegate() {
				enterLoginActivity ();
			}, 8000);
		}

		public override void initView ()
		{
			btn_start = FindViewById<Button> (Resource.Id.btn_start);
			tv_maintitle = FindViewById<TextView> (Resource.Id.tv_maintitle);
			ll_root = FindViewById<LinearLayout> (Resource.Id.ll_root);
			//ll_root animation
			AnimationSet set = new AnimationSet (true);
			//rotate animation
			RotateAnimation rotate = new RotateAnimation (0, 360, Dimension.RelativeToSelf, 0.5f,
				                         Dimension.RelativeToSelf, 0.5f);
			//scale animation
			ScaleAnimation scaleAnimation = new ScaleAnimation (0.0f, 1.0f, 0.0f, 1.0f);
			set.AddAnimation (rotate);
			set.AddAnimation (scaleAnimation);
			set.Duration = 2000;
			set.FillAfter = true;
			ll_root.StartAnimation (set);
			set.AnimationEnd += delegate(object sender, Animation.AnimationEndEventArgs e) {
				handler.PostDelayed (delegate() {
					NineOldAndroids.View.ViewPropertyAnimator.Animate (tv_maintitle).SetDuration (1200).Alpha (1.0f).Start ();
				}, 2000);
				handler.PostDelayed (delegate() {
					btn_start.Visibility = ViewStates.Visible;
					NineOldAndroids.View.ViewPropertyAnimator.Animate (btn_start).SetDuration (1500).TranslationY (-150).Start ();
				}, 1200);
			};
		}

		/// <summary>
		/// Enters the login activity.
		/// </summary>
		void enterLoginActivity ()
		{
			StartActivity (new Intent (this, typeof(LoginActivity)));
			Finish ();
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_main;
		}

		public override void OnBackPressed ()
		{

		}
	}
}



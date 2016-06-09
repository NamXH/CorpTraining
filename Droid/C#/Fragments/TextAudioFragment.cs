
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Graphics;
using Android.Views.Animations;

namespace CorpTraining.Droid
{
	public class TextAudioFragment : Fragment
	{
		private MediaPlayer mp = new MediaPlayer ();
		private Screen screen;
		private LinearLayout ll_text;
		private List<Text> texts;

		public TextAudioFragment (Screen screen)
		{
			this.screen = screen;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_textaudio, container, false);
			ll_text = view.FindViewById<LinearLayout> (Resource.Id.ll_text);
			//dynamically make text
			if (screen.Texts == null) {
				TextView textview = new TextView (Activity);
				var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 0, 1.0f);
				textview.SetTextColor (Color.White);
				textview.SetTextSize (ComplexUnitType.Sp, 20.0f);
				textview.Gravity = GravityFlags.Start;
				textview.Text = "Enter here...";
				ll_text.AddView (textview);
			} else {
				texts = new List<Text> (screen.Texts);
				if (texts != null && texts.Count > 0) {
					foreach (var text in texts) {
						TextView textview = new TextView (Activity);
						var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 0, 1.0f);
						textview.SetTextColor (Color.White);
						textview.SetTextSize (ComplexUnitType.Sp, 20.0f);
						textview.Gravity = GravityFlags.Start;
						textview.Text = text.TextValue;
						ll_text.AddView (textview);
					}
				}
			}
			var activity = Activity as ScreensActivity;
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			mp.Prepared += delegate(object sender, EventArgs e) {
				activity.validateBtns ();
			};
			return view;
		}

		public override void OnDestroy ()
		{
			ScreensActivity.handler.RemoveCallbacksAndMessages (null);//remove all messages
			mp.Stop ();
			mp.Release ();
			base.OnDestroy ();
		}
	}
}


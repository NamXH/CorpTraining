﻿
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
using Android.Graphics;

namespace CorpTraining.Droid
{
	public class VideoTextFragment : BaseVideoFragment
	{
		private LinearLayout ll_text;
		private List<Text> texts;

		public VideoTextFragment (Screen screen) : base (screen)
		{
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.fragment_videotext;
		}

		public override string getVideoUrl ()
		{
			return screen.VideoUrl;
		}


		public override void init ()
		{
			var activity = Activity as ScreensActivity;
			ll_text = rootView.FindViewById<LinearLayout> (Resource.Id.ll_text);
			//dynamically make text
			if (screen.Texts == null) {
				TextView textview = new TextView (activity);
				var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 0, 1.0f);
				textview.SetTextColor (Color.White);
				textview.SetTextSize (ComplexUnitType.Sp, 20.0f);
				textview.Gravity = GravityFlags.Start;
				textview.Text = "Watching Video...";
				ll_text.AddView (textview);
			} else {
				texts = new List<Text> (screen.Texts);
				if (texts != null && texts.Count > 0) {
					foreach (var text in texts) {
						TextView textview = new TextView (activity);
						var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 0, 1.0f);
						textview.SetTextColor (Color.White);
						textview.SetTextSize (ComplexUnitType.Sp, 20.0f);
						textview.Gravity = GravityFlags.Start;
						textview.Text = text.TextValue;
						ll_text.AddView (textview);
					}
				}
			}
		}

	}
}


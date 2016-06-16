
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
			Utils.makeTextViews (screen.Texts, this.Activity, ll_text, Color.White);
		}

	}
}



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

namespace CorpTraining.Droid
{
	public class VideoTextFragment : BaseVideoFragment
	{
		public EditText et_answer;
		private TextView tv_answertitle;

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
			et_answer = rootView.FindViewById<EditText> (Resource.Id.et_answer);
			tv_answertitle = rootView.FindViewById<TextView> (Resource.Id.tv_answertitle);
			var activity = Activity as ScreensActivity;
			tv_answertitle.Text = (screen.Text == null) ? "Enter here" : screen.Text;
			if (activity.answer.ContainsKey (screen.Id)) {
				//contains
				et_answer.Text = activity.answer [screen.Id];
			}
		}

	}
}


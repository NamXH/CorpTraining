
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

namespace CorpTraining.Droid
{
	public class TextEssayAudioImageFragment : Fragment
	{
		public MediaPlayer mp = new MediaPlayer ();
		private ImageView iv1;
		private TextView subtxt1;
		private Screen screen;
		private List<Image> images;
		private LinearLayout ll_images;
		private LinearLayout ll_text;
		private List<Text> texts;
		public EditText et_answer;

		public TextEssayAudioImageFragment (Screen screen)
		{
			this.screen = screen;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_textessayaudioimagefragment, container, false);
			et_answer = view.FindViewById<EditText> (Resource.Id.et_answer);
			ScreensActivity activity = Activity as ScreensActivity;
			if (activity.answer.ContainsKey (screen.Id)) {
				et_answer.Text = activity.answer [screen.Id];
			}
			ll_text = view.FindViewById<LinearLayout> (Resource.Id.ll_text);
			//dynamically make text
			Utils.makeTextViews (screen.Texts, this.Activity, ll_text);
			ll_images = view.FindViewById<LinearLayout> (Resource.Id.ll_images);
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			mp.Prepared += delegate(object sender, EventArgs e) {
				activity.validateBtns ();
			};
			//dynamically make image+textview
			images = new List<Image> (screen.Images);
			Utils.makeTextImages (activity, ll_images, images);
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


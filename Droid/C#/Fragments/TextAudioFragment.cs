
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

namespace CorpTraining.Droid
{
	public class TextAudioFragment : Fragment
	{
		private MediaPlayer mp = new MediaPlayer ();
		private Screen screen;
		public EditText editText1;

		public TextAudioFragment (Screen screen)
		{
			this.screen = screen;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_textaudio, container, false);
			view.FindViewById<TextView> (Resource.Id.displayText).Text = ((screen.Text == null) ? "No text" : screen.Text);
			editText1 = view.FindViewById<EditText> (Resource.Id.editText1);
			var activity = Activity as ScreensActivity;
			if (activity.answer.ContainsKey (screen.Id)) {
				//contains
				editText1.Text = activity.answer [screen.Id];
			}
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			return view;
		}

		public override void OnDestroy ()
		{
			ScreensActivity.handler.RemoveCallbacksAndMessages (null);//remove all messages
			mp.Stop ();
			base.OnDestroy ();
		}
	}
}


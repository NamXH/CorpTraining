
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
	public class AudioVideoTextFragment : BaseVideoFragment
	{
		public EditText et_answer;
		private TextView tv_answertitle;
		private MediaPlayer mp = new MediaPlayer ();

		public AudioVideoTextFragment (Screen screen) : base (screen)
		{
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.fragment_audiovideotext;
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
			if (isWatched) {
				Utils.setAndPlayMusic (Activity, rootView, screen.AudioUrl, ScreensActivity.handler, mp);
			} else {
				vv.Completion += delegate(object sender, IO.Vov.Vitamio.MediaPlayer.CompletionEventArgs e) {
					vv.SeekTo (0);
					tv_current_position.Text = Utils.formatMillis (0L);
					//set iswatched
					editor.PutBoolean (screen.Id + "", true).Commit ();
					Utils.setAndPlayMusic (Activity, rootView, screen.AudioUrl, ScreensActivity.handler, mp);
				};
			}
			mp.Prepared += delegate(object sender, EventArgs e) {
				activity.validateBtns ();
			};
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


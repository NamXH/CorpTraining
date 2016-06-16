
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
	public class AudioVideoTextFragment : BaseVideoFragment
	{
		private TextView tv_answertitle;
		private MediaPlayer mp = new MediaPlayer ();
		private LinearLayout ll_text;
		private List<Text> texts;

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
			ll_text = rootView.FindViewById<LinearLayout> (Resource.Id.ll_text);
			var activity = Activity as ScreensActivity;
			Utils.makeTextViews (screen.Texts, this.Activity, ll_text, Color.White);
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


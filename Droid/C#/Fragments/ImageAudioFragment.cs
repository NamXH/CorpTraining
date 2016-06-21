
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
	public class ImageAudioFragment : Fragment
	{
		public MediaPlayer mp = new MediaPlayer ();
		private ImageView iv1;
		private TextView subtxt1;
		private Screen screen;
		private List<Image> images;
		private LinearLayout ll_images;

		public ImageAudioFragment (Screen screen)
		{
			this.screen = screen;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_imageaudio, container, false);
			ll_images = view.FindViewById<LinearLayout> (Resource.Id.ll_images);
			var activity = Activity as ScreensActivity;
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			mp.Prepared += delegate(object sender, EventArgs e) {
				activity.validateBtns ();
			};
			//dynamically make textview
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


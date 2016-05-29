
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
using Android.Text;

namespace CorpTraining.Droid
{
	public class TextImageAudioFragment : Fragment
	{
		public MediaPlayer mp = new MediaPlayer ();
		private ImageView iv1;
		private TextView subtxt1;
		private Screen screen;
		private List<Image> images;
		private LinearLayout ll_images;
		public EditText et_note;

		public TextImageAudioFragment (Screen screen)
		{
			this.screen = screen;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_textimageaudio, container, false);
			view.FindViewById<TextView> (Resource.Id.questionTxt).Text = screen.Text;
			ll_images = view.FindViewById<LinearLayout> (Resource.Id.ll_images);
			et_note = view.FindViewById<EditText> (Resource.Id.et_note);
			var activity = Activity as ScreensActivity;
			if (activity.answer.ContainsKey (screen.Id)) {
				//contains
				et_note.Text = activity.answer [screen.Id];
			}
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			mp.Prepared += delegate(object sender, EventArgs e) {
				activity.validateBtns ();
			};
			//dynamically make textview
			images = new List<Image> (screen.Images);
			if (images != null && images.Count > 0) {
				foreach (var image in images) {
					//imageview
					ImageView iv = new ImageView (this.Context);
					var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 0, 4.0f);
					//set default image
					iv.SetImageResource (Resource.Mipmap.default_bitmap);
					Utils.setImageView (iv, image.Url);
					ll_images.AddView (iv, param);
					//textview
					TextView tv = new TextView (this.Context);
					var textparam = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 0, 1.0f);
					tv.Text = image.Title;
					ll_images.AddView (tv, textparam);
				}
			}

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



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
			ll_images = view.FindViewById<LinearLayout> (Resource.Id.ll_images);
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			mp.Prepared += delegate(object sender, EventArgs e) {
				activity.validateBtns ();
			};
			//dynamically make textview
			images = new List<Image> (screen.Images);
			if (images != null && images.Count > 0) {
				foreach (var image in images) {
					//imageview
					ImageView iv = new ImageView (activity);
					var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 0, 4.0f);
					//set default image
					iv.SetImageResource (Resource.Mipmap.default_bitmap);
					Utils.setImageView (iv, image.Url);
					ll_images.AddView (iv, param);
					//textview
					TextView tv = new TextView (activity);
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



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
	public class TextQuestionAudioImageFragment : Fragment
	{
		public MediaPlayer mp = new MediaPlayer ();
		private ImageView iv1;
		private TextView subtxt1;
		private Screen screen;
		private List<Image> images;
		private LinearLayout ll_images;
		private LinearLayout ll_text;
		private List<Text> texts;

		public ViewGroup choicesRadioGroup;
		public List<Option> options;
		private int lesson_id;
		private TextView questionTxt;

		public TextQuestionAudioImageFragment (Screen screen, int id)
		{
			this.screen = screen;
			this.lesson_id = id;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_textquestionaudioimage, container, false);
			ll_text = view.FindViewById<LinearLayout> (Resource.Id.ll_text);
			//dynamically make text
			Utils.makeTextViews (screen.Texts, this.Activity, ll_text, Color.White);
			ll_images = view.FindViewById<LinearLayout> (Resource.Id.ll_images);
			var activity = Activity as ScreensActivity;
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
			choicesRadioGroup = (ViewGroup)view.FindViewById<RadioGroup> (Resource.Id.choicesRadioGrp);
			questionTxt = view.FindViewById<TextView> (Resource.Id.questionTxt);
			populateChoices (view);
			return view;
		}

		public override void OnDestroy ()
		{			
			ScreensActivity.handler.RemoveCallbacksAndMessages (null);//remove all messages
			mp.Stop ();
			mp.Release ();
			base.OnDestroy ();
		}

		public async void populateChoices (View view)
		{
			//query to server
			int id = -1;
			options = new List<Option> (await LessonUtil.GetOptionsByScreenAsync (lesson_id, screen.Id + ""));
			var activity = Activity as ScreensActivity;
			if (activity.answer.ContainsKey (screen.Id)) {
				id = int.Parse (activity.answer [screen.Id]);
			}
			for (int i = 0; i < options.Count; i++) {
				Option option = options [i];
				if (option != null) {
					RadioButton rdBtn = new RadioButton (Application.Context);
					rdBtn.Id = (i);
					rdBtn.Text = option.Title;
					rdBtn.SetTextSize (ComplexUnitType.Sp, 18.0f);
					if (i == id) {
						//set checked
						rdBtn.Checked = true;
					}
					choicesRadioGroup.AddView (rdBtn);
				}
			}
			//remove default selected
			if (id == -1) {
				(choicesRadioGroup as RadioGroup).ClearCheck ();
			}
			activity.validateBtns ();
		}
	}
}


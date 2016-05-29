
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
	public class QuestionAudioFragment : Fragment
	{
		private Screen screen;
		private TextView questionTxt;
		public ViewGroup choicesRadioGroup;
		public List<Option> options;
		private int lesson_id;
		MediaPlayer mp = new MediaPlayer ();

		public QuestionAudioFragment (Screen screen, int id)
		{
			this.screen = screen;
			this.lesson_id = id;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_questionaudio, container, false);
			choicesRadioGroup = (ViewGroup)view.FindViewById<RadioGroup> (Resource.Id.choicesRadioGrp);
			questionTxt = view.FindViewById<TextView> (Resource.Id.questionTxt);
			questionTxt.Text = (screen.Question == null) ? "No question" : screen.Question;
			Utils.setAndPlayMusic (Activity, view, screen.AudioUrl, ScreensActivity.handler, mp);
			populateChoices (view);
			mp.Prepared += delegate(object sender, EventArgs e) {
				var activity = Activity as ScreensActivity;
				activity.validateBtns ();
			};
			return view;
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


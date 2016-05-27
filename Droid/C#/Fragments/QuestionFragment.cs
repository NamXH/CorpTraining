
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
using Android.Content.PM;

namespace CorpTraining.Droid
{
	public class QuestionFragment : Fragment
	{
		private Screen screen;
		private TextView questionTxt;
		private ViewGroup choicesRadioGroup;
		private List<Option> options;
		private int lesson_id;

		public QuestionFragment (Screen screen, int id)
		{
			this.screen = screen;
			this.lesson_id = id;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.fragment_question, container, false);
			choicesRadioGroup = (ViewGroup)view.FindViewById<RadioGroup> (Resource.Id.choicesRadioGrp);
			questionTxt = view.FindViewById<TextView> (Resource.Id.questionTxt);
			populateChoices (view);
			return view;
		}

		public async void populateChoices (View view)
		{
			//query to server
			options = new List<Option> (await LessonUtil.GetOptionsByScreenAsync (lesson_id, screen.Id + ""));
			for (int i = 0; i < options.Count; i++) {
				Option option = options [i];
				if (option != null) {
					RadioButton rdBtn = new RadioButton (Application.Context);
					rdBtn.Id = (i);
					rdBtn.Text = option.Title;
					choicesRadioGroup.AddView (rdBtn);
				}
			}
			(choicesRadioGroup as RadioGroup).ClearCheck ();
		}
	}
}


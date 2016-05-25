
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V4.View;

namespace CorpTraining.Droid
{
	[Activity (Label = "Screens", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]				
	public class ScreensActivity : BaseActivity
	{
		private Button nextBtn;
		private Button previousBtn;
		private int currentScreen = 0;
		private int position = 0;
		private Fragment fragment;
		private Button btn_submit;
		private ImageView iv_vision;
		private Screen screen;
		public static Handler handler = new Handler ();
		private LinearLayout ll_loading;
		private LinearLayout ll_questions;
		public TextView tv_timer;
		private int duration;
		private string lesson_title;
		private string lesson_des;
		private int screenCount = 0;
		public Dictionary<int,string> answer;
		//record the answer
		public override void initListner ()
		{
			
		}

		public override void initData ()
		{
			
		}

		public override void initView ()
		{
			//find views
			nextBtn = FindViewById<Button> (Resource.Id.btn_next);
			previousBtn = FindViewById<Button> (Resource.Id.btn_pre);
			fragment = FragmentManager.FindFragmentById (Resource.Id.fragmentContainer);
			btn_submit = FindViewById<Button> (Resource.Id.btn_submit);
			ll_loading = FindViewById<LinearLayout> (Resource.Id.ll_loading);
			ll_questions = FindViewById<LinearLayout> (Resource.Id.ll_questions);
			tv_timer = FindViewById<TextView> (Resource.Id.tv_timer);
			//receive intent
			lesson_title = Intent.GetStringExtra (Constants.LESSON_TITLE);
			lesson_des = Intent.GetStringExtra (Constants.LESSON_DES);
			//set total time
			duration = 80 * Constants.screens.Count * 1000;
			//set screenCounts
			screenCount = Constants.screens.Count;
			answer = new Dictionary<int,String> ();
			validateBtns ();
			updateScreen ();
			nextBtn.Click += NextBtn_Click;
			previousBtn.Click += PreviousBtn_Click;
			btn_submit.Click += delegate(object sender, EventArgs e) {
				//submit button is pressed
				submitAnswers ();
			};
			//start count time:
			MyCountDownTimer timer = new MyCountDownTimer (duration, 1000, this);
			timer.Start ();
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_screens;
		}

		private void updateScreen ()
		{
			//Every time a button is clicked the screen is updated
			FindViewById<TextView> (Resource.Id.navigationTxt).Text = (currentScreen + 1) + " / " + screenCount;
			//get screen
			screen = Constants.screens [currentScreen];
			if (screen != null) {
				switch (screen.Type) {
				case "video_text":
					//must finish watching video
					nextBtn.Visibility = ViewStates.Invisible;
					previousBtn.Visibility = ViewStates.Invisible;
					showVideoTextFragment ();
					break;

				case "audio_question_image":
					
					break;

				case "audio_question":
					
					break;

				case "audio_edittext":
					
					break;
				case "text":
					
					break;
				case "audio_image":
					
					break;
				case "audio_text_image":
					
					break;
				case "audio_text_image_edittext":
					
					break;
				case "video":
					
					break;
				case "text_audio":
					showTextAudioFragment ();
					break;
				default:
					break;
				}
			}
		}

		public void validateBtns ()
		{
			previousBtn.Visibility = ViewStates.Visible;
			nextBtn.Visibility = ViewStates.Visible;
			btn_submit.Visibility = ViewStates.Invisible;
			if (currentScreen == 0) {
				previousBtn.Visibility = ViewStates.Invisible;
			} else if (screen != null && currentScreen == screenCount - 1) {
				nextBtn.Visibility = ViewStates.Invisible;
				btn_submit.Visibility = ViewStates.Visible;
			}
		}

		private Boolean updateAnswers ()
		{
			switch (screen.Type) {
			case "video_text":
				var videoText = fragment as VideoTextFragment;
				string text_answer = videoText.et_answer.Text;
				if (string.IsNullOrEmpty (text_answer)) {
					//not answer the question
					DialogFactory.ToastDialog (this, "Empty answer", "Please type your answer", 0);
					return false;
				}
				//record the answer
				recordAnswer (text_answer);
				break;

			case "audio_question_image":

				break;

			case "audio_question":

				break;

			case "audio_edittext":

				break;
			case "text":

				break;
			case "audio_image":

				break;
			case "audio_text_image":

				break;
			case "audio_text_image_edittext":

				break;
			case "video":

				break;
			default:
				break;
			}
			return true;

		}

		public void recordAnswer (String text_answer)
		{
			if (answer.ContainsKey (currentScreen)) {
				//correct
				answer [currentScreen] = text_answer;
			} else {
				//insert
				answer.Add (currentScreen, text_answer);
			}	
		}

		public override void OnBackPressed ()
		{
			//back pressed
			DialogFactory.ToastDialog (this, "Return", "Please answer all questions and submit!", 0);
		}

		/// <summary>
		/// Submits the answers.
		/// </summary>
		private void submitAnswers ()
		{
			//toast a dialog
			DialogFactory.toastNegativePositiveDialog (this, "Submit", "Are you sure to submit the answers", Constants.SUBMIT_ANSWERS);
		}

		private void PreviousBtn_Click (object sender, System.EventArgs e)
		{
			if (updateAnswers ()) {
				if (currentScreen > 0)
					currentScreen--;
				validateBtns ();
				updateScreen ();
				handler.RemoveCallbacksAndMessages (null);
			}
		}

		private void NextBtn_Click (object sender, System.EventArgs e)
		{
			if (updateAnswers ()) { 
				if (currentScreen < screenCount - 1)
					currentScreen++;
				validateBtns ();
				updateScreen ();
				handler.RemoveCallbacksAndMessages (null);
			}
		}

		//Fragments
		/// <summary>
		/// Shows the video text fragment.
		/// </summary>
		private void showVideoTextFragment ()
		{
			var videoText = fragment as VideoTextFragment;
			if (videoText == null) {
				// Make new fragment to show this selection.
				videoText = new VideoTextFragment (screen);
				// Execute a transaction, replacing any existing
				// fragment with this one inside the frame.
				var ft = FragmentManager.BeginTransaction ();
				ft.Replace (Resource.Id.fragmentContainer, videoText);
				ft.SetTransition (FragmentTransit.FragmentFade);
				ft.Commit ();
				fragment = videoText;
			}
		}

		private void showTextAudioFragment ()
		{
			var audioText = fragment as TextAudioFragment;
			if (audioText == null) {
				// Make new fragment to show this selection.
				audioText = new TextAudioFragment (screen);
				// Execute a transaction, replacing any existing
				// fragment with this one inside the frame.
				var ft = FragmentManager.BeginTransaction ();
				ft.Replace (Resource.Id.fragmentContainer, audioText);
				ft.SetTransition (FragmentTransit.FragmentFade);
				ft.Commit ();
				fragment = audioText;
			}
		}


	}

	public class MyCountDownTimer:CountDownTimer
	{
		private Context context;

		public MyCountDownTimer (long millisInFuture, long countDownInterval, Context context) : base (millisInFuture, countDownInterval)
		{
			this.context = context;
		}

		public override void OnFinish ()
		{
			DialogFactory.ToastDialog (context, "Time up", "Time is out,it will go to result screen!", Constants.TIME_UP);
		}

		public override void OnTick (long millisUntilFinished)
		{
			if (millisUntilFinished <= 60000) {
				(context as ScreensActivity).tv_timer.SetTextColor (Color.Red);
			}
			(context as ScreensActivity).tv_timer.Text = Utils.formatMillis (millisUntilFinished) + "";
		}
	}
}


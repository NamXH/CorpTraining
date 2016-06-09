
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
using Android.Views.InputMethods;
using Android.Hardware.Input;

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
		private int lesson_id;
		private int screenCount = 0;
		public Dictionary<int,string> answer;
		//record all answers
		private InputMethodManager manager;
		private List<string> ids;
		public ISharedPreferences preference;
		public ISharedPreferencesEditor editor;
		private Dictionary<int,int> selectedAnswer;
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
			lesson_id = Intent.GetIntExtra (Constants.LESSON_ID, 0);
			ids = new List<string> ();
			selectedAnswer = new Dictionary<int, int> ();
			preference = GetSharedPreferences (Constants.PREFERENCE_CONFIG, FileCreationMode.Private);
			editor = preference.Edit ();
			//set total time
			duration = 80 * Constants.screens.Count * 1000;
			//set screenCounts
			screenCount = Constants.screens.Count;
			answer = new Dictionary<int,String> ();
			Constants.screenAnswers = new List<ScreenAnswer> ();
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
				nextBtn.Visibility = ViewStates.Invisible;
				previousBtn.Visibility = ViewStates.Invisible;
				if (screen.Type == null) {
					showNullFragment ();
				} else {
					switch (screen.Type) {
					case "video":
					case "video_text":
					//must finish watching video
						ids.Add (screen.Id + "");
						showVideoTextFragment ();
						break;
					case "question_audio":
					case "audio_question":
						showQuestionAudioFragment ();
						break;
					case "text":
						showTextFragment ();
						break;
					case "text_audio_image":
					case "text_image_audio":
						showTextImageAudioFragment ();
						break;
					case "text_question_audio_image":
						showTextQuestionAudioImage ();
						break;
					case "text_audio":
					case "audio_text":
						showTextAudioFragment ();
						break;
					case "question":
						showQuestionFragment ();
						break;
					case "audio_text_video":
						ids.Add (screen.Id + "");
						showAudioTextVideoFragment ();
						break;
					case "text_essay_audio_image":
						showTextEssayAudioImage ();
						break;
					default:
						showNullFragment ();
						break;
					}
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
			case "text_essay_audio_image":
				var textessayaudioimage = fragment as TextEssayAudioImageFragment;
				var eText = textessayaudioimage.et_answer.Text;
				if (string.IsNullOrEmpty (eText)) {
					DialogFactory.ToastDialog (this, "Empty answer", "Please type your answer", 0);
					return false;
				}
				recordAnswer (eText);
				break;
			case "question"://selected
				var question = fragment as QuestionFragment;
				var checkedId = (question.choicesRadioGroup as RadioGroup).CheckedRadioButtonId;
				if (checkedId < 0) {
					//not selected
					DialogFactory.ToastDialog (this, "No selection", "Please select one answer", 0);
					return false;
				}
				recordAnswer (checkedId + "");
				recordSelectedAnswer (screen.Id, question.options [checkedId].Id);
				break;
			case "audio_question":
			case "question_audio"://selected
				var audioquestion = fragment as QuestionAudioFragment;
				var cId = (audioquestion.choicesRadioGroup as RadioGroup).CheckedRadioButtonId;
				if (cId < 0) {
					//not selected
					DialogFactory.ToastDialog (this, "No selection", "Please select one answer", 0);
					return false;
				}
				recordAnswer (cId + "");
				recordSelectedAnswer (screen.Id, audioquestion.options [cId].Id);
				break;
			case "text_question_audio_image":
				var textquestionaudioimage = fragment as TextQuestionAudioImageFragment;
				var tId = (textquestionaudioimage.choicesRadioGroup as RadioGroup).CheckedRadioButtonId;
				if (tId < 0) {
					//not selected
					DialogFactory.ToastDialog (this, "No selection", "Please select one answer", 0);
					return false;
				}
				recordAnswer (tId + "");
				recordSelectedAnswer (screen.Id, textquestionaudioimage.options [tId].Id);
				break;
			default:
				break;
			}
			return true;

		}

		public void recordAnswer (String text_answer)
		{
			if (answer.ContainsKey (screen.Id)) {
				//correct
				answer [screen.Id] = text_answer;
			} else {
				//insert
				answer.Add (screen.Id, text_answer);
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
				hideInput ();
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
				hideInput ();
				handler.RemoveCallbacksAndMessages (null);
			}
		}

		private void hideInput ()
		{
			if (manager == null) {
				manager = (InputMethodManager)GetSystemService (Context.InputMethodService);
			}
			if (manager.IsActive && CurrentFocus != null) {
				if (CurrentFocus.WindowToken != null) {
					manager.HideSoftInputFromWindow (CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
				}             
			}
		}

		//Fragments
		/// <summary>
		/// Shows the video text fragment.
		/// </summary>
		private void showVideoTextFragment ()
		{

			// Make new fragment to show this selection.
			var videoText = new VideoTextFragment (screen);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, videoText);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = videoText;
		}

		private void showTextAudioFragment ()
		{
			// Make new fragment to show this selection.
			var audioText = new TextAudioFragment (screen);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, audioText);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = audioText;
		}

		private void showTextImageAudioFragment ()
		{
			// Make new fragment to show this selection.
			var textImageAudio = new TextImageAudioFragment (screen);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, textImageAudio);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = textImageAudio;
		}

		private void showQuestionFragment ()
		{			
			// Make new fragment to show this selection.
			var question = new QuestionFragment (screen, lesson_id);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, question);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = question;
		}

		private void showQuestionAudioFragment ()
		{
			// Make new fragment to show this selection.
			var questionaudio = new QuestionAudioFragment (screen, lesson_id);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, questionaudio);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = questionaudio;
		}

		private void showAudioTextVideoFragment ()
		{
			// Make new fragment to show this selection.
			var audioTextVideo = new AudioVideoTextFragment (screen);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, audioTextVideo);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = audioTextVideo;
		}

		private void showTextQuestionAudioImage ()
		{
			// Make new fragment to show this selection.
			var textquestionaudioimage = new TextQuestionAudioImageFragment (screen, lesson_id);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, textquestionaudioimage);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = textquestionaudioimage;
		}

		private void showTextFragment ()
		{
			// Make new fragment to show this selection.
			var text = new TextFragment (screen);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, text);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = text;
		}

		private void showTextEssayAudioImage ()
		{
			// Make new fragment to show this selection.
			var text = new TextEssayAudioImageFragment (screen);
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, text);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = text;
		}

		private void showNullFragment ()
		{
			// Make new fragment to show this selection.
			var text = new NullFragment ();
			// Execute a transaction, replacing any existing
			// fragment with this one inside the frame.
			var ft = FragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.fragmentContainer, text);
			ft.SetTransition (FragmentTransit.FragmentFade);
			ft.Commit ();
			fragment = text;
		}

		/// <summary>
		/// Records the selected answer.
		/// </summary>
		/// <param name="screen_id">Screen identifier.</param>
		/// <param name="option_id">Option identifier.</param>
		private void recordSelectedAnswer (int screen_id, int option_id)
		{
			if (selectedAnswer.ContainsKey (screen_id)) {
				//correct
				selectedAnswer [screen_id] = option_id;
			} else {
				//insert
				selectedAnswer.Add (screen_id, option_id);
			}	
		}


		public void submit ()
		{
			//make iswatched false
			foreach (var item in ids) {
				editor.PutBoolean (item, false);
			}
			editor.Commit ();
			//pack screenanswer from selectedanswers
			foreach (var item in selectedAnswer) {
				ScreenAnswer sa = new ScreenAnswer ();
				sa.ScreenId = item.Key;
				sa.OptionId = item.Value;
				sa.UserId = (int)Constants.currentUser.Id;
				Constants.screenAnswers.Add (sa);
			}
			//jump to result activity:
			Intent intent = new Intent (this, typeof(ResultActivity));
			intent.PutExtra (Constants.LESSON_ID, lesson_id);
			StartActivity (intent);
			Finish ();
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



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
using Android.Views.Animations;
using System.Threading.Tasks;
using Android.Graphics;

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class ResultActivity : Activity
	{
		private ImageView iv_scan;
		private TextView tv_initiate;
		private ProgressBar pb_anti;
		private ListView lv_score;
		public Dictionary<int,int> correctAnswers;
		private List<Option> options;
		public List<string> standardAnswer;
		public List<bool> right;
		private Button btn_back;
		private ResultListViewAdapter adapter;
		private int correct = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_result);
			initView ();
			initData ();
			initListener ();
		}

		private void initView ()
		{
			iv_scan = FindViewById<ImageView> (Resource.Id.iv_scan);
			tv_initiate = FindViewById<TextView> (Resource.Id.tv_initiate);
			pb_anti = FindViewById<ProgressBar> (Resource.Id.pb_anti);
			btn_back = FindViewById<Button> (Resource.Id.btn_back);
			lv_score = FindViewById<ListView> (Resource.Id.lv_score);
			//set animation
			var rotate = new RotateAnimation (0f, 360f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf,
				             0.5f);
			//set animation time
			rotate.Duration = 4000;
			//set infinite
			rotate.RepeatCount = Animation.Infinite;
			//start
			iv_scan.StartAnimation (rotate);
		}

		private void initData ()
		{
			correctAnswers = new Dictionary<int, int> ();
			standardAnswer = new List<string> ();
			right = new List<bool> ();
			getCorrectAnswerFromServer ();
		}

		private void initListener ()
		{
			btn_back.Click += delegate(object sender, EventArgs e) {
				Intent intent = new Intent (this, typeof(HomeActivity));
				intent.PutExtra ("email", Constants.currentUser.Email);
				intent.PutExtra ("token", UserUtil.GetCurrentToken ());
				StartActivity (intent);	
				Finish ();
			};

		}

		private async void getCorrectAnswerFromServer ()
		{
			//get correct answer
			foreach (var screen in Constants.screens) {
				string type = screen.Type;
				if (type != null) {
					if (type.Equals ("question") || type.Equals ("audio_question") || type.Equals ("question_audio")) {
						IList<Option> list = await LessonUtil.GetOptionsByScreenAsync (Intent.GetIntExtra (Constants.LESSON_ID, 0), screen.Id + "");
						options = new List<Option> (list);
						foreach (var option in options) {
							if (option.Detail == true) {
								correctAnswers.Add (screen.Id, option.Id);
								switch (option.Order) {
								case 0:
									standardAnswer.Add ("A");
									break;
								case 1:
									standardAnswer.Add ("B");
									break;
								case 2:
									standardAnswer.Add ("C");
									break;
								case 3:
									standardAnswer.Add ("D");
									break;
								case 4:
									standardAnswer.Add ("E");
									break;
								case 5:
									standardAnswer.Add ("F");
									break;
								case 6:
									standardAnswer.Add ("G");
									break;
								default:
									standardAnswer.Add ("H");
									break;
								}
							}
						}
					}
				}
			}
			//send answer to server
			await LessonUtil.SendLessonAnswers (Intent.GetIntExtra (Constants.LESSON_ID, 0), Constants.screenAnswers);
			//judge how many right
			foreach (var answer in Constants.screenAnswers) {
				if (correctAnswers.ContainsKey (answer.ScreenId)) {
					if (correctAnswers [answer.ScreenId] == answer.OptionId) {
						//correct
						correct++;
						right.Add (true);
					} else {
						right.Add (false);
					}
				}
			}
			//complement
			for (int i = right.Count; i < standardAnswer.Count; i++) {
				right.Add (false);
			}
			//set adapter
			adapter = new ResultListViewAdapter (this);
			lv_score.Adapter = adapter;
			pb_anti.Max = standardAnswer.Count;
			pb_anti.Progress = correct;
			int score = (int)((correct / (float)standardAnswer.Count) * 100);
			if (score <= 50) {
				tv_initiate.SetTextColor (Color.Red);
			} else {
				tv_initiate.SetTextColor (Color.Green);
			}
			tv_initiate.Text = "Score:" + score;
		}
	}
}


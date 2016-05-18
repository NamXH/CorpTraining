using System;
using Android.App;
using Android.Content;

namespace CorpTraining.Droid
{
	public class DialogFactory
	{

		public static void ToastDialog (Context context, String title, String msg, int flag)
		{
			AlertDialog.Builder ab = new AlertDialog.Builder (context);
			ab.SetTitle (title);
			ab.SetMessage (msg);
			ab.SetPositiveButton ("confirm", delegate(object sender, DialogClickEventArgs e) {
				switch (flag) {
				case Constants.REGISTER_SUCCESS:
					//enter into login screen
					RegisterActivity activity = context as RegisterActivity;
					activity.StartActivity (new Intent (activity, typeof(LoginActivity)));
					activity.Finish ();
					break;
				case Constants.RETURN_LIST:
					//enter into homeactivity
					VideoPlayActivity video = context as VideoPlayActivity;
					video.StartActivity (new Intent (video, typeof(HomeActivity)));
					video.Finish ();
					break;
				case Constants.ANSWER_QUESTIONS:
					VideoPlayActivity vactivity = context as VideoPlayActivity;
					//todo
					break;
				default:
					break;
				}
			});
			ab.Create ().Show ();
		}

		public static void toastNegativePositiveDialog (Context context, String title, String msg, int flag)
		{
			AlertDialog.Builder ab = new AlertDialog.Builder (context);
			ab.SetTitle (title);
			ab.SetMessage (msg);
			ab.SetPositiveButton ("Confirm", delegate(object sender, DialogClickEventArgs e) {
				switch (flag) {
				case Constants.EXIT_CHOICE:
					HomeActivity home = context as HomeActivity;
					//delete the token
					UserUtil.LogOutUserByTokenAsync (home.token);
					home.StartActivity (new Intent (home, typeof(LoginActivity)));
					home.Finish ();
					break;
				case Constants.START_NOW:
					//jump to answer questions

					break;
				case Constants.RETURN_LIST_NEG:
					//enter into homeactivity
					VideoPlayActivity video = context as VideoPlayActivity;
					video.StartActivity (new Intent (video, typeof(HomeActivity)));
					video.Finish ();
					break;
				default:
					break;
				}
			});
			ab.SetNegativeButton ("Cancel", delegate(object sender, DialogClickEventArgs e) {

			});
			ab.Create ().Show ();
		}

	}
}



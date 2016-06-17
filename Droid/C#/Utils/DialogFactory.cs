using System;
using Android.App;
using Android.Content;
using Android.Graphics;

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
					ScreensActivity video = context as ScreensActivity;
					video.StartActivity (new Intent (video, typeof(HomeActivity)));
					video.Finish ();
					break;
				case Constants.TIME_UP:
					//jump to result activity
					ScreensActivity screenactivity = context as ScreensActivity;
					screenactivity.submit ();					
					break;
				case Constants.LOGIN_TIMEOUT:
					//jump to login
					var ha = context as HomeActivity;
					ha.StartActivity (new Intent (ha, typeof(LoginActivity)));
					ha.Finish ();
					break;
				case Constants.LESSON_ERROR:
					var policeactivity = context as PolicesActivity;
					policeactivity.OnBackPressed ();
					break;
				case Constants.TEXT_ERROR:
					var textactivity = context as TextActivity;
					textactivity.OnBackPressed ();
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
				case Constants.RETURN_LIST_NEG:
					//enter into homeactivity
					ScreensActivity video = context as ScreensActivity;
					video.StartActivity (new Intent (video, typeof(HomeActivity)));
					video.Finish ();
					break;
				case Constants.SUBMIT_ANSWERS:
					//jump to result activity
					ScreensActivity screenactivity = context as ScreensActivity;
					screenactivity.submit ();
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



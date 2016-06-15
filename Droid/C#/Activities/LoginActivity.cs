
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
using Android.Text;
using System.Text.RegularExpressions;
using Android.Preferences;
using Android.App.Admin;

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class LoginActivity : BaseActivity
	{
		private Button login_btn;
		private Button btn_cancel;
		private Button btn_signup;
		private CheckBox auto_save_password;
		private EditText login_accounts;
		private EditText login_password;
		private string email;
		private string password;
		private ISharedPreferences sp;
		private ISharedPreferencesEditor edit;
		private PopupWindow pop;
		private Tuple<string,string> result;
		private LinearLayout ll_loginpage;

		#region implemented abstract members of BaseActivity

		public override void initListner ()
		{
			login_btn.Click += delegate(object sender, EventArgs e) {
				Login ();	
			};
			btn_cancel.Click += delegate(object sender, EventArgs e) {
				Finish ();
			};
			btn_signup.Click += delegate(object sender, EventArgs e) {
				Register ();	
			};
		}

		public override void initData ()
		{
			auto_save_password.Checked = sp.GetBoolean ("checked", true);
			//remember me?
			if (auto_save_password.Checked) {
				login_accounts.Text = sp.GetString ("email", "");
				login_password.Text = sp.GetString ("password", "");
			}
		}

		public override void initView ()
		{
			login_btn = FindViewById<Button> (Resource.Id.btn_login);
			btn_cancel = FindViewById<Button> (Resource.Id.btn_cancel);
			auto_save_password = FindViewById<CheckBox> (Resource.Id.auto_save_password);
			btn_signup = FindViewById<Button> (Resource.Id.btn_signup);
			login_accounts = FindViewById<EditText> (Resource.Id.login_accounts);
			login_password = FindViewById<EditText> (Resource.Id.login_password);
			ll_loginpage = FindViewById<LinearLayout> (Resource.Id.loginpage);
			sp = PreferenceManager.GetDefaultSharedPreferences (this);
			edit = sp.Edit ();
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_login;
		}

		#endregion

		/// <summary>
		/// Logins the test.
		/// </summary>
		private void loginTest ()
		{
			Intent intent = new Intent (this, typeof(HomeActivity));
			intent.PutExtra ("email", email);
			StartActivity (intent);
			Finish ();
		}

		/// <summary>
		/// Login this instance.
		/// </summary>
		public async void Login ()
		{	
			email = login_accounts.Text;
			password = login_password.Text;
			//loginTest ();
			//check internet
			bool isNetWorking = Utils.isNetworkAvailable (this);
			if (isNetWorking == true) {
				//todo:check empty
				if (string.IsNullOrEmpty (email) || string.IsNullOrEmpty (password)) {
					DialogFactory.ToastDialog (this, "Login", "Username and password cannot be empty!", 0);
				} else {
					//check email
					Match match = Regex.Match (email, "^(\\w)+(\\.\\w+)*@(\\w)+((\\.\\w{2,3}){1,3})$");
					if (match.Success) {
						//Authenticate
						View popView = View.Inflate (this, Resource.Layout.popup_authenticate, null);
						pop = new PopupWindow (popView, 320, 200);
						pop.ShowAtLocation (login_btn, GravityFlags.Center, 0, 20);
						//Modal
						ll_loginpage.Alpha = 0.5f;
						EnableAllViews (false);
						//check email and password to server
						try {
							result = await UserUtil.AuthenticateUserAsync (email, password);
						} catch (Exception ex) {
							ll_loginpage.Alpha = 1.0f;
							EnableAllViews (true);
							pop.Dismiss ();
							AlertDialog.Builder ab = new AlertDialog.Builder (this);
							ab.SetTitle ("Server busy");
							ab.SetMessage ("Server is busy,please try again later!");
							ab.SetPositiveButton ("confirm", delegate(object sender, DialogClickEventArgs e) {
							});
							ab.Create ().Show ();
						}
						if (result != null) {
							if (result.Item2 != null) {
								//login successfully
								//remember me?
								bool check = auto_save_password.Checked;
								if (auto_save_password.Checked) {
									edit.PutString ("email", email);
									edit.PutString ("password", password);
									edit.PutBoolean ("checked", check);
									edit.Commit ();
								} else {
									edit.PutBoolean ("checked", check);
									edit.Commit ();
								}
								Toast.MakeText (this, "Login successfully!", ToastLength.Short).Show ();
								Intent intent = new Intent (this, typeof(HomeActivity));
								intent.PutExtra ("email", email);
								intent.PutExtra ("token", result.Item2);
								StartActivity (intent);
								Finish ();
							} else {
								ll_loginpage.Alpha = 1.0f;
								pop.Dismiss ();
								EnableAllViews (true);
								//failed
								DialogFactory.ToastDialog (this, "Login", "Username or password is not correct!", 0);//to do:specific error msg
							}
						} 
					} else {
						ll_loginpage.Alpha = 1.0f;
						pop.Dismiss ();
						EnableAllViews (true);
						DialogFactory.ToastDialog (this, "Login", "Email format is incorrent!", 0);
					}
				}
			} else {
				DialogFactory.ToastDialog (this, "Connect Error", "There is no internet,please connect the internet!", 0);
			}
		}

		/// <summary>
		/// Register an account
		/// </summary>
		private void Register ()
		{
			StartActivity (new Intent (this, typeof(RegisterActivity)));
			Finish ();
		}

		private void EnableAllViews (bool flag)
		{
			login_btn.Clickable = flag;
			btn_cancel.Clickable = flag;
			btn_signup.Clickable = flag;
			auto_save_password.Clickable = flag;
			login_accounts.Clickable = flag;
			login_password.Clickable = flag;
		}
	}
}


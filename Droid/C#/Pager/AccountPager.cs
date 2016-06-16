using System;
using Android.Views;
using System.Security.Cryptography;
using Android.Widget;
using Android.App.Admin;

namespace CorpTraining.Droid
{
	public class AccountPager:BasePager
	{
		private string token;
		private User user;
		private LinearLayout ll_content;
		private EditText et_firstname;
		private EditText et_lastname;
		private EditText et_email;
		private EditText et_phonenumber;
		private EditText et_password;
		private Button btn_save;
		private LinearLayout ll_load;
		private TextView tv_pull_list_header_title;
		private string firstname = "";
		private string lastname = "";
		private string email = "";
		private string phonenumber = "";
		private string password = "";

		public AccountPager (Android.App.Activity activity) : base (activity)
		{
		}

		public async override void initData ()
		{
			View view = View.Inflate (activity, Resource.Layout.viewpager_accountpager, null);
			fl_content.AddView (view);
			//findViews
			ll_content = view.FindViewById<LinearLayout> (Resource.Id.ll_content);
			et_firstname = view.FindViewById<EditText> (Resource.Id.et_firstname);
			et_lastname = view.FindViewById<EditText> (Resource.Id.et_lastname);
			et_email = view.FindViewById<EditText> (Resource.Id.et_email);
			et_phonenumber = view.FindViewById<EditText> (Resource.Id.et_phonenumber);
			et_password = view.FindViewById<EditText> (Resource.Id.et_password);
			btn_save = view.FindViewById<Button> (Resource.Id.btn_save);
			ll_load = view.FindViewById<LinearLayout> (Resource.Id.ll_load);
			tv_pull_list_header_title = view.FindViewById<TextView> (Resource.Id.tv_pull_list_header_title);
			ll_content.Visibility = ViewStates.Invisible;
			ll_load.Visibility = ViewStates.Visible;
			/*Tuple<bool,User> result = null;
			try {
				result = await UserUtil.GetUserProfileByTokenAsync (token);
			} catch (Exception ex) {
				tv_pull_list_header_title.Text = "Data Error!";
			}*/
			if (Constants.currentUser != null) {
				user = Constants.currentUser;
				firstname = user.FirstName;
				lastname = user.LastName;
				email = user.Email;
				phonenumber = user.Phone;
				password = user.Password;
				et_firstname.Text = firstname;
				et_lastname.Text = lastname;
				et_email.Text = email;
				et_phonenumber.Text = phonenumber;
				et_password.Text = password;
				ll_content.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
			} else {
				tv_pull_list_header_title.Text = "There is no user profile!";
			}
			btn_save.Click += delegate(object sender, EventArgs e) {
				//todo: correct the profile	
			};
		}
	}
}


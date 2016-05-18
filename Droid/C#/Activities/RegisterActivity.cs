
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
using System.Text.RegularExpressions;
using Android.Graphics;

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class RegisterActivity : BaseActivity
	{
		private Button btn_regback;
		private Button btn_continue;
		private EditText et_firstname;
		private EditText et_lastname;
		private EditText et_email;
		private EditText et_phonenumber;
		private EditText et_password;
		private EditText et_confirmpwd;
		private TextView tv_firstname;
		private TextView tv_lastname;
		private TextView tv_email;
		private TextView tv_phonenumber;
		private TextView tv_password;
		private TextView tv_confirmpwd;

		private User user;

		public override void initListner ()
		{
			//back button
			btn_regback.Click += delegate(object sender, EventArgs e) {
				StartActivity (new Intent (this, typeof(LoginActivity)));
				Finish ();
			};
			//continue button
			btn_continue.Click += delegate(object sender, EventArgs e) {
				//reset color to black
				clearColor ();
				//get edittext
				string firstname = et_firstname.Text;
				string lastname = et_lastname.Text;
				string email = et_email.Text;
				string phonenumber = et_phonenumber.Text;
				string password = et_password.Text;
				string confirmpwd = et_confirmpwd.Text;
				//Empty check
				if (string.IsNullOrEmpty (firstname)) {
					tv_firstname.SetTextColor (Color.Red);
					DialogFactory.ToastDialog (this, "Empty info", "Firstname cannot be empty!", 0);
				} else if (string.IsNullOrEmpty (lastname)) {
					tv_lastname.SetTextColor (Color.Red);
					DialogFactory.ToastDialog (this, "Empty info", "Lastname cannot be empty!", 0);
				} else if (string.IsNullOrEmpty (email)) {
					tv_email.SetTextColor (Color.Red);
					DialogFactory.ToastDialog (this, "Empty info", "Email cannot be empty!", 0);
				} else if (string.IsNullOrEmpty (phonenumber)) {
					tv_phonenumber.SetTextColor (Color.Red);
					DialogFactory.ToastDialog (this, "Empty info", "Phone number cannot be empty!", 0);
				} else if (string.IsNullOrEmpty (password)) {
					tv_password.SetTextColor (Color.Red);
					DialogFactory.ToastDialog (this, "Empty info", "Password cannot be empty!", 0);
				} else if (string.IsNullOrEmpty (confirmpwd)) {
					tv_email.SetTextColor (Color.Red);
					DialogFactory.ToastDialog (this, "Empty info", "Confirm password cannot be empty!", 0);
				} else {
					//email check
					Match match = Regex.Match (email, "^(\\w)+(\\.\\w+)*@(\\w)+((\\.\\w{2,3}){1,3})$");
					if (match.Success) {
						//check password and confirmpassword
						if (password.Equals (confirmpwd)) {
							//Create user and set info
							user = new User ();
							user.FirstName = firstname;
							user.LastName = lastname;
							user.Password = password;
							user.Phone = phonenumber;
							user.Email = email;
							//connect to server
							registerUser ();
						} else {
							DialogFactory.ToastDialog (this, "Password", "Password cannot match confirm password!", 0);
							tv_password.SetTextColor (Color.Red);
							tv_confirmpwd.SetTextColor (Color.Red);
						}
					} else {
						//email format incorrect
						DialogFactory.ToastDialog (this, "Email Format", "Email format is incorrent!", 0);
						tv_email.SetTextColor (Color.Red);
					}
				}

			};
		}

		private void clearColor ()
		{
			tv_firstname.SetTextColor (Color.Black);
			tv_lastname.SetTextColor (Color.Black);
			tv_email.SetTextColor (Color.Black);
			tv_phonenumber.SetTextColor (Color.Black);
			tv_password.SetTextColor (Color.Black);
			tv_confirmpwd.SetTextColor (Color.Black);
		}

		private async void registerUser ()
		{
			var result = await UserUtil.RegisterUserAsync (user);
			if (result.Item1 == true) {
				//register successfully
				DialogFactory.ToastDialog (this, "Register success", "Congratulations,you have registered successfully!", Constants.REGISTER_SUCCESS);
			} else {
				//register fail
				DialogFactory.ToastDialog (this, "Register fail", "this account has been registered", 0);//to do: more specific error msg
			}

		}

		public override void initData ()
		{
			
		}

		public override void initView ()
		{
			btn_regback = FindViewById<Button> (Resource.Id.btn_regback);
			btn_continue = FindViewById<Button> (Resource.Id.btn_continue);
			et_firstname = FindViewById<EditText> (Resource.Id.et_firstname);
			et_lastname = FindViewById<EditText> (Resource.Id.et_lastname);
			et_email = FindViewById<EditText> (Resource.Id.et_email);
			et_phonenumber = FindViewById<EditText> (Resource.Id.et_phonenumber);
			et_password = FindViewById<EditText> (Resource.Id.et_password);
			et_confirmpwd = FindViewById<EditText> (Resource.Id.et_confirmpwd);

			tv_firstname = FindViewById<TextView> (Resource.Id.tv_firstname);
			tv_lastname = FindViewById<TextView> (Resource.Id.tv_lastname);
			tv_email = FindViewById<TextView> (Resource.Id.tv_email);
			tv_phonenumber = FindViewById<TextView> (Resource.Id.tv_phonenumber);
			tv_password = FindViewById<TextView> (Resource.Id.tv_password);
			tv_confirmpwd = FindViewById<TextView> (Resource.Id.tv_confirmpwd);

		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_register;
		}

		public override void OnBackPressed ()
		{
			
		}
	}
}


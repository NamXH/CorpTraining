
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
using Java.Util;
using Android.Support.V4.Widget;
using Android.Support.V4.App;

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class HomeActivity : BaseActivity
	{
		private TextView tv_title;
		private ImageButton ib_menu;
		private NoScrollViewPager vp_content;
		private RadioGroup rg_functions;
		private RadioButton rb_lessons;
		private RadioButton rb_account;
		private RadioButton rb_settings;
		public List<BasePager> pagerList;
		public string token;
		public string email;
		//left menu content
		public string[] menus = new string[]{ "Search", "Watched Lessons", "Contact Us" };
		public int[] imgsources = new int[]{ Resource.Mipmap.search, Resource.Mipmap.notepad, Resource.Mipmap.mailback };
		private ListView lv_menu;
		private DrawerLayout drawer;

		public override void initListner ()
		{
			ib_menu.Click += delegate(object sender, EventArgs e) {
				drawer.OpenDrawer (lv_menu);	
			};
			vp_content.PageSelected += delegate(object sender, Android.Support.V4.View.ViewPager.PageSelectedEventArgs e) {
				switch (e.Position) {
				case 0:
					tv_title.Text = "Lessons";
					break;
				case 1:
					tv_title.Text = "Account";
					break;
				case 2:
					tv_title.Text = "Settings";
					break;
				default:
					break;
				}
			};

		}

		public override void initData ()
		{
			//receive 
			token = this.Intent.GetStringExtra ("token");
			email = this.Intent.GetStringExtra ("email");
			//add 3 pagers
			pagerList = new List<BasePager> ();
			pagerList.Add (new LessonsPager (this));
			pagerList.Add (new AccountPager (this));
			pagerList.Add (new SettingsPager (this));
			//set adapter
			vp_content.Adapter = new MyPagerAdapter (this);
			//set left menu adapter
			lv_menu.Adapter = new MenuListViewAdapter (this);
			//set listners
			vp_content.PageSelected += delegate(object sender, Android.Support.V4.View.ViewPager.PageSelectedEventArgs e) {
				pagerList [e.Position].initData ();
			};
			rg_functions.CheckedChange += delegate(object sender, RadioGroup.CheckedChangeEventArgs e) {
				switch (e.CheckedId) {
				case Resource.Id.rb_lessons:
					vp_content.CurrentItem = 0;
					break;
				case Resource.Id.rb_account:
					vp_content.CurrentItem = 1;
					break;
				case Resource.Id.rb_settings:
					vp_content.CurrentItem = 2;
					break;
				default:
					break;
				}
			};
			//set first page
			pagerList [0].initData ();
			tv_title.Text = "Lessons";
		}

		public override void initView ()
		{
			tv_title = FindViewById<TextView> (Resource.Id.tv_title);
			ib_menu = FindViewById<ImageButton> (Resource.Id.ib_menu);
			vp_content = FindViewById<NoScrollViewPager> (Resource.Id.vp_content);
			rg_functions = FindViewById<RadioGroup> (Resource.Id.rg_functions);
			rb_lessons = FindViewById<RadioButton> (Resource.Id.rb_lessons);
			rb_account = FindViewById<RadioButton> (Resource.Id.rb_account);
			rb_settings = FindViewById<RadioButton> (Resource.Id.rb_settings);
			lv_menu = FindViewById<ListView> (Resource.Id.left_menu);
			drawer = FindViewById<DrawerLayout> (Resource.Id.dl_menu);
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_home;
		}

		public override void OnBackPressed ()
		{
			DialogFactory.toastNegativePositiveDialog (this, "Exit", "Are you sure to log out?", Constants.EXIT_CHOICE);
		}
	}
}


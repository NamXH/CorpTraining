
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
using System.Threading.Tasks;
using Android.Util;
using Android.Graphics;
using System.Runtime.CompilerServices;

namespace CorpTraining.Droid
{
	[Activity (Label = "TextActivity")]			
	public class TextActivity : BaseActivity
	{
		private TextView tv_title;
		private LinearLayout ll_content;
		public TextView tv_pull_list_header_title;
		private LinearLayout ll_load;
		private int lesson_id;
		private string lesson_name;
		private List<Screen> screens;
		private Screen screen;

		public override void initListner ()
		{
			
		}

		public async override void initData ()
		{
			lesson_id = Intent.GetIntExtra ("lesson_id", -1);
			lesson_name = Intent.GetStringExtra ("lesson_name");
			tv_title.Text = lesson_name;
			await getScreenFromServer ();
		}

		private async Task getScreenFromServer ()
		{
			try {
				screens = await LessonUtil.GetScreensByLessonAsync (lesson_id);

			} catch (Exception ex) {
				DialogFactory.ToastDialog (this, "Data Error", "Error Connecting to Server,please try again later!", Constants.TEXT_ERROR);
			}
			if (screens != null) {
				//make text and images
				screen = screens [0];
				string type = screen.Type;
				if (type.Contains ("text")) {
					var texts = screen.Texts;
					Utils.makeTextViews (texts, this, ll_content, Color.Black);
				} 
				if (type.Contains ("image")) {
					var imageslist = screen.Images;
					var images = new List<Image> (imageslist);
					Utils.makeTextImages (this, ll_content, images);
				}
				//make return button
				Utils.makeReturnButton (this, ll_content);
				ll_content.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
			}
		}

		public override void initView ()
		{
			tv_title = FindViewById<TextView> (Resource.Id.tv_title);
			ll_content = FindViewById<LinearLayout> (Resource.Id.ll_content);
			tv_pull_list_header_title = FindViewById<TextView> (Resource.Id.tv_pull_list_header_title);
			ll_load = FindViewById<LinearLayout> (Resource.Id.ll_load);
			ll_content.Visibility = ViewStates.Invisible;
			ll_load.Visibility = ViewStates.Visible;
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_text;
		}
	}
}


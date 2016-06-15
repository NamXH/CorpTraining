
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

namespace CorpTraining.Droid
{
	[Activity (Label = "CorpTraining", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]				
	public class PracticesActivity : BaseActivity
	{
		private RefreshListView lv_list;
		private LinearLayout ll_load;
		private List<Lesson> lessonList;
		private int lessonId;
		private Lesson currentLesson;
		private int position = 0;
		private int module_id;
		private TextView tv_pull_list_header_title;

		public override void initListner ()
		{
			lv_list.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				lv_list.Visibility = ViewStates.Invisible;
				ll_load.Visibility = ViewStates.Visible;
				position = e.Position - 1;
				lessonId = lessonList [position].Id;
				currentLesson = lessonList [position];
				getVideoUrlFromServer ();
			};
		}

		public override void initData ()
		{
			currentLesson = new Lesson ();
			//load data
			//receive
			module_id = Intent.GetIntExtra (Constants.MODULE_ID, -1);
			getLessonsFromServer ();
		}

		public override void initView ()
		{
			lv_list = FindViewById<RefreshListView> (Resource.Id.lv_list);
			ll_load = FindViewById<LinearLayout> (Resource.Id.ll_load);
			tv_pull_list_header_title = FindViewById<TextView> (Resource.Id.tv_pull_list_header_title);
			lv_list.Visibility = ViewStates.Invisible;
			ll_load.Visibility = ViewStates.Visible;
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_practices;
		}

		private async void getVideoUrlFromServer ()
		{
			var result = await LessonUtil.GetScreensByLessonAsync (lessonId);
			if (result != null && result.Count > 0) {
				Constants.screens = result;
				//jump to screenactivity
				Intent intent = new Intent (this, typeof(ScreensActivity));
				intent.PutExtra (Constants.LESSON_TITLE, currentLesson.Title);
				intent.PutExtra (Constants.LESSON_DES, currentLesson.Description);
				intent.PutExtra (Constants.LESSON_ID, currentLesson.Id);
				StartActivity (intent);
			} else {
				lv_list.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
				DialogFactory.ToastDialog (this, "Empty lesson", "This lesson has not uploaded yet!Please try again later", 0);
			}
		}

		public async void getLessonsFromServer ()
		{
			try {
				var lessons = await LessonUtil.GetLessonsByModuleAsync (module_id);
				lessonList = new List<Lesson> (lessons);
			} catch (Exception ex) {
				DialogFactory.ToastDialog (this, "Server busy", "Server is busy,please drag down to refresh later", 0);
				tv_pull_list_header_title.Text = "Server busy!";
				tv_pull_list_header_title.SetTextColor (Color.Red);
			}
			if (lessonList != null) {
				lv_list.onRefreshComplete ();
				lv_list.Adapter = new MyListViewAdapter (this, lessonList);
				lv_list.setOnRefreshListener (new MyRefreshListener (this));
				lv_list.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
			}
		}

		public override void OnBackPressed ()
		{
			StartActivity (new Intent (this, typeof(HomeActivity)));
			Finish ();
		}
	}

	class MyListViewAdapter: BaseAdapter
	{
		private List<Lesson> list;
		private Context context;

		public MyListViewAdapter (Context context, List<Lesson> list)
		{
			this.list = list;
			this.context = context;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return new JavaObjectWrapper<Lesson> (){ Obj = list [position] };
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ViewHolder holder = new ViewHolder ();
			if (convertView == null) {
				View view = View.Inflate (context, Resource.Layout.item_videolist, null);
				holder.tv_title = view.FindViewById<TextView> (Resource.Id.tv_title);
				holder.tv_size = view.FindViewById<TextView> (Resource.Id.tv_size);
				holder.tv_description = view.FindViewById<TextView> (Resource.Id.tv_description);
				convertView = view;
				convertView.Tag = holder;
			} else {
				holder = (ViewHolder)convertView.Tag;
			}
			//set values
			holder.tv_title.Text = list [position].Title;
			holder.tv_description.Text = list [position].Description;
			holder.tv_size.Text = list [position].ScreenCount + " pages in total";
			return convertView;
		}

		public override int Count {
			get {
				return list.Count;
			}
		}
	}

	class ViewHolder:Java.Lang.Object
	{
		public TextView tv_title;
		public TextView tv_size;
		public TextView tv_description;
	}

	class MyRefreshListener:onRefreshListener
	{

		private PracticesActivity pager;

		public MyRefreshListener (PracticesActivity pager)
		{
			this.pager = pager;
		}

		public void onRefresh ()
		{
			pager.getLessonsFromServer ();
		}

		public void onLoadMore ()
		{
			//todo:load more lessons
		}
	}
}


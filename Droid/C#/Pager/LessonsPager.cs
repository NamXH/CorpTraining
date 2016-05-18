using System;
using Android.Widget;
using Android.Views;
using System.Security.Cryptography;
using System.Collections.Generic;
using Android.Content;
using SQLitePCL;

namespace CorpTraining.Droid
{
	public class LessonsPager: BasePager
	{
		private RefreshListView lv_list;
		private LinearLayout ll_load;
		private List<Lesson> lessonList;
		private int lessonId;
		private Lesson currentLesson;
		private int position = 0;

		public LessonsPager (Android.App.Activity activity) : base (activity)
		{
		}

		public async override void initData ()
		{
			View view = View.Inflate (activity, Resource.Layout.viewpager_lessonspager, null);
			fl_content.RemoveAllViews ();
			fl_content.AddView (view);
			lv_list = view.FindViewById<RefreshListView> (Resource.Id.lv_list);
			ll_load = view.FindViewById<LinearLayout> (Resource.Id.ll_load);
			lv_list.Visibility = ViewStates.Invisible;
			ll_load.Visibility = ViewStates.Visible;
			currentLesson = new Lesson ();
			//load data
			getLessonsFromServer ();
			lv_list.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				lv_list.Visibility = ViewStates.Invisible;
				ll_load.Visibility = ViewStates.Visible;
				position = e.Position - 1;
				lessonId = lessonList [position].Id;
				currentLesson = lessonList [position];
				getVideoUrlFromServer ();
			};
		}

		private async void getVideoUrlFromServer ()
		{
			var result = await LessonUtil.GetScreenByPositionAsync (lessonId, 0);
			string url = result.VideoUrl;
			if (!string.IsNullOrEmpty (url)) {
				Intent intent = new Intent (activity, typeof(VideoPlayActivity));
				intent.PutExtra (Constants.VIDEO_URL, url);
				intent.PutExtra (Constants.LESSON_TITLE, currentLesson.Title);
				intent.PutExtra (Constants.LESSON_DES, currentLesson.Description);
				activity.StartActivity (intent);
				activity.Finish ();
			} else {
				lv_list.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
				DialogFactory.ToastDialog (activity, "Connect error", "Connect error,please try again later!", 0);
			}
		}

		public async void getLessonsFromServer ()
		{
			try {
				var lessons = await LessonUtil.GetLessonsAsync ();
				lessonList = new List<Lesson> (lessons);
			} catch (Exception ex) {
				DialogFactory.ToastDialog (activity, "Server busy", "Server is busy,please drag down to refresh later", 0);
			}
			lv_list.onRefreshComplete ();
			lv_list.Adapter = new MyListViewAdapter (activity, lessonList);
			lv_list.setOnRefreshListener (new MyRefreshListener (this));
			lv_list.Visibility = ViewStates.Visible;
			ll_load.Visibility = ViewStates.Invisible;
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
			holder.tv_size.Text = list [position].ScreenCount + " questions in total";
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

		private LessonsPager pager;

		public MyRefreshListener (LessonsPager pager)
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


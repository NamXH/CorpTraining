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
		private ListView lv_list;
		private LinearLayout ll_load;
		private int lessonId;
		private Module currentModule;
		private int position = 0;
		private List<Module> moduleList;

		public LessonsPager (Android.App.Activity activity) : base (activity)
		{
		}

		public async override void initData ()
		{
			View view = View.Inflate (activity, Resource.Layout.viewpager_lessonspager, null);
			fl_content.RemoveAllViews ();
			fl_content.AddView (view);
			lv_list = view.FindViewById<ListView> (Resource.Id.lv_list);
			ll_load = view.FindViewById<LinearLayout> (Resource.Id.ll_load);
			lv_list.Visibility = ViewStates.Invisible;
			ll_load.Visibility = ViewStates.Visible;
			Constants.currentUser = new User ();
			//load module
			getModulesFromServer ();
			lv_list.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				//jump to activity according to Module ID
				currentModule = moduleList [e.Position];
				Intent intent = null;
				switch (currentModule.Id) {
				case 1:
					intent = new Intent (activity, typeof(PolicesActivity));
					break;
				case 2:
					intent = new Intent (activity, typeof(ProceduresActivity));
					break;
				case 3:
					intent = new Intent (activity, typeof(PracticesActivity));
					break;
				default:
					break;
				}
				if (intent != null) {
					Constants.module_id = currentModule.Id;
					activity.StartActivity (intent);
					activity.Finish ();
				}
			};
		}

		public async void getModulesFromServer ()
		{
			try {
				var modules = await LessonUtil.GetModulesAsync ();
				Constants.currentUser = await UserUtil.GetCurrentUserAsync ();
				moduleList = new List<Module> (modules);
			} catch (Exception ex) {
				DialogFactory.ToastDialog (activity, "Server busy", "Server is busy,please login later", Constants.LOGIN_TIMEOUT);
			}
			if (Constants.currentUser == null) {
				//illegal login
				DialogFactory.ToastDialog (activity, "Login timeout", "Timeout,please login again", Constants.LOGIN_TIMEOUT);
			} else {
				lv_list.Adapter = new MyModuleListAdapter (moduleList, this.activity);
				lv_list.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
			}
		}
	}

	public class MyModuleListAdapter:BaseAdapter
	{
		private List<Module> list;
		private Context context;

		public MyModuleListAdapter (List<Module> list, Context context)
		{
			this.list = list;
			this.context = context;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return new JavaObjectWrapper<Module> (){ Obj = list [position] };
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ModuleViewHolder holder = null;
			Module cModule;
			if (convertView == null) {
				convertView = View.Inflate (context, Resource.Layout.item_modulelist, null);
				holder = new ModuleViewHolder ();
				holder.tv_title = convertView.FindViewById<TextView> (Resource.Id.tv_title);
				convertView.Tag = holder;
			} else {
				holder = (ModuleViewHolder)convertView.Tag;
			}
			cModule = list [position];
			holder.tv_title.Text = (cModule.Name == null) ? "No theme Lessons" : cModule.Name;
			return convertView;
		}

		public override int Count {
			get {
				return list.Count;
			}
		}
	}

	class ModuleViewHolder:Java.Lang.Object
	{
		public TextView tv_title;
	}
}


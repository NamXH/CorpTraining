using System;
using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;

namespace CorpTraining.Droid
{
	public class MenuListViewAdapter:BaseAdapter
	{
		private HomeActivity activity;

		public MenuListViewAdapter (HomeActivity activity)
		{
			this.activity = activity;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return activity.menus [position];
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View view = View.Inflate (activity, Resource.Layout.item_leftmenu, null);
			ImageView iv_icon = view.FindViewById<ImageView> (Resource.Id.iv_icon);
			TextView tv_title = view.FindViewById<TextView> (Resource.Id.tv_title);
			iv_icon.SetBackgroundResource (activity.imgsources [position]);
			tv_title.Text = activity.menus [position];
			return view;
		}

		public override int Count {
			get {
				return activity.imgsources.Length;
			}
		}
	}
}


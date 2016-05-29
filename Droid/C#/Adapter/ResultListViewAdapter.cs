using System;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;

namespace CorpTraining.Droid
{
	public class ResultListViewAdapter:BaseAdapter
	{
		private ResultActivity result;

		public ResultListViewAdapter (ResultActivity result)
		{
			this.result = result;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return result.standardAnswer [position];
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View view;
			ResultViewHolder holder;
			if (convertView == null) {
				view = View.Inflate (result, Resource.Layout.item_resultlistview, null);
				holder = new ResultViewHolder ();
				holder.ll_item = view.FindViewById<LinearLayout> (Resource.Id.ll_item);
				holder.tv_order = view.FindViewById<TextView> (Resource.Id.tv_order);
				holder.tv_answer = view.FindViewById<TextView> (Resource.Id.tv_answer);
				holder.tv_right = view.FindViewById<TextView> (Resource.Id.tv_right);
				convertView = view;
				convertView.Tag = holder;
			} else {
				holder = (ResultViewHolder)convertView.Tag;
			}
			holder.tv_order.Text = (position + 1) + "";
			holder.tv_answer.Text = result.standardAnswer [position];
			if (result.right [position]) {
				holder.ll_item.SetBackgroundColor (Color.LightGreen);
				holder.tv_right.Text = "√";
			} else {
				holder.ll_item.SetBackgroundColor (Color.Red);
				holder.tv_right.Text = "×";
			}
			return convertView;
		}

		public override int Count {
			get {
				return result.standardAnswer.Count;
			}
		}
	}

	public class ResultViewHolder: Java.Lang.Object
	{
		public LinearLayout ll_item;
		public TextView tv_order;
		public TextView tv_answer;
		public TextView tv_right;
	}
}


using System;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using Android.Views;

namespace CorpTraining.Droid
{
	public class SortAdapter:BaseAdapter,ISectionIndexer
	{
		private List<SortModel> list = null;
		private Context mContext;

		public SortAdapter (Context mContext, List<SortModel> list)
		{
			this.mContext = mContext;
			this.list = list;
		}

		public void updateListView (List<SortModel> list)
		{
			this.list = list;
			NotifyDataSetChanged ();
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return this.list [position];
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			SortViewHolder viewHolder = null;
			SortModel mContent = list [position];
			if (convertView == null) {
				viewHolder = new SortViewHolder ();
				convertView = LayoutInflater.From (mContext).Inflate (Resource.Layout.item_users, null);
				viewHolder.tvTitle = convertView.FindViewById<TextView> (Resource.Id.title);
				viewHolder.tvLetter = convertView.FindViewById<TextView> (Resource.Id.catalog);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = (SortViewHolder)convertView.Tag;
			}
			//根据position获取分类的首字母的Char ascii值
			int section = GetSectionForPosition (position);
			//如果当前位置等于该分类首字母的Char的位置 ，则认为是第一次出现
			if (position == GetPositionForSection (section)) {
				viewHolder.tvLetter.Visibility = ViewStates.Visible;
				viewHolder.tvLetter.Text = mContent.SortLetters;
			} else {
				viewHolder.tvLetter.Visibility = ViewStates.Gone;
			}
			viewHolder.tvTitle.Text = (this.list [position].Name);
			return convertView;
		}

		public override int Count {
			get {
				return this.list.Count;
			}
		}

		/**
	 * 根据分类的首字母的Char ascii值获取其第一次出现该首字母的位置
	 */
		public int GetPositionForSection (int sectionIndex)
		{
			for (int i = 0; i < Count; i++) {
				String sortStr = list [i].SortLetters;
				char firstChar = sortStr.ToUpper () [0];
				if (firstChar == sectionIndex) {
					return i;
				}
			}
			return -1;
		}


		public int GetSectionForPosition (int position)
		{
			return list [position].SortLetters [0];
		}

		public Java.Lang.Object[] GetSections ()
		{
			return null;
		}

	}

	class SortViewHolder:Java.Lang.Object
	{
		public TextView tvLetter;
		public TextView tvTitle;
	}
}


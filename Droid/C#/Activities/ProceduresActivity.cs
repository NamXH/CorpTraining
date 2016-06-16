
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
using Android.Text;

namespace CorpTraining.Droid
{
	[Activity (Label = "ProceduresActivity")]			
	public class ProceduresActivity : BaseActivity
	{
		public ListView sortListView;
		public SortAdapter adapter;
		private ClearEditText mClearEditText;
		private FrameLayout fl_content;
		private LinearLayout ll_load;
		public List<SortModel> sourceDateList;
		public TextView tv_pull_list_header_title;
		private int module_id;

		public override void initListner ()
		{
			//set edittext listener
			mClearEditText.AddTextChangedListener (new MyProcedureTextChangedListener (this));
			//set item click listener
			sortListView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				SortModel user = sourceDateList [e.Position];
				Intent intent = new Intent (this, typeof(TextActivity));
				intent.PutExtra ("lesson_id", user.LessonId);
				intent.PutExtra ("lesson_name", user.Name);
				StartActivity (intent);
			};
		}

		public async override void initData ()
		{
			//receive
			module_id = Constants.module_id;
			//get data from server
			await getDataFromServer ();
			//set adapter
			adapter = new SortAdapter (this, sourceDateList);
			sortListView.Adapter = adapter;
			//show content
			ll_load.Visibility = ViewStates.Invisible;
			fl_content.Visibility = ViewStates.Visible;
		}

		public override void initView ()
		{
			//find views
			sortListView = FindViewById<ListView> (Resource.Id.country_lvcountry);
			mClearEditText = FindViewById<ClearEditText> (Resource.Id.filter_edit);
			fl_content = FindViewById<FrameLayout> (Resource.Id.fl_content);
			ll_load = FindViewById<LinearLayout> (Resource.Id.ll_load);
			tv_pull_list_header_title = FindViewById<TextView> (Resource.Id.tv_pull_list_header_title);
			ll_load.Visibility = ViewStates.Visible;
			fl_content.Visibility = ViewStates.Invisible;
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_polices;
		}

		public async Task getDataFromServer ()
		{
			IList<Lesson> lessons = null;
			try {
				lessons = await LessonUtil.GetLessonsByModuleAsync (module_id);
			} catch (Exception ex) {
				DialogFactory.ToastDialog (this, "Data Error", "Error Connecting to Server,please try again later!", Constants.LESSON_ERROR);
			}
			if (lessons != null) {
				sourceDateList = new List<SortModel> ();
				foreach (var lesson in lessons) {
					string name = lesson.Title;
					if (name.Length > 0) {
						string firstAlpha = name.Substring (0, 1).ToUpper ();
						SortModel temp = new SortModel ();
						temp.SortLetters = firstAlpha;
						temp.Name = name;
						temp.LessonId = lesson.Id;
						sourceDateList.Add (temp);
					} else {
						continue;
					}
				}
				sourceDateList.Sort ();
			}
		}

		public override void OnBackPressed ()
		{
			goBack ();
		}

		private void goBack ()
		{
			StartActivity (new Intent (this, typeof(HomeActivity)));
			Finish ();
		}
	}

	public class MyProcedureTextChangedListener:Java.Lang.Object,ITextWatcher
	{

		private ProceduresActivity mUser;

		public MyProcedureTextChangedListener (ProceduresActivity mUser)
		{
			this.mUser = mUser;
		}

		public void AfterTextChanged (IEditable s)
		{

		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
		{

		}

		public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count)
		{
			filterData (s.ToString ());
		}


		public void filterData (string str)
		{
			List<SortModel> filterList = new List<SortModel> ();
			if (string.IsNullOrEmpty (str)) {
				filterList = mUser.sourceDateList;
			} else {
				filterList.Clear ();
				foreach (var model in mUser.sourceDateList) {
					string name = model.Name.ToUpper ();
					if (name.IndexOf (str.ToUpper ()) != -1) {
						filterList.Add (model);
					}
				}
			}
			//sort
			filterList.Sort ();
			mUser.adapter.updateListView (filterList);
		}
	}
}


using System;
using Android.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.App;
using Java.Text;

namespace CorpTraining.Droid
{
	public class RefreshListView:ListView,Android.Widget.AbsListView.IOnScrollListener,Android.Widget.AdapterView.IOnItemClickListener
	{
		private View myHeaderView;
		private TextView tv_title;
		private TextView tv_time;
		private ImageView iv_arrow;
		private ProgressBar pb_progress;
		private float startY;
		private float endY;
		private int measuredHeight;
		private int gap;
		private const int PULL_REFRESH = 0;
		//Pull refresh
		private const int RELEASE_REFRESH = 1;
		//Release refresh
		private const int REFRESHING = 2;
		//refreshing...
		private int currentState = PULL_REFRESH;
		private RotateAnimation animUp;
		private RotateAnimation animDown;
		private View mfootView;
		private int mfootViewMeasuredHeight;
		private onRefreshListener listner;
		private bool isLoadingmore;
		private IOnItemClickListener mItemClickListener;

		public RefreshListView (Android.Content.Context context) : base (context)
		{
			initHeaderView ();
			//initFooterView ();
		}


		public RefreshListView (Android.Content.Context context, Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
			initHeaderView ();
			//initFooterView ();
		}


		public RefreshListView (Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr)
		{
			initHeaderView ();
			//initFooterView ();
		}

		private void initHeaderView ()
		{
			myHeaderView = View.Inflate (Context, Resource.Layout.refresh_header, null);
			this.AddHeaderView (myHeaderView);
			tv_title = (TextView)myHeaderView.FindViewById (Resource.Id.tv_refresh);
			tv_time = (TextView)myHeaderView.FindViewById (Resource.Id.tv_time);
			iv_arrow = (ImageView)myHeaderView.FindViewById (Resource.Id.iv_arrow);
			pb_progress = (ProgressBar)myHeaderView.FindViewById (Resource.Id.pb_progress);
			myHeaderView.Measure (0, 0);
			measuredHeight = myHeaderView.MeasuredHeight;
			myHeaderView.SetPadding (0, -measuredHeight, 0, 0);//Hide headerview
			initArrowAnim ();
			refreshState ();
			tv_time.Text = "Last refreshing time：" + getDate ();
		}

		//todo: foot refresh
		private void initFooterView ()
		{
			mfootView = View.Inflate (Context, Resource.Layout.refresh_footer, null);
			this.AddFooterView (mfootView);
			mfootView.Measure (0, 0);
			mfootViewMeasuredHeight = mfootView.MeasuredHeight;
			mfootView.SetPadding (0, -mfootViewMeasuredHeight, 0, 0);//隐藏脚布局
			this.SetOnScrollListener (this);
		}

		private void initArrowAnim ()
		{
			//arrow up
			animUp = new RotateAnimation (0, -180, Dimension.RelativeToSelf, 0.5f,
				Dimension.RelativeToSelf, 0.5f);
			animUp.Duration = 300;
			animUp.FillAfter = true;

			//arrow down
			animDown = new RotateAnimation (-180, 0, Dimension.RelativeToSelf, 0.5f,
				Dimension.RelativeToSelf, 0.5f);
			animDown.Duration = 300;
			animDown.FillAfter = true;
		}

		public void refreshState ()
		{
			switch (currentState) {
			case PULL_REFRESH:
				iv_arrow.Visibility = ViewStates.Visible;
				tv_title.Text = "Drag down to refresh";
				pb_progress.Visibility = ViewStates.Invisible;
				iv_arrow.StartAnimation (animDown);
				break;
			case RELEASE_REFRESH:
				iv_arrow.Visibility = ViewStates.Visible;
				tv_title.Text = "Release to refresh";
				pb_progress.Visibility = ViewStates.Invisible;
				iv_arrow.StartAnimation (animUp);
				break;
			case REFRESHING:
				iv_arrow.ClearAnimation ();
				iv_arrow.Visibility = ViewStates.Invisible;
				tv_title.Text = "Now refreshing...";
				pb_progress.Visibility = ViewStates.Visible;
				if (listner != null)
					listner.onRefresh ();
				break;
			default:
				break;
			}
		}

		public String getDate ()
		{
			return DateTime.Now.ToLocalTime ().ToString ();
		}

		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
			
		}

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			if (scrollState == ScrollState.Idle
			    || scrollState == ScrollState.Fling) {
				if (LastVisiblePosition == Count - 1 && !isLoadingmore) {
					//滑动到最后
					mfootView.SetPadding (0, 0, 0, 0);
					SetSelection (Count - 1);//change listview show position
					isLoadingmore = true;
					if (listner != null) {
						listner.onLoadMore ();
					}
				}
			}
		}

		public void setOnItemClickListener (IOnItemClickListener listener)
		{
			base.OnItemClickListener = listener;
			this.mItemClickListener = listener;
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id)
		{
			if (mItemClickListener != null) {
				mItemClickListener.OnItemClick (parent, view, position - HeaderViewsCount, id);//手动减headerview
			}
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			switch (e.Action) {
			case MotionEventActions.Down:
				startY = e.RawY;
				break;
			case MotionEventActions.Move:
				if (startY == -1) {
					startY = e.RawY;
				}
				if (currentState == REFRESHING)
					break;
				endY = e.RawY;
				int dy = (int)(endY - startY);
				if (dy > 0 && FirstVisiblePosition == 0) {
					gap = dy - measuredHeight;
					myHeaderView.SetPadding (0, gap, 0, 0);//set padding
					if (gap > 0 && currentState == PULL_REFRESH) {
						currentState = RELEASE_REFRESH;
						refreshState ();
					} else if (gap < 0) {
						currentState = PULL_REFRESH;
					}
				}
				break;
			case MotionEventActions.Up:
				startY = -1;
				if (currentState == RELEASE_REFRESH) {
					currentState = REFRESHING;
					myHeaderView.SetPadding (0, 0, 0, 0);
					refreshState ();
				} else if (currentState == PULL_REFRESH) {
					myHeaderView.SetPadding (0, -measuredHeight, 0, 0);
				}
				break;
			default:
				break;
			}
			return base.OnTouchEvent (e);
		}

		public void setOnRefreshListener (onRefreshListener listener)
		{
			this.listner = listener;
		}

		public void onRefreshComplete ()
		{
			if (isLoadingmore) {
				mfootView.SetPadding (0, -mfootViewMeasuredHeight, 0, 0);
				isLoadingmore = false;
			}
			currentState = PULL_REFRESH;
			iv_arrow.Visibility = ViewStates.Visible;
			pb_progress.Visibility = ViewStates.Invisible;
			tv_title.Text = "Drag down to refresh";
			myHeaderView.SetPadding (0, -measuredHeight, 0, 0);
			tv_time.Text = "Last refreshing time：" + getDate ();
		}
		
	}

	public interface onRefreshListener
	{
		void onRefresh ();

		void onLoadMore ();
	}
}


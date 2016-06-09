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
using System.Security.Cryptography;
using Android.Media;
using NineOldAndroids.View;
using IO.Vov.Vitamio;
using Android.Text.Format;
using Javax.Crypto.Interfaces;
using Android.Gestures;
using Android.Support.V4.View;
using Java.Security;
using NineOldAndroids.Animation;
using Android.Content.PM;
using Android.Text;
using Android.Preferences;

namespace CorpTraining.Droid
{
	[Activity (Label = "VideoPlay", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen",
		ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]			
	public abstract class BaseVideoFragment : Fragment
	{
		private TextView tv_title;
		private TextView tv_system_time;
		public TextView tv_current_position;
		private TextView tv_duration;
		private ImageView iv_battery;
		private SeekBar sb_voice;
		private SeekBar sb_video;
		private Button btn_play;
		private Button btn_share;
		private Button btn_voice;
		private Button btn_exit;
		//private GestureDetector gestureDetector;
		//private MyGestureListener mygesture;
		public IO.Vov.Vitamio.Widget.VideoView vv;
		private MyHandler handler;
		//Braightness and volume
		private AudioManager audioManager;
		private int maxVolume;
		public int currentVolume;
		private float maxVlumeScreenHeightScale;
		private LinearLayout ll_loading;
		//full screen->not full
		private bool isFullScreen;
		private int vvHeight;
		private int vvWidth;
		private int screenWidth;
		private int screenHeight;
		//Broadcastreceiver
		private BatteryChangedReceiver batteryChangedReceiver;
		private View view;
		public string video_url;
		public View rootView;
		public Screen screen;
		public ISharedPreferences preference;
		public ISharedPreferencesEditor editor;
		public bool isWatched;
		private TextView tv_loading;

		public BaseVideoFragment (Screen screen)
		{
			this.screen = screen;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			view = inflater.Inflate (getLayoutResource (), container, false);
			rootView = view;
			//read isWatched from preferences
			preference = (Activity as ScreensActivity).preference;
			editor = (Activity as ScreensActivity).editor;
			isWatched = preference.GetBoolean (screen.Id + "", false);
			initView ();
			if (!isWatched) {
				initData ();
			} else {
				ll_loading.Visibility = ViewStates.Visible;
				tv_loading.Text = "You have watched this video!";
				var activity = Activity as ScreensActivity;
				if (!screen.Type.Equals ("audio_text_video")) {
					activity.validateBtns ();
				}
			}
			initListner ();
			init ();
			return view;
		}


		public abstract void init ();

		public abstract int getLayoutResource ();

		public void initListner ()
		{
			sb_voice.ProgressChanged += ((sender, e) => {
				if (e.FromUser) {
					setStreamVolume (e.Progress);
				}
			});
			sb_video.ProgressChanged += delegate(object sender, SeekBar.ProgressChangedEventArgs e) {
				if (e.FromUser) {
					if (vv.IsPlaying) {
						vv.SeekTo (e.Progress);
					} else {
						vv.SeekTo (e.Progress);
						btn_play.SetBackgroundResource (Resource.Drawable.selector_btn_play);
					}
				}
			};
			//video buffer listener
			vv.BufferingUpdate += delegate(object sender, IO.Vov.Vitamio.MediaPlayer.BufferingUpdateEventArgs e) {
				updateSecondaryProgress (e);
			};
			//set buffered slow listener
			vv.Info += delegate(object sender, IO.Vov.Vitamio.MediaPlayer.InfoEventArgs e) {
				switch (e.P1) {
				//buffer started
				case (int)MediaInfo.BufferingStart:
					ll_loading.Visibility = ViewStates.Visible;
					break;
				//buffer end then play
				case (int)MediaInfo.BufferingEnd:
					hideLoadingCtl ();
					break;
				default:
					break;
				}
			};
			if (!screen.Type.Equals ("audio-text-video")) {
				vv.Completion += delegate(object sender, IO.Vov.Vitamio.MediaPlayer.CompletionEventArgs e) {
					vv.SeekTo (0);
					tv_current_position.Text = Utils.formatMillis (0L);
					var activity = Activity as ScreensActivity;
					//set iswatched
					editor.PutBoolean (screen.Id + "", true).Commit ();
					activity.validateBtns ();
				};
			}

			btn_voice.Click += delegate(object sender, EventArgs e) {
				toggleMute ();
			};

			btn_exit.Click += delegate(object sender, EventArgs e) {
				DialogFactory.toastNegativePositiveDialog (Activity, "Return", "Are you sure to return to Lesson List?", Constants.RETURN_LIST_NEG);
			};

			btn_play.Click += delegate(object sender, EventArgs e) {
				play ();
			};

			btn_share.Click += delegate(object sender, EventArgs e) {
				//todo:share function

			};
		}

		/// <summary>
		/// Updates the secondary progress.
		/// </summary>
		/// <param name="e">E.</param>
		private void updateSecondaryProgress (IO.Vov.Vitamio.MediaPlayer.BufferingUpdateEventArgs e)
		{
			float percentFloat = e.P1 / 100.0f;
			int secondaryProgress = (int)(vv.Duration * percentFloat);
			sb_video.SecondaryProgress = secondaryProgress;
		}

		/// <summary>
		/// set volume
		/// </summary>
		/// <param name="index">Index.</param>
		private void setStreamVolume (int index)
		{
			audioManager.SetStreamVolume (Stream.Music, index, VolumeNotificationFlags.PlaySound);
		}

		public void OnBackPressed ()
		{
			DialogFactory.ToastDialog (Activity, "Back", "Please finish watching this lesson!", 0);
		}

		public abstract string getVideoUrl ();

		public async void initData ()
		{
			//register battery change broadcast
			RegisterBatteryChangedReceiver ();
			//check vitamio installed?
			if (!LibsChecker.CheckVitamioLibs (Activity)) {
				return;
			}
			//get video_url
			video_url = Utils.EncodeURL (getVideoUrl ());
			if (!string.IsNullOrEmpty (video_url)) {
				vv.SetVideoURI (Android.Net.Uri.Parse (Utils.EncodeURL (video_url)));
			} else {
				DialogFactory.ToastDialog (Activity, "Connect error", "Connect error,please try again later!", Constants.RETURN_LIST);
			}
			vv.RequestFocus ();
			vv.Prepared += delegate(object sender, IO.Vov.Vitamio.MediaPlayer.PreparedEventArgs e) {
				e.P0.SetPlaybackSpeed (1.0f);//start play
				vv.Start ();
				//put duration
				string duration = Utils.formatMillis (vv.Duration);//set duration
				tv_duration.Text = duration;
				sb_video.Max = (int)vv.Duration;
				updateCurrentPosition ();
				btn_play.SetBackgroundResource (Resource.Drawable.selector_btn_pause);//set play button
				tv_title.Text = Activity.Intent.GetStringExtra (Constants.LESSON_TITLE) + ":" + Activity.Intent.GetStringExtra (Constants.LESSON_DES);//set video title
				//hide the loading progress bar
				hideLoadingCtl ();
			};
			//control volume
			initVolume ();
			// scale volume
			maxVlumeScreenHeightScale = ((float)maxVolume) / Utils.getWindowWidth (Activity);
			//set screen width and height
			screenWidth = Utils.getWindowWidth (Activity);
			screenHeight = Utils.getWindowHeight (Activity);
		}

		/// <summary>
		/// Hides the loading ctl.
		/// </summary>
		private void hideLoadingCtl ()
		{		
			//set end animationlistner	
			NineOldAndroids.View.ViewPropertyAnimator.Animate (ll_loading).Alpha (0.0f).SetDuration (1500).SetListener (new MyAnimatorListener (ll_loading));
		}

		/// <summary>
		/// Inits the volume.
		/// </summary>
		void initVolume ()
		{
			audioManager = (AudioManager)Activity.GetSystemService (Context.AudioService);
			maxVolume = audioManager.GetStreamMaxVolume (Android.Media.Stream.Music);
			currentVolume = GetStreamVolume ();
			sb_voice.Max = maxVolume;
			sb_voice.Progress = currentVolume;
		}

		public int GetStreamVolume ()
		{
			return audioManager.GetStreamVolume (Stream.Music);
		}

		public override void OnStart ()
		{
			base.OnStart ();
			UpdateSystemTime ();
		}

		public override void OnStop ()
		{
			base.OnStop ();
			handler.RemoveMessages (Constants.UPDATE_SYSTEM_TIME);
		}

		/// <summary>
		/// update the system time
		/// </summary>
		void UpdateSystemTime ()
		{
			tv_system_time.Text = DateFormat.Format ("kk:mm:ss", System.DateTime.Now.Millisecond);
			handler.SendEmptyMessageDelayed (Constants.UPDATE_SYSTEM_TIME, 1000);
		}

		/// <summary>
		/// Registers the battery changed receiver.
		/// </summary>
		public void RegisterBatteryChangedReceiver ()
		{
			IntentFilter filter = new IntentFilter (Intent.ActionBatteryChanged);
			batteryChangedReceiver = new BatteryChangedReceiver (iv_battery);
			Activity.RegisterReceiver (batteryChangedReceiver, filter);
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			if (batteryChangedReceiver != null) {
				Activity.UnregisterReceiver (batteryChangedReceiver);
			}
			handler.RemoveCallbacksAndMessages (null);
			vv.StopPlayback ();
		}


		private void initView ()
		{
			vv = view.FindViewById<IO.Vov.Vitamio.Widget.VideoView> (Resource.Id.video_view);
			//TextView control
			tv_title = view.FindViewById<TextView> (Resource.Id.tv_title);
			tv_system_time = view.FindViewById<TextView> (Resource.Id.tv_system_time);
			tv_current_position = view.FindViewById<TextView> (Resource.Id.tv_current_position);
			tv_duration = view.FindViewById<TextView> (Resource.Id.tv_duration);
			iv_battery = view.FindViewById<ImageView> (Resource.Id.iv_battery);
			//seekbar
			sb_voice = view.FindViewById<SeekBar> (Resource.Id.sb_voice);
			sb_video = view.FindViewById<SeekBar> (Resource.Id.sb_video);
			//button
			btn_play = view.FindViewById<Button> (Resource.Id.btn_play);
			btn_share = view.FindViewById<Button> (Resource.Id.btn_share);
			btn_voice = view.FindViewById<Button> (Resource.Id.btn_voice);
			btn_exit = view.FindViewById<Button> (Resource.Id.btn_exit);
			ll_loading = view.FindViewById<LinearLayout> (Resource.Id.ll_loading);
			tv_loading = view.FindViewById<TextView> (Resource.Id.tv_loading);
			ll_loading.Visibility = ViewStates.Visible;
			handler = new MyHandler (tv_system_time, tv_current_position, vv, sb_video, this);
		}

		/// <summary>
		/// Toggles the mute.
		/// </summary>
		protected void toggleMute ()
		{
			if (GetStreamVolume () > 0) {
				currentVolume = GetStreamVolume ();
				setStreamVolume (0);
				sb_voice.Progress = 0;

			} else {
				setStreamVolume (currentVolume);
				sb_voice.Progress = currentVolume;
				btn_voice.SetBackgroundResource (Resource.Mipmap.btn_voice_normal);
			}
		}

		/// <summary>
		/// play button
		/// </summary>
		public void play ()
		{
			if (vv.IsPlaying) {
				vv.Pause ();
			} else {
				vv.Start ();
			}
			updatePlayBtn ();
		}

		//update button
		protected void updatePlayBtn ()
		{
			if (vv.IsPlaying) {
				btn_play.SetBackgroundResource (Resource.Drawable.selector_btn_pause);
			} else {
				btn_play.SetBackgroundResource (Resource.Drawable.selector_btn_play);
			}
		}

		/** change volume */
		public void changeVolume (float distanceYY)
		{
			int moveVolume = (int)(distanceYY * maxVlumeScreenHeightScale);
			int result = currentVolume + moveVolume;

			// extends 
			if (result > maxVolume) {
				result = maxVolume;
			} else if (result < 0) {
				result = 0;
			}
			setStreamVolume (result);
			sb_voice.Progress = result;
		}

		public void updateCurrentPosition ()
		{
			int cp = (int)vv.CurrentPosition;
			tv_current_position.Text = Utils.formatMillis (cp);
			sb_video.Progress = cp;
			handler.SendEmptyMessageDelayed (Constants.UPDATE_CURRENT_POSITION, 300);
		}
			
	}

	public class MyAnimatorListener:Java.Lang.Object,Animator.IAnimatorListener
	{
		public void OnAnimationCancel (Animator animation)
		{

		}

		public void OnAnimationEnd (Animator animation)
		{
			ll_loading.Visibility = ViewStates.Gone;
			ViewHelper.SetAlpha (ll_loading, 1.0f);
		}

		public void OnAnimationRepeat (Animator animation)
		{

		}

		public void OnAnimationStart (Animator animation)
		{

		}

		private LinearLayout ll_loading;

		public MyAnimatorListener (LinearLayout ll_loading)
		{
			this.ll_loading = ll_loading;
		}
	}

	/// <summary>
	/// handle message
	/// </summary>
	public class MyHandler:Handler
	{
		private TextView tv_system_time;
		private TextView tv_position;
		private SeekBar sb;
		private IO.Vov.Vitamio.Widget.VideoView vv;
		private BaseVideoFragment vp;

		public MyHandler (TextView tv_system_time, TextView tv_position, IO.Vov.Vitamio.Widget.VideoView vv, SeekBar sb, BaseVideoFragment vp)
		{
			this.tv_system_time = tv_system_time;
			this.tv_position = tv_position;
			this.vv = vv;
			this.sb = sb;
			this.vp = vp;
		}

		public override void HandleMessage (Message msg)
		{
			base.HandleMessage (msg);
			switch (msg.What) {
			case 0://update system time
				tv_system_time.Text = DateFormat.Format ("kk:mm:ss", Java.Lang.JavaSystem.CurrentTimeMillis ());
				SendEmptyMessageDelayed (Constants.UPDATE_SYSTEM_TIME, 1000);
				break;
			case 1://update current position
				tv_position.Text = Utils.formatMillis (vv.CurrentPosition);
				sb.Progress = (int)vv.CurrentPosition;
				SendEmptyMessageDelayed (Constants.UPDATE_CURRENT_POSITION, 300);
				break;
			default:
				break;
			}
		}

	}
}


using System;
using Android.Content;
using Android.Views;
using Android.Runtime;
using Java.Util;
using Android.Text.Format;
using Android.Widget;
using Android.Media;
using Android.OS;
using Android.Graphics;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Android.Util;
using Android.Net;
using Android.App;


namespace CorpTraining.Droid
{
	public static class Utils
	{
		public static int getWindowWidth (Context context)
		{
			IWindowManager windowManager = context.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ();
			return windowManager.DefaultDisplay.Width;
		}

		public static int getWindowHeight (Context context)
		{
			IWindowManager windowManager = context.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ();
			return windowManager.DefaultDisplay.Height;
		}


		public static string formatMillis (long time)
		{
			Calendar calendar = Calendar.Instance;
			calendar.Clear ();
			calendar.Add (Calendar.Millisecond, (int)time);
			String pattern = time / Constants.hourMillis > 0 ? "kk:mm:ss" : "mm:ss";
			return DateFormat.Format (pattern, calendar);
		}

		//set and play music
		public static void setAndPlayMusic (Context context, View view, string audioUrl, Handler handler, MediaPlayer mp)
		{
			Button btn_play;
			SeekBar sb_audio;
			TextView tv_play_time;
			SeekBar sb_voice;
			btn_play = view.FindViewById<Button> (Resource.Id.btn_play);
			sb_audio = view.FindViewById<SeekBar> (Resource.Id.sb_audio);
			tv_play_time = view.FindViewById<TextView> (Resource.Id.tv_play_time);
			sb_voice = view.FindViewById<SeekBar> (Resource.Id.sb_voice);
			Utils.openAudio (context, mp, EncodeURL (audioUrl));
			mp.Prepared += delegate(object sender, EventArgs e) {
				//play
				mp.Start ();
				//update play button
				int id;
				if (mp.IsPlaying) {
					id = Resource.Drawable.selector_audio_btn_pause;
				} else {
					id = Resource.Drawable.selector_audio_btn_play;
				}
				btn_play.SetBackgroundResource (id);
				//set play-time
				tv_play_time.Text = Utils.formatMillis (mp.CurrentPosition);
				//set seekbar
				sb_audio.Max = mp.Duration;
				sb_audio.Progress = mp.CurrentPosition;
				//set volume
				AudioManager audioManager = (AudioManager)context.GetSystemService (Context.AudioService);
				int maxVolume = audioManager.GetStreamMaxVolume (Android.Media.Stream.Music);
				int currentVolume = audioManager.GetStreamVolume (Android.Media.Stream.Music);
				sb_voice.Max = maxVolume;
				sb_voice.Progress = currentVolume;
				//update
				sendUpdateTime (handler, tv_play_time, sb_audio, mp);
				//set voice
				sb_voice.ProgressChanged += delegate(object s, SeekBar.ProgressChangedEventArgs arg) {
					if (arg.FromUser) {
						audioManager.SetStreamVolume (Android.Media.Stream.Music, arg.Progress, VolumeNotificationFlags.PlaySound);
					}
				};
				//when the mediaplayer finish
				mp.Completion += delegate(object s, EventArgs arg) {
					mp.SeekTo (0);
					tv_play_time.Text = Utils.formatMillis (0L);
					btn_play.SetBackgroundResource (Resource.Drawable.selector_btn_play);
				};
			};
			//set onclick
			btn_play.Click += delegate(object sender, EventArgs e) {
				int id;
				if (mp.IsPlaying) {
					mp.Pause ();
					id = Resource.Drawable.selector_audio_btn_play;
				} else {
					mp.Start ();
					id = Resource.Drawable.selector_audio_btn_pause;
				}
				btn_play.SetBackgroundResource (id);

			};
		}

		//open audio
		public static void openAudio (Context context, MediaPlayer mp, string audioUri)
		{
			if (!string.IsNullOrEmpty (audioUri)) {
				//close other music or video
				Intent i = new Intent ("com.android.music.musicservicecommand");
				i.PutExtra ("command", "pause");
				context.SendBroadcast (i);
			}
			try {
				mp.SetDataSource (context, Android.Net.Uri.Parse (audioUri));
				mp.PrepareAsync ();

			} catch (Exception ex) {
				ex.ToString ();
			}
		}

		//send update time
		public static void sendUpdateTime (Handler handler, TextView tv_play_time, SeekBar sb_audio, MediaPlayer mp)
		{
			handler.PostDelayed (delegate () {
				tv_play_time.Text = Utils.formatMillis (mp.CurrentPosition);
				sb_audio.Progress = mp.CurrentPosition;
				sendUpdateTime (handler, tv_play_time, sb_audio, mp);
			}, 300);
		}

		//set bitmap
		public static async Task setImageView (ImageView iv, string url)
		{
			Bitmap bitmap = null;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (EncodeURL (url));
			request.Method = "GET";
			request.ContentType = "multipart/form-data";
			using (WebResponse response = await request.GetResponseAsync ()) {
				using (System.IO.Stream stream = response.GetResponseStream ()) {
					bitmap = BitmapFactory.DecodeStream (stream);
				}
			}
			iv.SetImageBitmap (bitmap);
		}

		public static string EncodeURL (string str)
		{
			String url = str.Replace (" ", "%20");
			return url;
		}

		public static string populateSelection (int order)
		{
			string selection = "";
			switch (order) {
			case 0:
				selection = "A";
				break;
			case 1:
				selection = "B";
				break;
			case 2:
				selection = "C";
				break;
			case 3:
				selection = "D";
				break;
			case 4:
				selection = "E";
				break;
			case 5:
				selection = "F";
				break;
			case 6:
				selection = "G";
				break;
			}
			return selection;
		}

		public static void makeTextViews (IList<Text> texts, Context context, LinearLayout ll_text, Color color)
		{
			if (texts == null) {
				TextView textview = new TextView (context);
				var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 0, 1.0f);
				textview.SetTextColor (Color.White);
				textview.SetTextSize (ComplexUnitType.Sp, 20.0f);
				textview.Gravity = GravityFlags.Start;
				textview.Text = "Watching Video...";
				ll_text.AddView (textview, param);
			} else {
				texts = new List<Text> (texts);
				if (texts != null && texts.Count > 0) {
					foreach (var text in texts) {
						TextView textview = new TextView (context);
						var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 0, 1.0f);
						textview.SetTextColor (color);
						textview.SetTextSize (ComplexUnitType.Sp, 20.0f);
						textview.Gravity = GravityFlags.Start;
						textview.Text = text.TextValue;
						textview.LayoutParameters = param;
						param.BottomMargin = 10;
						ll_text.AddView (textview, param);
					}
				}
			}
		}

		public static bool isNetworkAvailable (Context context)
		{   
			ConnectivityManager cm = (ConnectivityManager)context   
				.GetSystemService (Context.ConnectivityService);   
			if (cm == null) {   
			} else {
				NetworkInfo[] info = cm.GetAllNetworkInfo ();   
				if (info != null) {   
					for (int i = 0; i < info.Length; i++) {   
						if (info [i].GetState () == NetworkInfo.State.Connected) {   
							return true;   
						}   
					}   
				}   
			}   
			return false;   
		}

		public static void makeTextImages (Context context, LinearLayout ll_images, List<Image> images)
		{
			if (images != null && images.Count > 0) {
				foreach (var image in images) {
					//imageview
					ImageView iv = new ImageView (context);
					var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 0, 4.0f);
					//set default image
					iv.SetImageResource (Resource.Mipmap.default_bitmap);
					Utils.setImageView (iv, image.Url);
					ll_images.AddView (iv, param);
					//textview
					TextView tv = new TextView (context);
					var textparam = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 0, 1.0f);
					tv.Text = image.Title;
					ll_images.AddView (tv, textparam);
				}
			}
		}

		public static void makeReturnButton (Context context, LinearLayout ll_content)
		{
			Button button = new Button (context);
			var param = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			button.Text = "Back to Lesson List";
			button.SetBackgroundResource (Resource.Drawable.btn_red_selector);
			button.SetTextSize (ComplexUnitType.Sp, 20.0f);
			button.SetTextColor (Color.Black);
			param.LeftMargin = 10;
			param.RightMargin = 10;
			param.TopMargin = 15;
			button.Click += delegate(object sender, EventArgs e) {
				(context as TextActivity).OnBackPressed ();
			};
			ll_content.AddView (button, param);
		}
	}
}


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

		public static string EncodeURL (string str)
		{
			String url = str.Replace (" ", "%20");
			return url;
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
			Utils.openAudio (context, mp, audioUrl);
			mp.Prepared += delegate(object sender, EventArgs e) {
				//play
				mp.Start ();
				//update play button
				int id;
				if (mp.IsPlaying) {
					id = Resource.Drawable.selector_btn_pause;
				} else {
					id = Resource.Drawable.selector_btn_play;
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
					id = Resource.Drawable.selector_btn_play;
				} else {
					mp.Start ();
					id = Resource.Drawable.selector_btn_pause;
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
				mp.Completion += delegate(object sender, EventArgs e) {

				};
				mp.SetDataSource (context, Android.Net.Uri.Parse (EncodeURL (audioUri)));
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

	}
}


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

namespace CorpTraining.Droid
{
	public class BatteryChangedReceiver : BroadcastReceiver
	{
		private ImageView iv_battery;

		public BatteryChangedReceiver (ImageView iv_battery)
		{
			this.iv_battery = iv_battery;
		}

		public override void OnReceive (Context context, Intent intent)
		{
			//var set=intent.Extras.KeySet ();
			int level = intent.GetIntExtra ("level", 0);
			updateBatteryBg (level);
		}

		public void updateBatteryBg (int level)
		{
			int resid;
			if (level == 0) {
				resid = Resource.Mipmap.ic_battery_0;
			} else if (level <= 10) {
				resid = Resource.Mipmap.ic_battery_10;
			} else if (level <= 20) {
				resid = Resource.Mipmap.ic_battery_20;
			} else if (level <= 40) {
				resid = Resource.Mipmap.ic_battery_40;
			} else if (level <= 60) {
				resid = Resource.Mipmap.ic_battery_60;
			} else if (level <= 80) {
				resid = Resource.Mipmap.ic_battery_80;
			} else {
				resid = Resource.Mipmap.ic_battery_100;
			}
			iv_battery.SetBackgroundResource (resid);
		}
	}
}



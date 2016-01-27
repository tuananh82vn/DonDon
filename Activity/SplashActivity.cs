
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Views.Animations;
using Android.Net;
using Java.Net;


namespace DonDon
{

	[Activity(MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		public ImageView imageLogo;
		public Animation rotateAboutCenterAnimation;
		public System.Timers.Timer _backgroundtimer;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.SplashLayout);

			RequestedOrientation = ScreenOrientation.SensorPortrait;

			imageLogo = FindViewById<ImageView>(Resource.Id.floating_image);


			Init ();

//			if (NetworkHelper.DetectNetwork()) 
//			{
//				
//			} 
//			else 
//			{
//				Toast.MakeText (this, "No Connection ...", ToastLength.Short).Show ();
//				KeepChecking ();
//			}

		}

		private void Init()
		{
			// Simulate a long loading process on app startup.
			Task<bool>.Run (() => {
				Thread.Sleep (2000);
				if(Settings.RestaurantId != 0 ){


					StartActivity(typeof(LoginActivity));
				}
				else
				{
					StartActivity(typeof(RestaurantSelectActivity));
				}
				this.Finish();
			}); 
		}

		private void KeepChecking(){
			_backgroundtimer = new System.Timers.Timer ();
			//Trigger event every second
			_backgroundtimer.Interval = 6000;
			_backgroundtimer.Elapsed += OnTimeBackgrounddEvent;
			_backgroundtimer.Start ();
		}

		private void OnTimeBackgrounddEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (NetworkHelper.DetectNetwork()) 
			{
				_backgroundtimer.Stop ();
				Init ();
			} 
			else 
			{
				RunOnUiThread (() => Toast.MakeText (this, "No Connection ...", ToastLength.Long).Show ());
			}
		}
	}
}


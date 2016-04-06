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
using Android.Views.InputMethods;
using System.Threading;
using Android.Content.PM;

namespace DonDon
{
	[Activity (Label = "RestaurantSelectActivity")]			
	public class RestaurantSelectActivity : Activity
	{

		public Spinner spinner_Restaurant ;
		public RestaurantSpinnerAdapter restaurantList; 
		public int RestaurantId = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.RestaurantSelect);

			RequestedOrientation = ScreenOrientation.SensorPortrait;


			spinner_Restaurant = FindViewById<Spinner> (Resource.Id.spinner_Restaurant);

			Button button = FindViewById<Button>(Resource.Id.btLogin);
			button.Click += btloginClick;  

			GetRestaurantList ();

			// Create your application here
		}

		public void btloginClick(object sender, EventArgs e)
		{
			if (RestaurantId != 0 ) {

				ThreadPool.QueueUserWorkItem (o => Next ());

			} 
			else 
			{
				RunOnUiThread (() => Toast.MakeText (this, "Please select restaurant to login", ToastLength.Short).Show ());
			}
		}

		private void Next(){

			Settings.RestaurantId  = RestaurantId;

			StartActivity(typeof(LoginActivity));

			this.Finish ();

		}

		private void GetRestaurantList(){

			if(NetworkHelper.DetectNetwork()){

				restaurantList = new RestaurantSpinnerAdapter (this,RestaurantController.GetRestaurantList());

				spinner_Restaurant.Adapter = restaurantList;

				spinner_Restaurant.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Restaurant_ItemSelected);
			}
			else
			{
				Toast.MakeText (this, "No internet connection", ToastLength.Short).Show ();
			}

		}

		private void Restaurant_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			RestaurantId = restaurantList.GetItemAtPosition (e.Position).Id;
		}

	}

			
}


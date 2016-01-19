
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
using Android.Content.PM;
using Android.Graphics;

namespace DonDon
{
	[Activity (Label = "HomeActivity")]			
	public class HomeActivity : Activity
	{

		const int Start_DATE_DIALOG_ID = 0;

		private DateTime StartDate;

		private EditText StartDatePicker;

		public List<OrderList> _OrderList;

		public OrderListAdapter orderListAdapter;

		public ListView orderListView;

		public DatePicker orderDate;

		public ProgressDialog progress;

		public string results;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Home);

			orderListView = FindViewById<ListView> (Resource.Id.OrderListView);

			StartDatePicker = FindViewById<EditText> (Resource.Id.editText_OrderDate);
			StartDatePicker.InputType = 0;

			StartDatePicker.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
	
			StartDate = DateTime.Today;
			StartDatePicker.Text = StartDate.ToString ("dd'/'MM'/'yyyy");

			Button buttonView = FindViewById<Button>(Resource.Id.bt_View);
			buttonView.Click += btViewClick;  

			Button buttonOrder = FindViewById<Button>(Resource.Id.bt_Order);
			buttonOrder.Click += btOrderClick;  

			LoadOrderList ();
		}

		public void LoadOrderList()
		{
			var  items = Intent.GetParcelableArrayListExtra("key");
			if (items != null) {

				items = items.Cast<OrderList1> ().ToArray ();

			}
		}

		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case Start_DATE_DIALOG_ID:
				{
					var datePicker = new DatePickerDialog (this, OnStartDateSet, StartDate.Year, StartDate.Month - 1, StartDate.Day); 
					datePicker.DatePicker.MaxDate = Utility.SetMaxDate (0);
					return datePicker;
				}
			}
			return null;

		}

		// the event received when the user "sets" the date in the dialog
		void OnStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			StartDatePicker.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
			this.StartDate = e.Date;
		}


		//Loading data
		public void InitData(){

			progress = new ProgressDialog (this,Resource.Style.StyledDialog);
			progress.Indeterminate = true;
			progress.SetMessage("Please wait...");
			progress.SetCancelable (true);
			progress.Show ();

			orderListAdapter = new OrderListAdapter (this, OrderController.GetOrderList(StartDate));

			orderListView.Adapter = orderListAdapter;

			RegisterForContextMenu(orderListView);

			progress.Dismiss ();
		}

		public void btViewClick(object sender, EventArgs e)
		{
			InitData ();
		}

		public void btOrderClick(object sender, EventArgs e)
		{
			StartActivity(typeof(OrderActivity));
		}
	}
}


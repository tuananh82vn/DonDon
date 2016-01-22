
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

			RequestedOrientation = ScreenOrientation.SensorPortrait;


			orderListView = FindViewById<ListView> (Resource.Id.OrderListView);

			StartDatePicker = FindViewById<EditText> (Resource.Id.editText_OrderDate);
			StartDatePicker.InputType = 0;

			StartDatePicker.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
	
			StartDate = Utility.GetTodayDate ();
			StartDatePicker.Text = StartDate.ToString ("dd'/'MM'/'yyyy");

			Button buttonView = FindViewById<Button>(Resource.Id.bt_View);
			buttonView.Click += btViewClick;  

			Button buttonOrder = FindViewById<Button>(Resource.Id.bt_Order);
			buttonOrder.Click += btOrderClick;  

			Button buttonSend = FindViewById<Button>(Resource.Id.bt_Send);
			buttonSend.Click += btSendClick;  

			LoadOrderList ();
		}

		public void LoadOrderList()
		{
			var  items = Intent.GetParcelableArrayListExtra("key");
			if (items != null) {
				
				items = items.Cast<OrderList1> ().ToArray ();

				List<OrderList> orderList = new List<OrderList> ();

				foreach (OrderList1 item in items) {
					OrderList order = new OrderList ();
					order.StockId = item.Id;
					order.StockName = item.StockName;
					order.Unit = item.Unit;

					order.OrderNumber = item.ShouldNumber - item.StockNumber;
					order.ShouldNumber = item.ShouldNumber;
					order.StockNumber = item.StockNumber;

					if (order.OrderNumber < 0) {
						order.OrderNumber = 0;
					}

					orderList.Add (order);

				}

				orderListAdapter = new OrderListAdapter (this, orderList);
				orderListView.Adapter = orderListAdapter;

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
			Intent Intent = new Intent (this, typeof(OrderActivity));

			var orderList = this.orderListAdapter.GetOrderList();

			List<OrderList1> Items = new List<OrderList1> ();

			foreach (var order in orderList) {
				OrderList1 item = new OrderList1 (order.StockId, order.StockName, order.ShouldNumber, order.StockNumber, order.OrderNumber, order.Unit);
				Items.Add (item);
			}

			Intent.PutParcelableArrayListExtra("key", Items.ToArray());

			StartActivity (Intent);

		}

		public void btSendClick(object sender, EventArgs e)
		{
				new AlertDialog.Builder(this)
				.SetPositiveButton("Yes", async (sender1, args) =>
				{
						ApiResultSave result = await OrderController.SendOrderList (orderListAdapter.GetOrderList());

						if (result != null) 
						{
							if (result.Success) 
							{
									var builder = new AlertDialog.Builder(this);
									builder.SetMessage("Order sent successfully");
									builder.SetPositiveButton("Ok", (s, ee) => { });
									builder.Create().Show();
							}
							else{
								new AlertDialog.Builder(this).SetMessage(result.ErrorMessage)
									.SetTitle("Done")
									.Show();
							}
						}
						else{
							new AlertDialog.Builder(this).SetMessage("Network or Server problem. Try again")
								.SetTitle("Done")
								.Show();	
						}
				})
				.SetNegativeButton("No", (sender2, args) =>
					{
						// User pressed no 
					})
				.SetMessage("Are you sure to send this order ?")
				.SetTitle("Confirm")
				.Show();
		}
	}
}


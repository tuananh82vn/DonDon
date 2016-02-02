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

		public InputMethodManager inputManager;

		public EditText mNotes;

		public TextView tv_Username;

		public Button buttonOrder;

		public Button buttonSend;

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

			buttonOrder = FindViewById<Button>(Resource.Id.bt_Order);
			buttonOrder.Click += btOrderClick;  

			buttonSend = FindViewById<Button>(Resource.Id.bt_Send);
			buttonSend.Click += btSendClick;  

			mNotes = FindViewById<EditText>(Resource.Id.editText_Notes);

			tv_Username = FindViewById<TextView>(Resource.Id.textView_Username);
			tv_Username.Text = Settings.Fullname;

			HideSoftKeyboard1 (this);

			LoadOrderList ();


		}


		public void HideSoftKeyboard1(Activity activity)
		{
			var view = activity.CurrentFocus;
			if (view != null)
			{
				InputMethodManager manager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
				manager.HideSoftInputFromWindow(view.WindowToken, 0);
			}
		}

		public override bool DispatchTouchEvent(MotionEvent event1) {
			if (event1.Action == MotionEventActions.Down) {
				View v = this.CurrentFocus;
				if ( v is EditText) {
					Rect outRect = new Rect();
					v.GetGlobalVisibleRect(outRect);
					if (!outRect.Contains((int)event1.GetX(), (int)event1.GetY())) {
						v.ClearFocus();
						InputMethodManager imm = (InputMethodManager) GetSystemService(Context.InputMethodService);
						imm.HideSoftInputFromWindow(v.WindowToken, 0);
					}
				}
			}
			return base.DispatchTouchEvent( event1 );
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
					order.ShouldNumber = item.ShouldNumber;
					order.StockNumber = item.StockNumber;
					order.OrderNumber = item.OrderNumber;


					orderList.Add (order);

				}

				orderListAdapter = new OrderListAdapter (this, orderList);
				orderListView.Adapter = orderListAdapter;

				this.mNotes.Text = OrderController.GetOrderNotes (Utility.GetTodayDate ());

			}
			else
			{			
				InitData ();
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
			InitData ();
		}


		public override void OnBackPressed(){
			
				new AlertDialog.Builder(this)
					.SetPositiveButton("Yes", (sender1, args) =>
						{
							this.Finish();
							Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
						})
					.SetNegativeButton("No", (sender2, args) =>
						{
							// User pressed no 
						})
					.SetMessage("Do you want to exit the application ?")
					.SetTitle("Confirm")
					.Show();
			 

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

			this.mNotes.Text = OrderController.GetOrderNotes (StartDate);


			RegisterForContextMenu(orderListView);


			if (StartDate != Utility.GetTodayDate ()) {
				this.buttonOrder.Visibility = ViewStates.Invisible;
				this.buttonSend.Visibility = ViewStates.Invisible;
			} else {
				this.buttonOrder.Visibility = ViewStates.Visible;
				this.buttonSend.Visibility = ViewStates.Visible;
			}

			progress.Dismiss ();
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

			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

		}

		public void btSendClick(object sender, EventArgs e)
		{
				new AlertDialog.Builder(this)
				.SetPositiveButton("Yes", async (sender1, args) =>
				{
						progress = new ProgressDialog (this,Resource.Style.StyledDialog);
						progress.Indeterminate = true;
						progress.SetMessage("Please wait...");
						progress.SetCancelable (true);
						progress.Show ();

						ApiResultSave result = await OrderController.SendOrderList (orderListAdapter.GetOrderList(), this.mNotes.Text);

						if (result != null) 
						{
							if (result.Success) 
							{
									progress.Dismiss ();

									var builder = new AlertDialog.Builder(this);
									builder.SetMessage("Order sent successfully.");
									builder.SetPositiveButton("Ok", (s, ee) => { });
									builder.Create().Show();
							}
							else{
								
									progress.Dismiss ();

									new AlertDialog.Builder(this).SetMessage(result.ErrorMessage)
										.SetTitle("Done")
										.Show();
							}
						}
						else
						{
							progress.Dismiss ();

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


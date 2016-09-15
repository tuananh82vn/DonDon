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

		public Button buttonAmend;


		public int OptionSendSelected = 0;

		public string Version = "";


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Home);

			RequestedOrientation = ScreenOrientation.SensorPortrait;

			orderListView = FindViewById<ListView> (Resource.Id.OrderListView);

			StartDatePicker = FindViewById<EditText> (Resource.Id.editText_OrderDate);
			StartDatePicker.InputType = 0;

			StartDatePicker.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
	
			//StartDate = Utility.GetTodayDate();
			//StartDatePicker.Text = StartDate.ToString ("dd'/'MM'/'yyyy");

			buttonOrder = FindViewById<Button>(Resource.Id.bt_Order);
			buttonOrder.Click += btOrderClick;  

			buttonSend = FindViewById<Button>(Resource.Id.bt_Send);
			buttonSend.Click += btSendClick;  

			buttonAmend = FindViewById<Button>(Resource.Id.bt_Amend);
			buttonAmend.Click += btAmendClick;  

			mNotes = FindViewById<EditText>(Resource.Id.editText_Notes);

			tv_Username = FindViewById<TextView>(Resource.Id.textView_Username);
			tv_Username.Text = Settings.Fullname;

			HideSoftKeyboard1 (this);

			LoadOrderList ();

			if (Settings.CKStaff) {
				this.buttonAmend.Visibility = ViewStates.Invisible;
			}

			Context context = this.ApplicationContext;
			Version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;

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

			StartDate = Utility.GetTodayDate();
			StartDatePicker.Text = StartDate.ToString("dd'/'MM'/'yyyy");

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
					if (item.IsSkip == 0) {
						order.IsSkip = false;
					}
					else
						order.IsSkip = true;
					
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

		[Obsolete]
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
						LoginController.Log(Constant.ButtonBackExitClick);
						this.Finish();
						Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
					})
				.SetNegativeButton("No", (sender2, args) =>
					{

					})
				.SetMessage("Do you want to exit the application?")
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
				this.buttonAmend.Visibility = ViewStates.Invisible;
			} else {
				this.buttonOrder.Visibility = ViewStates.Visible;
				this.buttonSend.Visibility = ViewStates.Visible;

				if(!Settings.CKStaff)
					this.buttonAmend.Visibility = ViewStates.Visible;
			}

			progress.Dismiss ();
		}

		public void btOrderClick(object sender, EventArgs e)
		{
			LoginController.Log(Constant.ButtonOrderClick);

			Intent Intent = new Intent (this, typeof(OrderActivity));

			var CurrentDate = Utility.GetTodayDate();

			List<OrderList> orderList = new List<OrderList>();

			//get current data
			if (CurrentDate == this.StartDate)
			{
				LoginController.Log("CurrentDate == this.StartDate: " + this.StartDate.ToShortDateString());

				orderList = this.orderListAdapter.GetOrderList();
			}
			//get the newest data
			else
			{
				LoginController.Log("CurrentDate != this.StartDate: " + CurrentDate.ToShortDateString());

				orderList = OrderController.GetOrderList(CurrentDate);
			}

			List<OrderList1> Items = new List<OrderList1>();

			foreach (var order in orderList)
			{
				var isSkip = 0;

				if (order.IsSkip)
				{
					isSkip = 1;
				}

				OrderList1 item = new OrderList1(order.StockId, order.StockName, order.ShouldNumber, order.StockNumber, order.OrderNumber, order.Unit, isSkip);
				Items.Add(item);
			}

			Intent.PutExtra("type", "order");

			Intent.PutParcelableArrayListExtra("key", Items.ToArray());

			StartActivity (Intent);

			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

		}

		public void btAmendClick(object sender, EventArgs e)
		{

			new AlertDialog.Builder(this)
				.SetPositiveButton("Yes", async (sender1, args) =>
					{

						LoginController.Log(Constant.ButtonAmendClick);

						Intent Intent = new Intent (this, typeof(OrderActivity));

						var orderList = this.orderListAdapter.GetOrderList();

						List<OrderList1> Items = new List<OrderList1> ();

						foreach (var order in orderList) {

							var isSkip = 0;

							if (order.IsSkip) {
								isSkip = 1;
							}

							OrderList1 item = new OrderList1 (order.StockId, order.StockName, order.ShouldNumber, order.StockNumber, order.OrderNumber, order.Unit, isSkip);
							Items.Add (item);
						}

						Intent.PutParcelableArrayListExtra("key", Items.ToArray());

						Intent.PutExtra("type", "amend");

						StartActivity (Intent);

						this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

					})
				.SetNegativeButton("No", (sender3, args) =>
					{
						// User pressed no 
					})
				.SetTitle("This button will allow you to enter or change the order number, Continue ?")
				.Show();
		}

		private void ListClicked (object sender, DialogClickEventArgs e)
		{
			OptionSendSelected = e.Which;
		}

		public void btSendClick(object sender, EventArgs e)
		{

				OptionSendSelected = 0;

				string[] color_options  = new string[] {"Send All","Send to Daiwa","Send to Centre Kitchen"};


				new AlertDialog.Builder(this)
				.SetPositiveButton("Send", async (sender1, args) =>
				{

							progress = new ProgressDialog (this,Resource.Style.StyledDialog);
							progress.Indeterminate = true;
							progress.SetMessage("Please wait...");
							progress.SetCancelable (true);
							progress.Show ();

							ApiResultSave result = await OrderController.SendOrderList (orderListAdapter.GetOrderList(), this.mNotes.Text, OptionSendSelected);

							if (result != null) 
							{
								if (result.Success) 
								{
										progress.Dismiss ();

										var builder = new AlertDialog.Builder(this);
										string message = "";

										if(OptionSendSelected == 0){
											message = "Order sent all successfully.";
											LoginController.Log(Constant.ButtonSendAllClick+"_"+Version);

										}
										else if(OptionSendSelected == 1)
										{
											message = "Order sent to Daiwa successfully.";
											LoginController.Log(Constant.ButtonSendDWClick+ "_" + Version);

										}
										else if(OptionSendSelected == 2)
										{
											message = "Order sent to Centre Kitchen successfully.";
											LoginController.Log(Constant.ButtonSendCKClick+ "_" + Version);

										}

										builder.SetMessage(message);
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
				
				.SetSingleChoiceItems(color_options, 0, ListClicked)

				.SetNegativeButton("Cancel", (sender3, args) =>
					{
						// User pressed no 
					})
				.SetTitle("Please carefully select which option you want to send order ?")
				.Show();
		}
	}
}


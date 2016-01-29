
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
using Android.Graphics;
using Com.Telerik.Widget.List;
using Android.Content.PM;

namespace DonDon
{
	[Activity (Label = "OrderActivity", NoHistory = true )]			
	public class OrderActivity : Activity
	{

		public OrderAddListAdapter orderListAdapter;

		public ListView orderListView;

		public ProgressDialog progress;

		private DateTime StartDate;

		public TextView tv_Date;

		public TextView tv_StockName;

		public TextView tv_Unit;

		public TextView tv_Stock;


		public EditText edit_Stock;

		public Button bt_Next;

		public Button bt_Back;

		public Button bt_Finish;

		public Button bt_Skip;

		public int selectedIndex;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Order);

			RequestedOrientation = ScreenOrientation.SensorPortrait;

			initControl ();

			StartDate = Utility.GetTodayDate();

			tv_Date.Text = StartDate.ToShortDateString ();

			if (LoadOrderList () == 0) {
				InitData ();
			}

			//Set first item selected;
			selectedIndex = 0;

			this.orderListView.SetItemChecked (selectedIndex, true);
			this.orderListView.ItemClick += listView_ItemClick;

			this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;
			this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;
			this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString();



			if (Settings.CKStaff) {
				this.tv_Stock.Text = "Order";
				this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).OrderNumber.ToString();
			}

			this.edit_Stock.RequestFocus ();
			this.edit_Stock.SetSelection (this.edit_Stock.Text.Length);
		}

		public int LoadOrderList()
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

					if (Settings.CKStaff) {
						order.OrderNumber = item.OrderNumber;
					}


					order.ShouldNumber = item.ShouldNumber;
					order.StockNumber = item.StockNumber;

					if (order.OrderNumber < 0) {
						order.OrderNumber = 0;
					}

					orderList.Add (order);

				}

				orderListAdapter = new OrderAddListAdapter (this, orderList);
				orderListView.Adapter = orderListAdapter;

				return 1;
			} 
			else 
			{
				return 0;
			}
		}

		public override void OnBackPressed(){

			Intent Intent = new Intent (this, typeof(HomeActivity));

			var orderList = this.orderListAdapter.GetOrderList();

			List<OrderList1> Items = new List<OrderList1> ();

			foreach (var order in orderList) {
				OrderList1 item = new OrderList1 (order.StockId, order.StockName, order.ShouldNumber, order.StockNumber, order.OrderNumber, order.Unit);
				Items.Add (item);
			}

			Intent.PutParcelableArrayListExtra("key", Items.ToArray());

			Intent.SetFlags (ActivityFlags.ClearTask | ActivityFlags.NewTask);

			StartActivity (Intent);

			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

			this.Finish();

		}

		public void initControl(){

			orderListView = FindViewById<ListView> (Resource.Id.OrderListView);
			orderListView.ChoiceMode = ChoiceMode.Single;
			orderListView.RequestFocusFromTouch ();


			tv_Date = FindViewById<TextView> (Resource.Id.tv_Date);
			edit_Stock = FindViewById<EditText> (Resource.Id.edit_Stock);
			tv_StockName = FindViewById<TextView> (Resource.Id.tv_StockName);
			tv_Unit = FindViewById<TextView> (Resource.Id.tv_Unit);

			tv_Stock = FindViewById<TextView> (Resource.Id.tv_Stock);

			bt_Next = FindViewById<Button>(Resource.Id.bt_Next);
			bt_Next.Click += btNextClick;  

			bt_Back = FindViewById<Button>(Resource.Id.bt_Back);
			bt_Back.Click += btBackClick;  

			bt_Finish = FindViewById<Button>(Resource.Id.bt_Finish);
			bt_Finish.Click += btFinalClick;  

			bt_Skip = FindViewById<Button>(Resource.Id.bt_Skip);
			bt_Skip.Click += btSkipClick; 
		}

		public void btFinalClick(object sender, EventArgs e)
		{
			Intent Intent = new Intent (this, typeof(HomeActivity));

			var orderList = this.orderListAdapter.GetOrderList();

			List<OrderList1> Items = new List<OrderList1> ();

			foreach (var order in orderList) {
				OrderList1 item = new OrderList1 (order.StockId, order.StockName, order.ShouldNumber, order.StockNumber, order.OrderNumber, order.Unit);
				Items.Add (item);
			}

			Intent.PutParcelableArrayListExtra("key", Items.ToArray());

			Intent.SetFlags (ActivityFlags.ClearTask | ActivityFlags.NewTask);

			StartActivity (Intent);

			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

			this.Finish();

		}

		public void btNextClick(object sender, EventArgs e)
		{

			//Get edittext 
			if (edit_Stock.Text != "") 
			{
				try {
					
					var stockNumber = Int32.Parse (edit_Stock.Text);


					if (Settings.CKStaff) {
						this.orderListAdapter.SetOrderAtPosition (selectedIndex, stockNumber);
					}
					else
					{
						this.orderListAdapter.SetStockAtPosition (selectedIndex, stockNumber);
					}


				} catch (Exception ew) {
					Toast.MakeText (this, "Input not valid", ToastLength.Short).Show ();
					return;
				}
			}

			//Move to next item
			if (this.selectedIndex != this.orderListAdapter.Count - 1) {

				this.selectedIndex = this.selectedIndex + 1;

				this.orderListView.SetItemChecked (selectedIndex, true);

				this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;

				this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;



				if (Settings.CKStaff) {
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).OrderNumber.ToString ();
				}
				else
				{
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString ();
				}


				this.edit_Stock.RequestFocus ();
				this.edit_Stock.SetSelection (this.edit_Stock.Text.Length);

			}else {
				Toast.MakeText (this, "This is the last item already.", ToastLength.Short).Show ();
			}

		}

		public void btSkipClick(object sender, EventArgs e)
		{

			this.orderListAdapter.SetOrderAtPosition (selectedIndex, -1);

			//Move to next item
			if (this.selectedIndex != this.orderListAdapter.Count - 1) {

				this.selectedIndex = this.selectedIndex + 1;

				this.orderListView.SetItemChecked (selectedIndex, true);

				this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;

				this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;


				if (Settings.CKStaff) {
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).OrderNumber.ToString ();
				}
				else
				{
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString ();
				}


				this.edit_Stock.RequestFocus ();

				this.edit_Stock.SetSelection (this.edit_Stock.Text.Length);

			}else {
				Toast.MakeText (this, "This is the last item already.", ToastLength.Short).Show ();
			}


		}


		public void btBackClick(object sender, EventArgs e)
		{

			//Get edittext 
			if(edit_Stock.Text != ""){
				try{
					var stockNumber =  Int32.Parse(edit_Stock.Text);

					if (Settings.CKStaff) {
						this.orderListAdapter.SetOrderAtPosition (selectedIndex, stockNumber);
					}
					else
					{
						this.orderListAdapter.SetStockAtPosition (selectedIndex, stockNumber);
					}				}
				catch(Exception ew){
					Toast.MakeText (this, "Input not valid", ToastLength.Short).Show ();
					return;
				}
			}

			//Move to previous item
			if (this.selectedIndex != 0) {

				this.selectedIndex = this.selectedIndex - 1;

				this.orderListView.SetItemChecked (selectedIndex, true);

				this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;

				this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;

				if (Settings.CKStaff) {
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).OrderNumber.ToString ();
				}
				else
				{
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString ();
				}

				this.edit_Stock.RequestFocus ();
				this.edit_Stock.SetSelection (this.edit_Stock.Text.Length);
			} else {
				Toast.MakeText (this, "This is the first item already.", ToastLength.Short).Show ();
			}
		}



		//Loading data
		public void InitData(){

			progress = new ProgressDialog (this,Resource.Style.StyledDialog);
			progress.Indeterminate = true;
			progress.SetMessage("Please wait...");
			progress.SetCancelable (true);
			progress.Show ();

			orderListAdapter = new OrderAddListAdapter (this, OrderController.GetOrderList(StartDate));

			orderListView.Adapter = orderListAdapter;


			RegisterForContextMenu(orderListView);

			progress.Dismiss ();
		}


		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			this.selectedIndex = e.Position;
			this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (e.Position).StockName;
			this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (e.Position).Unit;

		}
	}
}


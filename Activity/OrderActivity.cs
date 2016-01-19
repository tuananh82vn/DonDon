
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

namespace DonDon
{
	[Activity (Label = "OrderActivity")]			
	public class OrderActivity : Activity
	{

		public OrderAddListAdapter orderListAdapter;

		public ListView orderListView;

		public ProgressDialog progress;

		private DateTime StartDate;

		public TextView tv_Date;

		public TextView tv_StockName;

		public TextView tv_Unit;

		public EditText edit_Stock;

		public Button bt_Next;

		public Button bt_Back;

		public Button bt_Skip;

		public Button bt_Finish;


		public int selectedIndex;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Order);

			initControl ();

			StartDate = DateTime.Today;

			tv_Date.Text = DateTime.Today.ToShortDateString ();

			InitData ();

			//Set first item selected;
			selectedIndex = 0;

			this.orderListView.SetItemChecked (selectedIndex, true);

			this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;

			this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;

			this.edit_Stock.RequestFocus ();

		}

		public void initControl(){

			orderListView = FindViewById<ListView> (Resource.Id.OrderListView);
			orderListView.ChoiceMode = ChoiceMode.Single;
			orderListView.RequestFocusFromTouch ();


			tv_Date = FindViewById<TextView> (Resource.Id.tv_Date);
			edit_Stock = FindViewById<EditText> (Resource.Id.edit_Stock);
			tv_StockName = FindViewById<TextView> (Resource.Id.tv_StockName);
			tv_Unit = FindViewById<TextView> (Resource.Id.tv_Unit);

			bt_Next = FindViewById<Button>(Resource.Id.bt_Next);
			bt_Next.Click += btNextClick;  

			bt_Back = FindViewById<Button>(Resource.Id.bt_Back);
			bt_Back.Click += btBackClick;  

			bt_Skip = FindViewById<Button>(Resource.Id.bt_Skip);
			bt_Skip.Click += btSkipClick;  

			bt_Finish = FindViewById<Button>(Resource.Id.bt_Finish);
			bt_Finish.Click += btFinalClick;  
		}

		public void btFinalClick(object sender, EventArgs e)
		{
			Intent Intent = new Intent (this, typeof(HomeActivity));

			var orderList = this.orderListAdapter.GetOrderList();

			List<OrderList1> Items = new List<OrderList1> ();

			foreach (var order in orderList) {
				OrderList1 item = new OrderList1 (order.Id, order.StockName, order.ShouldNumber, order.StockNumber, order.OrderNumber, order.Unit);
				Items.Add (item);
			}

			Intent.PutParcelableArrayListExtra("key", Items.ToArray());

			StartActivity (Intent);
		}

		public void btNextClick(object sender, EventArgs e)
		{
			if (this.selectedIndex != this.orderListAdapter.Count - 1) {

				//Get edittext 
				if(edit_Stock.Text != ""  && edit_Stock.Text != "SKIP" ){
					try{
						var stockNumber =  Int32.Parse(edit_Stock.Text);
						this.orderListAdapter.SetStockAtPosition (selectedIndex, stockNumber);
						this.orderListAdapter.SetSkipAtPosition (selectedIndex, false);
					}
					catch(Exception ew){
						Toast.MakeText (this, "Input not valid", ToastLength.Short).Show ();
						return;
					}
				}

				this.selectedIndex = this.selectedIndex + 1;
				this.orderListView.SetItemChecked (selectedIndex, true);
				this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;
				this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;


				if (this.orderListAdapter.GetItemAtPosition (selectedIndex).Skip) {
					this.edit_Stock.Text = "SKIP";
				}
				else if (this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber != 0) {
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString ();
				} else  {
					this.edit_Stock.Text = "";
				}

				this.edit_Stock.RequestFocus ();
				this.edit_Stock.SetSelection(this.edit_Stock.Text.Length);

				InputMethodManager imm = (InputMethodManager) GetSystemService(Context.InputMethodService);
				imm.ShowSoftInput(this.edit_Stock, ShowFlags.Implicit);

			}
			else 
			{
				InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow (this.edit_Stock.WindowToken, HideSoftInputFlags.NotAlways);
			}
		}

		public void btSkipClick(object sender, EventArgs e)
		{
			this.orderListAdapter.SetSkipAtPosition (selectedIndex, true);

			if (this.selectedIndex != this.orderListAdapter.Count - 1) {

				this.selectedIndex = this.selectedIndex + 1;
				this.orderListView.SetItemChecked (selectedIndex, true);

				this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;
				this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;
				if (this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber != 0) {
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString ();
				} else if (this.orderListAdapter.GetItemAtPosition (selectedIndex).Skip) {
					this.edit_Stock.Text = "SKIP";
				} else {
					this.edit_Stock.Text = "";
				}

				this.edit_Stock.RequestFocus ();
				this.edit_Stock.SetSelection (this.edit_Stock.Text.Length);

				InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.ShowSoftInput (this.edit_Stock, ShowFlags.Implicit);
			} 
			else 
			{
				InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow (this.edit_Stock.WindowToken, HideSoftInputFlags.NotAlways);

				this.edit_Stock.Text = "SKIP";
				this.edit_Stock.SetSelection (this.edit_Stock.Text.Length);

			}
		}

		public void btBackClick(object sender, EventArgs e)
		{
			if (this.selectedIndex != 0) {

				//Get edittext 
				if(edit_Stock.Text != ""  && edit_Stock.Text != "SKIP" ){
					try{
						var stockNumber =  Int32.Parse(edit_Stock.Text);
						this.orderListAdapter.SetStockAtPosition (selectedIndex, stockNumber);
						this.orderListAdapter.SetSkipAtPosition (selectedIndex, false);
					}
					catch(Exception ew){
						Toast.MakeText (this, "Input not valid", ToastLength.Short).Show ();
						return;
					}
				}

				this.selectedIndex = this.selectedIndex - 1;
				this.orderListView.SetItemChecked (selectedIndex, true);
				this.tv_StockName.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockName;
				this.tv_Unit.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).Unit;

				if (this.orderListAdapter.GetItemAtPosition (selectedIndex).Skip) {
					this.edit_Stock.Text = "SKIP";
				}
				else if (this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber != 0) {
					this.edit_Stock.Text = this.orderListAdapter.GetItemAtPosition (selectedIndex).StockNumber.ToString ();
				} else  {
					this.edit_Stock.Text = "";
				}

				this.edit_Stock.RequestFocus ();
				this.edit_Stock.SetSelection(this.edit_Stock.Text.Length);

				InputMethodManager imm = (InputMethodManager) GetSystemService(Context.InputMethodService);
				imm.ShowSoftInput(this.edit_Stock, ShowFlags.Implicit);

			}
			else 
			{
				InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow (this.edit_Stock.WindowToken, HideSoftInputFlags.NotAlways);
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

			orderListView.ItemClick += listView_ItemClick;

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


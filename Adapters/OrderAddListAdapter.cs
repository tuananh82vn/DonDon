using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Java.Lang;
using Object = Java.Lang.Object; 
using System.Linq;
using Android.Content;
using Android.Graphics;
using Com.Telerik.Widget.List;
using System.Collections;
using Android.Support.V7.Widget;

namespace DonDon
{

	public class OrderAddListAdapter : BaseAdapter<OrderList>, IFilterable
	{

		private List<OrderList> _originalData;
		private List<OrderList> _OrderList;

		public Filter Filter { get; private set; }

		Activity _activity;

		public OrderAddListAdapter (Activity activity, List<OrderList> data)
		{

			_activity = activity;
			_OrderList = data;
			Filter = new OrderFilter(this);

//			mAlternatingColors = new int[] { 0xF2F2F2, 0xC3C3C3 };
		}

		public override OrderList this[int position]
		{
			get { return _OrderList[position]; }
		} 

		public OrderList GetItemAtPosition(int position)
		{
			return _OrderList[position];
		}

		public List<OrderList> GetOrderList()
		{
			return _OrderList;
		}

		public void SetStockAtPosition(int position, int StockNumber)
		{
			_OrderList[position].StockNumber = StockNumber;
		}

		public void SetOrderAtPosition(int position, int OrderNumber)
		{
			_OrderList[position].OrderNumber = OrderNumber;
		}

		public void SetSkipAtPosition(int position , bool skip)
		{
			_OrderList[position].Skip = skip;
		}

		public override int Count 
		{
			get 
			{  
				if (_OrderList == null)
				{
					return 0;
				}
				return _OrderList.Count; 
			}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return long.Parse(_OrderList [position].StockId.ToString());
		}

		public string GetItemName (int position) {
			return _OrderList [position].StockName;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.OrderAddList, parent, false);

			var StockName = view.FindViewById<TextView> (Resource.Id.tv_StockName);
			StockName.Text = _OrderList [position].StockName;


    		return view;
		}

		private Color GetColorFromInteger(int color)
		{
			return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
		}

		private class OrderFilter : Filter
		{
			private readonly OrderAddListAdapter _adapter;
			public OrderFilter(OrderAddListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<OrderList>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._OrderList; 

				if (constraint == null) return returnObj;

				if (_adapter._originalData != null && _adapter._originalData.Any())
				{
					results.AddRange(_adapter._originalData.Where(t => t.StockName.ToLower().Contains(constraint.ToString().ToLower())));
				}

				// Nasty piece of .NET to Java wrapping, be careful with this!
				returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
				returnObj.Count = results.Count;

				constraint.Dispose();

				return returnObj;
			}

			protected override void PublishResults(ICharSequence constraint, FilterResults results)
			{
				using (var values = results.Values)
					_adapter._OrderList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<OrderList>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}


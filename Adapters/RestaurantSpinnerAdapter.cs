using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace DonDon
{
	public class RestaurantSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<RestaurantList> _RestaurantList;

		private readonly IList<View> _views = new List<View>();

		public RestaurantSpinnerAdapter(Activity context, List<RestaurantList> data)
		{
			_context = context;
			_RestaurantList = data;
		}

		public RestaurantList GetItemAtPosition(int position)
		{
			return _RestaurantList.ElementAt(position);
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int id)
		{
			return id;
		}

		public int getPositionById(int ProjectId){
			for (int i = 0; i < _RestaurantList.Count (); i++) {
				if (_RestaurantList.ElementAt (i).Id == ProjectId) {
					return i;
				}

			}
			return -1;
		}

		public override int Count
		{
			get
			{
				return _RestaurantList == null ? 0 : _RestaurantList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			RestaurantList item = _RestaurantList.ElementAt(position);

			var view = convertView ?? _context.LayoutInflater.Inflate (Resource.Layout.SpinnerItemDropdown, parent, false);

			var text = view.FindViewById<TextView>(Resource.Id.text);

			if (text != null)
				text.Text = item.RestaurantName;

			return view;
		}

		private void ClearViews()
		{
			foreach (var view in _views)
			{
				view.Dispose();
			}
			_views.Clear();
		}

		protected override void Dispose(bool disposing)
		{
			ClearViews();
			base.Dispose(disposing);
		}
	}
}


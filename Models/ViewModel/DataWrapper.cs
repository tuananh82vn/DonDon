using System;
using System.Collections.Generic;

namespace DonDon
{
	public class DataWrapper : Java.Lang.Object, Java.IO.ISerializable {

		private List<OrderList> OrderList;


		public DataWrapper(List<OrderList> data) {
			this.OrderList = data;
		}

		public List<OrderList> getOrderList() {
			return this.OrderList;
		}

	}
}


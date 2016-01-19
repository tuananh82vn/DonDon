using System;
using Android.OS;
using Java.Interop;

namespace DonDon
{
	public class OrderList 
	{
		public int Id { get ; set;	}

		public string StockName {get ; set;}

		public int ShouldNumber {get ; set;}

		public int StockNumber {get ; set;}

		public int OrderNumber {get ; set;}

		public string Unit { get ; set;}

		public bool Skip { get ; set;}
	}
}


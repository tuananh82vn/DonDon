using System;
using Android.OS;
using Java.Interop;

namespace DonDon
{
	public class OrderList 
	{
		public int StockId { get ; set;	}

		public string StockName {get ; set;}

		public double ShouldNumber {get ; set;}

		public double StockNumber {get ; set;}

		public double OrderNumber {get ; set;}

		public string Unit { get ; set;}

		public bool Skip { get ; set;}
	}
}


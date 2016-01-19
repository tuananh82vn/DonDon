using System;
using Android.App;
using Android.Views.InputMethods;

namespace DonDon
{
	public static class Utility
	{

		public static long SetMaxDate(int days)
		{
			DateTime _dt_now = DateTime.Now;
			DateTime _start = new DateTime (1970, 1, 1);
			TimeSpan ts = (_dt_now - _start);

			//Add Days to SetMax Days;
			int noOfDays  = ts.Days + days;
			return (long)(TimeSpan.FromDays(noOfDays).TotalMilliseconds);
		}

	}
}


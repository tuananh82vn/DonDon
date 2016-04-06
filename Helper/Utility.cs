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

		public static DateTime GetTodayDate()
		{
			string url = Settings.InstanceURL;

			url=url+"/api/GetTodayDate";

			var logon = new
			{
			};

			try {

				string results= ConnectWebAPI.Request(url,logon);

				//too tired today , sorry about this stupid code :(
				results = results.Remove(0,1);
				results = results.Remove(results.Length-1,1);

				return DateTime.Parse(results);


			} catch (Exception ex) {

				Console.WriteLine (ex.StackTrace);

				return DateTime.Today;
			}
		}


			
	}
}


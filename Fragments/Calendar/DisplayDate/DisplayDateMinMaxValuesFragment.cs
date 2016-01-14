﻿using Com.Telerik.Widget.Calendar;
using Android.Views;
using Android.OS;
using Java.Util;
using System;

namespace DonDon
{
	public class DisplayDateMinMaxValuesFragment : Android.Support.V4.App.Fragment, ExampleFragment
	{
		RadCalendarView calendarView;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			calendarView = new RadCalendarView (Activity);
			Calendar calendar = Java.Util.Calendar.Instance;
			calendar.Set (CalendarField.DayOfMonth, 5);
			calendarView.MinDate = calendar.TimeInMillis;
			calendar.Add (CalendarField.Month, 1);
			calendar.Set (CalendarField.DayOfMonth, 15);
			calendarView.MaxDate = calendar.TimeInMillis;

			return calendarView;
		}

		public String Title() {
			return "Display Date Min Max Values";
		}
	}
}
﻿using Com.Telerik.Widget.Calendar;
using Android.OS;
using Android.Views;
using System;

namespace DonDon
{
	public class InitCodeFragment : Android.Support.V4.App.Fragment, ExampleFragment
	{
		RadCalendarView calendarView;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			ViewGroup rootView = (ViewGroup)inflater.Inflate(Resource.Layout.fragment_calendar_example, container, false);

			calendarView = new RadCalendarView(this.Activity);
			rootView.AddView (calendarView);
			return rootView;
		}

		public String Title() {
			return "Init from code";
		}
	}
}
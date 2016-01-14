﻿using System;
using System.Collections.Generic;
using Android.Views;
using Android.OS;
using Com.Telerik.Widget.Calendar;
using Android.Content;
using Android.Widget;
using Com.Telerik.Widget.Calendar.Events.Read;
using Java.Util;
using Com.Telerik.Widget.Calendar.Events;
using Android.Graphics;

namespace DonDon
{
	public class EventsInlineDisplayModeFragment : Android.Support.V4.App.Fragment, ExampleFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			RadCalendarView calendarView = new RadCalendarView (Activity);

			// Setting the events display mode
			calendarView.SelectionMode = CalendarSelectionMode.Single;
			calendarView.EventsDisplayMode = EventsDisplayMode.Inline;

			// Creating some events
			List<Event> events = new List<Event>();
			Calendar calendar = Calendar.Instance;

			long start = calendar.TimeInMillis;
			calendar.Add(CalendarField.Hour, 1);
			long end = calendar.TimeInMillis;

			events.Add(new Event("Test1", start, end));

			calendar.Add(CalendarField.Date, 1);
			calendar.Add(CalendarField.Hour, -1);
			start = calendar.TimeInMillis;
			calendar.Add(CalendarField.Hour, 3);
			end = calendar.TimeInMillis;

			events.Add(new Event("Test2", start, end));

			calendarView.EventAdapter.Events = events;

			// Customization
			calendarView.Adapter.InlineEventsBackgroundColor = Color.Black;
			calendarView.Adapter.InlineEventTimeStartTextColor = Color.White;
			calendarView.Adapter.InlineEventTimeEndTextColor = Color.White;
			calendarView.Adapter.InlineEventTitleTextSize = 42.0F;

			return calendarView;
		}

		public String Title() {
			return "Inline display mode";
		}
	}
}
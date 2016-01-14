﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DonDon
{
	[Activity (Label = "ControlActivity")]			
	public class ControlActivity : Activity
	{
		private ExpandableListView expList;
		internal static ExamplesProvider currentProvider;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.SetContentView(Resource.Layout.activity_control);
			this.expList = (ExpandableListView)this.FindViewById (Resource.Id.expListView);
			this.Title = currentProvider.ControlName();
			ExamplesAdapter ea = new ExamplesAdapter(currentProvider.Examples());
			this.expList.SetAdapter(ea);
			this.expList.ChildClick += (object sender, ExpandableListView.ChildClickEventArgs e) => {
				ExampleActivity.selectedExampleFragment = (Android.Support.V4.App.Fragment)ea.GetChild(e.GroupPosition, e.ChildPosition);
				Intent exampleIntent = new Intent(this, typeof(ExampleActivity));
			this.StartActivity(exampleIntent);
				e.Handled = true;
			};
			// Create your application here
		}
	}
}


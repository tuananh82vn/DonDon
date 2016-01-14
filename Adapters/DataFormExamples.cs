﻿using System;
using Android.Widget;
using Java.Util;
using Android.Views;

namespace DonDon
{
	public class DataFormExamples : Java.Lang.Object, ExamplesProvider
	{
		private HashMap dataFormExamples;

		public DataFormExamples(){
			this.dataFormExamples = this.getDataFormExamples();
		}

		public String ControlName() {
			return "Data Form";
		}

		public HashMap Examples(){
			return this.dataFormExamples;
		}

		private HashMap getDataFormExamples(){
			HashMap dataFormExamples = new HashMap();
			ArrayList result = new ArrayList();

			result.Add (new DataFormGettingStartedFragment());
			result.Add (new DataFormFeaturesFragment ());
			result.Add (new DataFormEditorsFragment ());
			result.Add (new DataFormValidationFragment ());
			result.Add (new DataFormGroupLayoutFragment ());
			result.Add (new DataFormPlaceholderLayoutFragment ());
			result.Add (new DataFormValidationBehaviorFragment ());
			result.Add (new DataFormValidationModeFragment ());

			dataFormExamples.Put ("Init", result);

			return dataFormExamples;
		}
	}
}


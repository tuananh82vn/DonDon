using System;
using Java.Util;
using Com.Telerik.Android.Common;

namespace DonDon
{
	public class SideDrawerExamples : Java.Lang.Object, ExamplesProvider {

		private HashMap sideDrawerExamples;

		public SideDrawerExamples(){
			this.sideDrawerExamples = this.getSideDrawerExamples();
		}

		public String ControlName() {
			return "Side Drawer";
		}

		public HashMap Examples(){
			return this.sideDrawerExamples;
		}

		private HashMap getSideDrawerExamples(){
			HashMap sideDrawerExamples = new HashMap();
			ArrayList result = new ArrayList();

			result.Add (new DrawerInitialSetupFragment());

			sideDrawerExamples.Put ("Init", result);

			return sideDrawerExamples;
		}
	}
}


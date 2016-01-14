using System;
using System.Collections.Generic;

namespace DonDon
{
	public static class RestaurantController
	{
		public static List<RestaurantList> GetRestaurantList(){

			string url = Settings.InstanceURL;

			url=url+"/Api/GetRestaurantList";


			var objsearch = (new
				{
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<RestaurantList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<RestaurantList>>> (results);

				List<RestaurantList> returnObject = new List<RestaurantList> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						RestaurantList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<RestaurantList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;

				return returnObject;
			} else
				return null;
		}
	}
}


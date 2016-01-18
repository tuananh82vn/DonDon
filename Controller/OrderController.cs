using System;
using System.Collections.Generic;

namespace DonDon
{
	public static class OrderController
	{
		public static List<OrderList> GetOrderList(DateTime orderDate){

			string url = Settings.InstanceURL;

			url=url+"/Api/GetOrderList";


			var objsearch = (new
			{
				OrderDate = orderDate,
				RestaurantId = Settings.RestaurantId

			});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<OrderList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<OrderList>>> (results);

				List<OrderList> returnObject = new List<OrderList> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						OrderList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderList> (Item.ToString ());
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


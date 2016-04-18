using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;

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

		public static string GetOrderNotes(DateTime orderDate){

			string url = Settings.InstanceURL;

			url=url+"/Api/GetOrderNotes";


			var objsearch = (new
				{
					OrderDate = orderDate,
					RestaurantId = Settings.RestaurantId

				});

			try {

				string results= ConnectWebAPI.Request(url,objsearch);



				return results;

			} catch (Exception ex) {

				Console.WriteLine (ex.StackTrace);

				return null;
			}
		}

		public static async System.Threading.Tasks.Task<ApiResultSave> SendOrderList(List<OrderList> obj, string notes, int orderOption)
		{
			ApiResultSave apiResultSave = new ApiResultSave();

			apiResultSave = await new WebApiHelper().EditAddObject("/API/SendOrderList",obj, notes,orderOption);

			return apiResultSave;
		}


	}
}


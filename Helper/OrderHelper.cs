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
	public sealed class WebApiHelper
	{
		private HttpClient httpClient;

		public WebApiHelper()
		{
			httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(Settings.InstanceURL);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.MaxResponseContentBufferSize = 52428800;
			httpClient.Timeout = TimeSpan.FromMinutes(5);
		}

		public async System.Threading.Tasks.Task<ApiResultSave> EditAddObject(string url,Object objTask)
		{
			ApiResultSave apiResult = new ApiResultSave();

			try
			{
				ApiSave<Object> objApiTask = new ApiSave<Object>();

				objApiTask.UserId = Settings.UserId;

				objApiTask.RestaurantId = Settings.RestaurantId;

				objApiTask.OrderDate = Utility.GetTodayDate();

				objApiTask.Item = objTask;

				var jsonString = JsonConvert.SerializeObject(objApiTask);                

				string contentstring = GetJsonConentString(jsonString);

				var content = new StringContent(contentstring, Encoding.UTF8, "application/x-www-form-urlencoded");

				var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

				if (response.IsSuccessStatusCode)
				{

					var dataContent = await response.Content.ReadAsStringAsync();

					apiResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultSave>(dataContent);

				}
			}
			catch (Exception)
			{
				throw;
			}

			return apiResult;
		}

		public string GetJsonConentString(string jsonString)
		{

			jsonString = jsonString.Replace("T00:00:00+05:30", string.Empty);

			String value = jsonString;
			int limit = 32000;

			StringBuilder sb = new StringBuilder();
			int loops = value.Length / limit;

			for (int i = 0; i <= loops; i++)
			{
				if (i < loops)
				{
					sb.Append(System.Uri.EscapeDataString(value.Substring(limit * i, limit)));
				}
				else
				{
					sb.Append(System.Uri.EscapeDataString(value.Substring(limit * i)));
				}
			}

			jsonString = sb.ToString();

			string content = string.Format("content={0}", jsonString);

			return content;

		}
	}
}


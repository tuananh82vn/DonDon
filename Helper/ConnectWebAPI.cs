using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Text;
using Android.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DonDon
{
	public static class ConnectWebAPI
	{
		public static string Request(string url, object json  )
		{
			// Get Response
			HttpWebResponse response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
				request.Method = "POST";
				request.Accept = "application/json";
				request.ContentType = "application/json";
				request.KeepAlive = false;


				string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(json);


				byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

				request.ContentLength = requestBytes.Length;


				Stream requestStream = request.GetRequestStream();
				requestStream.Write(requestBytes, 0, requestBytes.Length);
				requestStream.Close();

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException wex)
			{
				// for timeouts etc
				if (response == null)
					return null;

				// try and get the error text
				response = (HttpWebResponse)wex.Response;
				StreamReader sr = null;
				sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII);
				string _wexBody = sr.ReadToEnd();
				sr.Close();
				return _wexBody;
			}
			catch
			{
				return "";
			}
			//Return Response

			string responseText = string.Empty;

			WebHeaderCollection header = response.Headers;

			var encoding = ASCIIEncoding.ASCII;
			using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
			{
				responseText = reader.ReadToEnd();
			}
			return responseText;
		}
	}
}


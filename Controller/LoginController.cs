using System;

namespace DonDon
{
	public class LoginController
	{
		public LoginObject Login(string username,string password)
		{
			LoginObject obj = new LoginObject ();

			string url = Settings.InstanceURL;

			url=url+"/api/logon";

			var logon = new
			{
				Item = new
				{
					UserName = username,
					Password = password,
				}
			};

			try {

				string results= ConnectWebAPI.Request(url,logon);

				obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginObject> (results);

				return obj;

			} catch (Exception ex) {

				Console.WriteLine (ex.StackTrace);

				return null;
			}
		}
	}
}


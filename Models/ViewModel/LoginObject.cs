using System;

namespace DonDon
{
	public class LoginObject
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public bool Success { get; set; }
		public string ErrorMessage { get; set; }

	}
}


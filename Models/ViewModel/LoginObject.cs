using System;

namespace DonDon
{
	public class LoginObject
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public bool CKStaff { get; set; }

		public bool Success { get; set; }
		public string ErrorMessage { get; set; }

	}
}


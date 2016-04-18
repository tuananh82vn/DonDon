using System;

namespace DonDon
{
	public class ApiSave<T>
	{
		public int UserId { get; set; }

		public int RestaurantId { get; set; }

		public string Notes { get; set; }

		public int OrderOption { get; set; }

		public DateTime OrderDate { get; set; }

		public T Item { get; set; }
	}
}


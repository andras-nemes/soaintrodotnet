using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Service.Responses
{
	public class ProductReservationResponse : ServiceResponseBase
	{
		public string ReservationId { get; set; }
		public DateTime Expiration { get; set; }
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		public int ProductQuantity { get; set; }
	}
}

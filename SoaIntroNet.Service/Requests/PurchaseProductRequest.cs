using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Service.Requests
{
	public class PurchaseProductRequest
	{
		public string CorrelationId { get; set; }
		public string ReservationId { get; set; }
		public string ProductId { get; set; }
	}
}

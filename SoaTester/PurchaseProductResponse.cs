using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoaTester
{
	public class PurchaseProductResponse : ServiceResponseBase
	{
		public string PurchaseId { get; set; }
		public string ProductName { get; set; }
		public string ProductId { get; set; }
		public int ProductQuantity { get; set; }
	}
}

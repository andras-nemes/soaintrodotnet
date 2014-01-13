using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Service.Requests
{
	public class ReserveProductRequest
	{
		public string ProductId { get; set; }
		public int ProductQuantity { get; set; }
	}
}

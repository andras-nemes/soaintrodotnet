using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoaIntroNet.Service.Requests;
using SoaIntroNet.Service.Responses;

namespace SoaIntroNet.Service
{
	public interface IProductService
	{
		ProductReservationResponse ReserveProduct(ReserveProductRequest productReservationRequest);
		PurchaseProductResponse PurchaseProduct(PurchaseProductRequest productPurchaseRequest);
	}
}

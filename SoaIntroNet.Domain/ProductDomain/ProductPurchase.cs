using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Domain.ProductDomain
{
	public class ProductPurchase
	{
		public ProductPurchase(Product product, int quantity)
		{
			Id = Guid.NewGuid();
			if (product == null) throw new ArgumentNullException("Product cannot be null.");
			if (quantity < 1) throw new ArgumentException("The quantity should be at least 1.");
			Product = product;
			ProductQuantity = quantity;
		}

		public Guid Id { get; set; }
		public Product Product { get; set; }
		public int ProductQuantity { get; set; }
	}
}

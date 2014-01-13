using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Domain.ProductDomain
{
	public class ProductReservation
	{
		public ProductReservation(Product product, int expiryInMinutes, int quantity)
		{
			if (product == null) throw new ArgumentNullException("Product cannot be null.");
			if (quantity < 1) throw new ArgumentException("The quantity should be at least 1.");
			Product = product;
			Id = Guid.NewGuid();
			Expiry = DateTime.Now.AddMinutes(expiryInMinutes);
			Quantity = quantity;
		}

		public Guid Id { get; set; }
		public Product Product { get; set; }
		public DateTime Expiry { get; set; }
		public int Quantity { get; set; }
		public bool HasBeenConfirmed { get; set; }

		public bool Expired()
		{
			return DateTime.Now > Expiry;
		}

		public bool IsActive()
		{
			return !HasBeenConfirmed && !Expired();
		}
	}
}

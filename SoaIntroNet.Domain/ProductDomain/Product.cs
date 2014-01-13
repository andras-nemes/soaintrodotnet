using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Domain.ProductDomain
{
	public class Product
	{
		public Product()
		{
			ReservedProducts = new List<ProductReservation>();
			PurchasedProducts = new List<ProductPurchase>();
		}

		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Allocation { get; set; }
		public List<ProductReservation> ReservedProducts { get; set; }
		public List<ProductPurchase> PurchasedProducts { get; set; }

		public int Available()
		{
			int soldAndReserved = 0;
			PurchasedProducts.ForEach(p => soldAndReserved += p.ProductQuantity);
			ReservedProducts.FindAll(p => p.IsActive()).ForEach(p => soldAndReserved += p.Quantity);

			return Allocation - soldAndReserved;
		}

		public bool ReservationIdValid(Guid reservationId)
		{
			if (HasReservation(reservationId))
			{
				return GetReservationWith(reservationId).IsActive();
			}
			return false;
		}

		public ProductPurchase ConfirmPurchaseWith(Guid reservationId)
		{
			if (!ReservationIdValid(reservationId))
			{
				throw new Exception(string.Format("Cannot confirm the purchase with this Id: {0}", reservationId));
			}

			ProductReservation reservation = GetReservationWith(reservationId);
			ProductPurchase purchase = new ProductPurchase(this, reservation.Quantity);
			reservation.HasBeenConfirmed = true;
			PurchasedProducts.Add(purchase);
			return purchase;
		}

		public ProductReservation GetReservationWith(Guid reservationId)
		{
			if (!HasReservation(reservationId))
			{
				throw new Exception(string.Concat("No reservation found with id {0}", reservationId.ToString()));
			}
			return (from r in ReservedProducts where r.Id == reservationId select r).FirstOrDefault();
		}

		private bool HasReservation(Guid reservationId)
		{
			return ReservedProducts.Exists(p => p.Id == reservationId);
		}

		public bool CanReserveProduct(int quantity)
		{
			return Available() >= quantity;
		}

		public ProductReservation Reserve(int quantity)
		{
			if (!CanReserveProduct(quantity))
			{
				throw new Exception("Can not reserve this many tickets.");
			}

			ProductReservation reservation = new ProductReservation(this, 1, quantity);
			ReservedProducts.Add(reservation);
			return reservation;
		}
	}
}

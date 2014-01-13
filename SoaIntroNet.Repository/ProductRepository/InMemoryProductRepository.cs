using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoaIntroNet.Domain.ProductDomain;

namespace SoaIntroNet.Repository.ProductRepository
{
	public class InMemoryProductRepository : IProductRepository
	{
		private int standardReservationTimeoutMinutes = 1;
		public List<Product> DatabaseProducts { get; set; }
		public List<ProductPurchase> DatabaseProductPurchases { get; set; }
		public List<ProductReservation> DatabaseProductReservations { get; set; }

		public InMemoryProductRepository()
		{
			InitialiseDatabase();
		}

		private void InitialiseDatabase()
		{
			DatabaseProducts = new List<Product>();
			Product firstProduct = new Product()
			{
				Allocation = 200,
				Description = "Product A",
				ID = Guid.Parse("13a35876-ccf1-468a-88b1-0acc04422243"),
				Name = "A"
			};
			Product secondProduct = new Product()
			{
				Allocation = 500,
				Description = "Product B",
				ID = Guid.Parse("f5efdfe0-7933-4efc-a290-03d20014703e"),
				Name = "B"
			};
			DatabaseProducts.Add(firstProduct);
			DatabaseProducts.Add(secondProduct);

			DatabaseProductPurchases = new List<ProductPurchase>();
			DatabaseProductPurchases.Add(new ProductPurchase(firstProduct, 10) { Id = Guid.Parse("0ede40e0-5a52-48b1-8578-de1891c5a7f0") });
			DatabaseProductPurchases.Add(new ProductPurchase(firstProduct, 20) { Id = Guid.Parse("5868144e-e04d-4c1f-81d7-fc671bfc52dd") });
			DatabaseProductPurchases.Add(new ProductPurchase(secondProduct, 12) { Id = Guid.Parse("8e6195ac-d448-4e28-9064-b3b1b792895e") });
			DatabaseProductPurchases.Add(new ProductPurchase(secondProduct, 32) { Id = Guid.Parse("f66844e5-594b-44b8-a0ef-2a2064ec2f43") });
			DatabaseProductPurchases.Add(new ProductPurchase(secondProduct, 1) { Id = Guid.Parse("0e73c8b3-f7fa-455d-ba7f-7d3f1bc2e469") });
			DatabaseProductPurchases.Add(new ProductPurchase(secondProduct, 4) { Id = Guid.Parse("e28a3cb5-1d3e-40a1-be7e-e0fa12b0c763") });

			DatabaseProductReservations = new List<ProductReservation>();
			DatabaseProductReservations.Add(new ProductReservation(firstProduct, standardReservationTimeoutMinutes, 10) { HasBeenConfirmed = true, Id = Guid.Parse("a2c2a6db-763c-4492-9974-62ab192201fe") });
			DatabaseProductReservations.Add(new ProductReservation(firstProduct, standardReservationTimeoutMinutes, 5) { HasBeenConfirmed = false, Id = Guid.Parse("37f2e5ac-bbe0-48b0-a3cd-9c0b47842fa1") });
			DatabaseProductReservations.Add(new ProductReservation(firstProduct, standardReservationTimeoutMinutes, 13) { HasBeenConfirmed = true, Id = Guid.Parse("b9393ea4-6257-4dea-a8cb-b78a0c040255") });
			DatabaseProductReservations.Add(new ProductReservation(firstProduct, standardReservationTimeoutMinutes, 3) { HasBeenConfirmed = false, Id = Guid.Parse("a70ef898-5da9-4ac1-953c-a6420d37b295") });
			DatabaseProductReservations.Add(new ProductReservation(secondProduct, standardReservationTimeoutMinutes, 17) { Id = Guid.Parse("85eaebfa-4be4-407b-87cc-9a9ea46d547b") });
			DatabaseProductReservations.Add(new ProductReservation(secondProduct, standardReservationTimeoutMinutes, 3) { Id = Guid.Parse("39d4278e-5643-4c43-841c-214c1c3892b0") });
			DatabaseProductReservations.Add(new ProductReservation(secondProduct, standardReservationTimeoutMinutes, 9) { Id = Guid.Parse("86fff675-e5e3-4e0e-bcce-36332c4de165") });

			firstProduct.PurchasedProducts = (from p in DatabaseProductPurchases where p.Product.ID == firstProduct.ID select p).ToList();
			firstProduct.ReservedProducts = (from p in DatabaseProductReservations where p.Product.ID == firstProduct.ID select p).ToList();

			secondProduct.PurchasedProducts = (from p in DatabaseProductPurchases where p.Product.ID == secondProduct.ID select p).ToList();
			secondProduct.ReservedProducts = (from p in DatabaseProductReservations where p.Product.ID == secondProduct.ID select p).ToList();
		}

		public Product FindBy(Guid productId)
		{

			return (from p in DatabaseProducts where p.ID == productId select p).FirstOrDefault();
		}

		public void Save(Product product)
		{
			ClearPurchasedAndReservedProducts(product);
			InsertPurchasedProducts(product);
			InsertReservedProducts(product);
		}

		private void ClearPurchasedAndReservedProducts(Product product)
		{
			DatabaseProductPurchases.RemoveAll(p => p.Id == product.ID);
			DatabaseProductReservations.RemoveAll(p => p.Id == product.ID);
		}

		private void InsertReservedProducts(Product product)
		{
			DatabaseProductReservations.AddRange(product.ReservedProducts);
		}

		private void InsertPurchasedProducts(Product product)
		{
			DatabaseProductPurchases.AddRange(product.PurchasedProducts);
		}

		public static InMemoryProductRepository Instance
		{
			get
			{
				return Nested.instance;
			}
		}

		private class Nested
		{
			static Nested()
			{
			}
			internal static readonly InMemoryProductRepository instance = new InMemoryProductRepository();
		}
	}
}

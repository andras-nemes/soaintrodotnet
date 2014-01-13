using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoaIntroNet.Domain.ProductDomain;
using SoaIntroNet.Repository.MessagingHistory;
using SoaIntroNet.Repository.ProductRepository;
using SoaIntroNet.Service.Exceptions;
using SoaIntroNet.Service.Requests;
using SoaIntroNet.Service.Responses;

namespace SoaIntroNet.Service
{
	public class ProductService : IProductService
	{
		private readonly IMessageRepositoryFactory _messageRepositoryFactory;
		private readonly IProductRepositoryFactory _productRepositoryFactory;
		private readonly IMessageRepository _messageRepository;
		private readonly IProductRepository _productRepository;

		public ProductService(IMessageRepositoryFactory messageRepositoryFactory, IProductRepositoryFactory productRepositoryFactory)
		{
			if (messageRepositoryFactory == null) throw new ArgumentNullException("MessageRepositoryFactory");
			if (productRepositoryFactory == null) throw new ArgumentNullException("ProductRepositoryFactory");
			_messageRepositoryFactory = messageRepositoryFactory;
			_productRepositoryFactory = productRepositoryFactory;
			_messageRepository = _messageRepositoryFactory.Create();
			_productRepository = _productRepositoryFactory.Create();
		}

		public ProductReservationResponse ReserveProduct(ReserveProductRequest productReservationRequest)
		{
			ProductReservationResponse reserveProductResponse = new ProductReservationResponse();
			try
			{
				Product product = _productRepository.FindBy(Guid.Parse(productReservationRequest.ProductId));
				if (product != null)
				{
					ProductReservation productReservation = null;
					if (product.CanReserveProduct(productReservationRequest.ProductQuantity))
					{
						productReservation = product.Reserve(productReservationRequest.ProductQuantity);
						_productRepository.Save(product);
						reserveProductResponse.ProductId = productReservation.Product.ID.ToString();
						reserveProductResponse.Expiration = productReservation.Expiry;
						reserveProductResponse.ProductName = productReservation.Product.Name;
						reserveProductResponse.ProductQuantity = productReservation.Quantity;
						reserveProductResponse.ReservationId = productReservation.Id.ToString();
					}
					else
					{
						int availableAllocation = product.Available();
						reserveProductResponse.Exception = new LimitedAvailabilityException(string.Concat("There are only ", availableAllocation,
							" pieces of this product left."));
					}
				}
				else
				{
					throw new ResourceNotFoundException(string.Concat("No product with id ", productReservationRequest.ProductId, ", was found."));
				}
			}
			catch (Exception ex)
			{
				reserveProductResponse.Exception = ex;
			}
			return reserveProductResponse;
		}

		public PurchaseProductResponse PurchaseProduct(PurchaseProductRequest productPurchaseRequest)
		{
			PurchaseProductResponse purchaseProductResponse = new PurchaseProductResponse();
			try
			{
				if (_messageRepository.IsUniqueRequest(productPurchaseRequest.CorrelationId))
				{					
					Product product = _productRepository.FindBy(Guid.Parse(productPurchaseRequest.ProductId));
					if (product != null)
					{
						ProductPurchase productPurchase = null;
						if (product.ReservationIdValid(Guid.Parse(productPurchaseRequest.ReservationId)))
						{
							productPurchase = product.ConfirmPurchaseWith(Guid.Parse(productPurchaseRequest.ReservationId));
							_productRepository.Save(product);
							purchaseProductResponse.ProductId = productPurchase.Product.ID.ToString();
							purchaseProductResponse.PurchaseId = productPurchase.Id.ToString();
							purchaseProductResponse.ProductQuantity = productPurchase.ProductQuantity;
							purchaseProductResponse.ProductName = productPurchase.Product.Name;
						}
						else
						{
							throw new ResourceNotFoundException(string.Concat("Invalid or expired reservation id: ", productPurchaseRequest.ReservationId));
						}
						_messageRepository.SaveResponse<PurchaseProductResponse>(productPurchaseRequest.CorrelationId, purchaseProductResponse);
					}
					else
					{
						throw new ResourceNotFoundException(string.Concat("No product with id ", productPurchaseRequest.ProductId, ", was found."));
					}
				}
				else
				{
					purchaseProductResponse = _messageRepository.RetrieveResponseFor<PurchaseProductResponse>(productPurchaseRequest.CorrelationId);
				}
			}
			catch (Exception ex)
			{
				purchaseProductResponse.Exception = ex;
			}

			return purchaseProductResponse;
		}
	}
}

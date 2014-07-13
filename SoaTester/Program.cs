using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SoaTester
{
	class Program
	{
		private static Uri _productReservationServiceUri = new Uri("http://localhost:49679/reservation");
		private static Uri _productPurchaseServiceUri = new Uri("http://localhost:49679/purchase");

		static void Main(string[] args)
		{
			ReserveProductRequest reservationRequest = new ReserveProductRequest();
			reservationRequest.ProductId = "13a35876-ccf1-468a-88b1-0acc04422243";
			reservationRequest.ProductQuantity = 10;
			ProductReservationResponse reservationResponse = ReserveProduct(reservationRequest);

			Console.WriteLine("Reservation response received.");
			Console.WriteLine(string.Concat("Reservation success: ", (reservationResponse.Exception == null)));
			if (reservationResponse.Exception == null)
			{
				Console.WriteLine("Reservation id: " + reservationResponse.ReservationId);
				PurchaseProductRequest purchaseRequest = new PurchaseProductRequest();
				purchaseRequest.ProductId = reservationResponse.ProductId;
				purchaseRequest.ReservationId = reservationResponse.ReservationId;
				PurchaseProductResponse purchaseResponse = PurchaseProduct(purchaseRequest);
				if (purchaseResponse.Exception == null)
				{
					Console.WriteLine("Purchase confirmation id: " + purchaseResponse.PurchaseId);
				}
				else
				{
					Console.WriteLine(purchaseResponse.Exception.Message);
				}
			}
			else
			{
				Console.WriteLine(reservationResponse.Exception.Message);
			}

			Console.ReadKey();
		}

		private static PurchaseProductResponse PurchaseProduct(PurchaseProductRequest request)
		{
			PurchaseProductResponse response = new PurchaseProductResponse();
			try
			{
				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _productPurchaseServiceUri);
				requestMessage.Headers.ExpectContinue = false;
				String jsonArguments = JsonConvert.SerializeObject(request);
				requestMessage.Content = new StringContent(jsonArguments, Encoding.UTF8, "application/json");
				HttpClient httpClient = new HttpClient();
				httpClient.Timeout = new TimeSpan(0, 10, 0);
				Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage,
					HttpCompletionOption.ResponseContentRead, CancellationToken.None);
				HttpResponseMessage httpResponse = httpRequest.Result;
				HttpStatusCode statusCode = httpResponse.StatusCode;
				HttpContent responseContent = httpResponse.Content;
				Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
				String stringContents = stringContentsTask.Result;
				if (statusCode == HttpStatusCode.OK && responseContent != null)
				{
					response = JsonConvert.DeserializeObject<PurchaseProductResponse>(stringContents);
				}
				else
				{
					response.Exception = new Exception(stringContents);
				}
			}
			catch (Exception ex)
			{
				response.Exception = ex;
			}
			return response;
		}

		private static ProductReservationResponse ReserveProduct(ReserveProductRequest request)
		{
			ProductReservationResponse response = new ProductReservationResponse();
			try
			{
				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _productReservationServiceUri);
				requestMessage.Headers.ExpectContinue = false;
				String jsonArguments = JsonConvert.SerializeObject(request);
				requestMessage.Content = new StringContent(jsonArguments, Encoding.UTF8, "application/json");
				HttpClient httpClient = new HttpClient();
				httpClient.Timeout = new TimeSpan(0, 10, 0);
				Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage,
					HttpCompletionOption.ResponseContentRead, CancellationToken.None);
				HttpResponseMessage httpResponse = httpRequest.Result;
				HttpStatusCode statusCode = httpResponse.StatusCode;
				HttpContent responseContent = httpResponse.Content;
				Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
				String stringContents = stringContentsTask.Result;
				if (statusCode == HttpStatusCode.OK && responseContent != null)
				{					
					response = JsonConvert.DeserializeObject<ProductReservationResponse>(stringContents);
				}
				else
				{
					response.Exception = new Exception(stringContents);
				}
			}
			catch (Exception ex)
			{
				response.Exception = ex;
			}
			return response;
		}
	}
}

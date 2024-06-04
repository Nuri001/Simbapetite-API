using Azure;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.Domain.RepositoryContracts;
using Simbapetite.Core.DTO;
using Simbapetite.Core.Helpers;
using Simbapetite.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;

		public OrderService(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<OrderHeader> CreatOrder(OrderHeaderCreateDTO orderHeaderDTO)
		{
			OrderHeader order = new()
			{
				ApplicationUserId = orderHeaderDTO.ApplicationUserId,
				PickupEmail = orderHeaderDTO.PickupEmail,
				PickupName = orderHeaderDTO.PickupName,
				PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
				OrderTotal = orderHeaderDTO.OrderTotal,
				OrderDate = DateTime.Now,
				StripePaymentIntentID = orderHeaderDTO.StripePaymentIntentID,
				TotalItems = orderHeaderDTO.TotalItems,
				Status = String.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.status_pending : orderHeaderDTO.Status,
			};
			await _orderRepository.AddOrderHeader(order);
			foreach (var orderDetailDTO in orderHeaderDTO.OrderDetailsDTO)
			{
				OrderDetails orderDetails = new()
				{
					OrderHeaderId = order.OrderHeaderId,
					ItemName = orderDetailDTO.ItemName,
					MenuItemId = orderDetailDTO.MenuItemId,
					Price = orderDetailDTO.Price,
					Quantity = orderDetailDTO.Quantity,
				};
			await	_orderRepository.AddOrderDetails(orderDetails);
			}
			return order;
		}

		public async Task<List<OrderHeader>> GetAllOrderHeaders()
		{
			return await _orderRepository.GetAllOrderHeaders();
		}

		public async Task<List<OrderHeader>?> GetOrders(int id)
		{
			return await _orderRepository.GetOrders(id);
		}

		public async Task<OrderHeader?> UpdateOrderHeader(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO)
		{
			OrderHeader orderFromDb =await _orderRepository.GetOrderHeader(id);

			if (orderFromDb == null) return null;
		
			if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupName))
			{
				orderFromDb.PickupName = orderHeaderUpdateDTO.PickupName;
			}
			if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupPhoneNumber))
			{
				orderFromDb.PickupPhoneNumber = orderHeaderUpdateDTO.PickupPhoneNumber;
			}
			if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupEmail))
			{
				orderFromDb.PickupEmail = orderHeaderUpdateDTO.PickupEmail;
			}
			if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.Status))
			{
				orderFromDb.Status = orderHeaderUpdateDTO.Status;
			}
			if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.StripePaymentIntentID))
			{
				orderFromDb.StripePaymentIntentID = orderHeaderUpdateDTO.StripePaymentIntentID;
			}

			return await _orderRepository.UpdadeOrderHeader(orderFromDb);
		}
	}
}

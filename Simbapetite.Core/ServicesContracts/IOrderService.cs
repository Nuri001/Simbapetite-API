using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.ServicesContracts
{
	public interface IOrderService
	{
		/// <summary>
		/// get all ordersheaders
		/// </summary>
		/// <returns>List<OrderHeader></returns>
		Task<List<OrderHeader>> GetAllOrderHeaders();


		/// <summary>
		/// get orders by OrderHeaderId
		/// </summary>
		/// <param name="id"></param>
		/// <returns>List<OrderHeader></returns>
		Task<List<OrderHeader>> GetOrders(int id);


		/// <summary>
		/// Creat ne Order
		/// </summary>
		/// <param name="orderHeaderDTO"></param>
		/// <returns>OrderHeader</returns>
		Task<OrderHeader> CreatOrder(OrderHeaderCreateDTO orderHeaderDTO);

		/// <summary>	
		/// UpdateOrderHeader
		/// </summary>
		/// <param name="id"></param>
		/// <param name="orderHeaderUpdateDTO"></param>
		/// <returns>OrderHeader</returns>
		Task<OrderHeader> UpdateOrderHeader(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO);


	}
}

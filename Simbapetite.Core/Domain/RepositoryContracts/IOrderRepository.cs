using Simbapetite.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.Domain.RepositoryContracts
{
	public interface IOrderRepository
	{
		/// <summary>
		/// get all ordersheaders
		/// </summary>
		/// <returns></returns>
		Task<List<OrderHeader>> GetAllOrderHeaders();

		/// <summary>
		/// get all orders by OrderHeaderId
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<OrderHeader>> GetOrders(int id);

		/// <summary>
		/// Get OrderHeader by OrderHeader Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>OrderHeader</returns>
		Task<OrderHeader> GetOrderHeader(int id);
		/// <summary>
		/// Add orderHeader to DB
		/// </summary>
		/// <param name="orderHeader"></param>
		/// <returns></returns>
		Task<OrderHeader> AddOrderHeader(OrderHeader orderHeader);

		/// <summary>
		/// Add OrderDetails to DB
		/// </summary>
		/// <param name="orderDetails"></param>
		/// <returns></returns>
		Task<OrderDetails> AddOrderDetails(OrderDetails orderDetails);

		/// <summary>
		/// Update OrderHeader
		/// </summary>
		/// <param name="orderHeader"></param>
		/// <returns></returns>
		Task<OrderHeader> UpdadeOrderHeader(OrderHeader orderHeader);
	}
}

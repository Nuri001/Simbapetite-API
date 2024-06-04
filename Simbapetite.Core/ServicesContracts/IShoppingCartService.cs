using Simbapetite.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.ServicesContracts
{
	public interface IShoppingCartService
	{
		/// <summary>
		/// return ShopingCart include menuItems by ShopingCart ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>ShopingCart</returns>
		Task<ShoppingCart> GetShoppingCart(string userId);

		/// <summary>
		/// Add Or Update Item In Shoping Cart
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="menuItemId"></param>
		/// <param name="updateQuantityBy"></param>
		/// <returns>true/false</returns>
		Task<bool> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy);
	}
}

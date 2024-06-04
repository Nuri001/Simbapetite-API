using Simbapetite.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.Domain.RepositoryContracts
{
	/// <summary>
	/// Represents data access logic for managing ShoppingCart entity
	/// </summary>
	public interface IShoppingCartRepository
	{
		/// <summary>
		/// return shoping cart by user id
		/// </summary>
		/// <param name=""></param>
		/// <returns>Shping cart</returns>
		Task<ShoppingCart> GetShoppingCart(string userId);

		/// <summary>
		/// add new shoping cart to db
		/// </summary>
		/// <param name="shoppingCart"></param>
		/// <returns>new shoping cart</returns>
		Task<ShoppingCart> AddShoppingCart(ShoppingCart shoppingCart);

		/// <summary>
		/// add new cart item to db
		/// </summary>
		/// <param name="cartItem"></param>
		/// <returns></returns>
		Task<CartItem> AddCartItem(CartItem cartItem);

		/// <summary>
		/// update cartItem Quantity
		/// </summary>
		/// <param name="cartItemy"></param>
		/// <returns></returns>
		Task<bool> UpdateCartItem(CartItem cartItemy);

		/// <summary>
		/// Remove cart item from cart and if it is the only item then remove Shoping cart
		/// </summary>
		/// <param name="shoppingCart"></param>
		/// <param name="cartItem"></param>
		/// <returns></returns>
		Task RemoveCartItem(ShoppingCart shoppingCart, CartItem cartItem);

	


	}
}

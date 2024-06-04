using Azure;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.Domain.RepositoryContracts;
using Simbapetite.Core.ServicesContracts;
using Simbapetite_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.Services
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly IMenuItemRepository _menuItemRepository;

	

		//constructor
		public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IMenuItemRepository menuItemRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_menuItemRepository = menuItemRepository;
	
		}

		public async Task<bool> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy)
		{
			ShoppingCart shoppingCart =await  _shoppingCartRepository.GetShoppingCart(userId);
			MenuItem menuItem = await _menuItemRepository.GetMenuItem(menuItemId);
			if (menuItem == null) return false;
			if (shoppingCart == null && updateQuantityBy > 0)
			{
				//create a shopping cart & add cart item

				ShoppingCart newCart = await _shoppingCartRepository.AddShoppingCart(new() { UserId = userId });

				CartItem newCartItem = await _shoppingCartRepository.AddCartItem(new()
				{
					MenuItemId = menuItemId,
					Quantity = updateQuantityBy,
					ShoppingCartId = newCart.Id,
					MenuItem = null
				});
			}
			else
			{
				//shopping cart exists

				CartItem cartItemInCart = shoppingCart.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);
				if (cartItemInCart == null)
				{
					//item does not exist in current cart
					CartItem newCartItem = await _shoppingCartRepository.AddCartItem(new()
					{
						MenuItemId = menuItemId,
						Quantity = updateQuantityBy,
						ShoppingCartId = shoppingCart.Id,
						MenuItem = null
					});

				}
				else
				{
					//item already exist in the cart and we have to update quantity
					int newQuantity = cartItemInCart.Quantity + updateQuantityBy;
					if (updateQuantityBy == 0 || newQuantity <= 0)
					{
						//remove cart item from cart and if it is the only item then remove cart
						await _shoppingCartRepository.RemoveCartItem(shoppingCart, cartItemInCart);
					}
					else
					{
						cartItemInCart.Quantity = newQuantity;
						bool success = await _shoppingCartRepository.UpdateCartItem(cartItemInCart);
						if (!success) return false;
					}

				}


				
			}
			return true;
		}

		public async Task<ShoppingCart> GetShoppingCart(string userId)
		{
			ShoppingCart shoppingCart;
			if (string.IsNullOrEmpty(userId))
			{
				shoppingCart = new();
			}
			else
			{
				shoppingCart = await _shoppingCartRepository.GetShoppingCart(userId);
				if(shoppingCart == null)
				{
					shoppingCart = new();
				}

			}
			if (shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
			{
				shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
			}

			return shoppingCart;
		}
	}
}

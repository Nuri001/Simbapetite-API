using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.ServicesContracts
{
	public interface IMenuItemService
	{
		/// <summary>
		/// return all menuItems in the db
		/// </summary>
		/// <returns>all menuItems</returns>
		Task<List<MenuItem>> GetAllMenuItems();

		/// <summary>
		/// return menuItem by menuItem ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>menuItem</returns>
		Task<MenuItem> GetMenuItem(int id);

		/// <summary>
		/// Creat a new MenuItem and add it to the DB
		/// </summary>
		/// <param name="menuItemCreateDTO"></param>
		/// <returns>new MenuItem</returns>
		Task<MenuItem> CreateMenuItem(MenuItemCreateDTO menuItemCreateDTO);

		/// <summary>
		/// Delete a MenuItem By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>tru/far for deleted or not</returns>
		Task<Boolean> DeleteMenuItem(int id);

		/// <summary>
		/// Update a MenuItem
		/// </summary>
		/// <param name="id"></param>
		/// <param name="menuItemUpdateDTO"></param>
		/// <returns>updated MenuItem</returns>
		Task<MenuItem> UpdateMenuItem(int id, MenuItemUpdateDTO menuItemUpdateDTO);

	}
}

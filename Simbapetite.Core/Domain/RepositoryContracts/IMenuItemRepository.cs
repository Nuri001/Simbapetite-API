using Simbapetite.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.Domain.RepositoryContracts
{
	/// <summary>
	/// Represents data access logic for managing MenuItem entity
	/// </summary>
	public interface IMenuItemRepository
	{

		Task<MenuItem> AddMenuItem(MenuItem menuItem);
		Task<List<MenuItem>> GetAllMenuItems();

		Task<MenuItem> GetMenuItem(int id);

		Task<bool> DeleteMenuItemByID(int id);

		Task<MenuItem> UpdateMenuItem(MenuItem menuItem);

	}
}

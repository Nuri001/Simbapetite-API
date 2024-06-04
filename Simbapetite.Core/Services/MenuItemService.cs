using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.Domain.RepositoryContracts;
using Simbapetite.Core.DTO;
using Simbapetite.Core.Helpers;
using Simbapetite.Core.ServicesContracts;
using Simbapetite_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbapetite.Core.Services
{
	public class MenuItemService : IMenuItemService
	{
	
		private readonly IMenuItemRepository _menuItemRepository;
		private readonly IBlobService _blobService;

		//constructor
		public MenuItemService(IMenuItemRepository menuItemRepository, IBlobService blobService)
		{
			_menuItemRepository = menuItemRepository;
			_blobService = blobService;
		}
		

		public async Task<MenuItem> CreateMenuItem(MenuItemCreateDTO menuItemCreateDTO)
		{
			try {
				string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDTO.File.FileName)}";
				MenuItem menuItemToCreate = new()
				{
					Name = menuItemCreateDTO.Name,
					Price = menuItemCreateDTO.Price,
					Category = menuItemCreateDTO.Category,
					SpecialTag = menuItemCreateDTO.SpecialTag,
					Description = menuItemCreateDTO.Description,
					Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, menuItemCreateDTO.File)
				};

				return await _menuItemRepository.AddMenuItem(menuItemToCreate);

			}
			catch (Exception ex)
			{
				throw new Exception("Exception in CreateMenuItem: "+ex.Message);               
			}
			
		}

		public async Task<bool> DeleteMenuItem(int id)
		{
			if (id == 0) { return false; }
			MenuItem menuItemFromDb = await GetMenuItem(id);
			if (menuItemFromDb == null) { return false; }
			try
			{
				await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
			}
			catch {
				throw new Exception("Error in _blobService.DeleteBlob");
			}
			
			return await _menuItemRepository.DeleteMenuItemByID(menuItemFromDb.Id);

		}

		public async Task<List<MenuItem>> GetAllMenuItems()
		{
			return await _menuItemRepository.GetAllMenuItems();
			
		}

		public async Task<MenuItem> GetMenuItem(int id)
		{
			return await _menuItemRepository.GetMenuItem(id);
		}

		public async Task<MenuItem> UpdateMenuItem(int id, MenuItemUpdateDTO menuItemUpdateDTO)
		{
			MenuItem menuItemFromDb=await GetMenuItem(id);
			if(menuItemFromDb==null) { return null; }
			menuItemFromDb.Name = menuItemUpdateDTO.Name;
			menuItemFromDb.Price = menuItemUpdateDTO.Price;
			menuItemFromDb.Category = menuItemUpdateDTO.Category;
			menuItemFromDb.SpecialTag = menuItemUpdateDTO.SpecialTag;
			menuItemFromDb.Description = menuItemUpdateDTO.Description;

			
			if (menuItemUpdateDTO.File != null && menuItemUpdateDTO.File.Length > 0)
			{
				try
				{
					string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDTO.File.FileName)}";
					await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
					menuItemFromDb.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, menuItemUpdateDTO.File);

				}
				catch {
					throw new Exception("Error in image file");
				}
				
			}
			MenuItem updatedMenuItem=await _menuItemRepository.UpdateMenuItem(menuItemFromDb);
			return updatedMenuItem;


		}

		
	}
}

using Simbapetite.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;
using FluentAssertions;
using Moq;
using Simbapetite.Core.Domain.RepositoryContracts;
using Simbapetite.Core.Services;
using EntityFrameworkCoreMock;
using Simbapetite.Core.Domain.Entities;
using System.Diagnostics.Metrics;
using Simbapetite_API.Services;

namespace Simbapetite.ServiceTests
{
	public class MenuItemServiceTest
	{	
		private readonly IMenuItemService _menuItemService;
		private readonly Mock<IMenuItemRepository> _menuItemRepositoryMock;
		private readonly IMenuItemRepository _menuItemRepository;
		private readonly Mock<IBlobService> _blobServiceMock;
		private readonly IBlobService _blobService;
		private readonly IFixture _fixture;
		public MenuItemServiceTest()
		{
			_fixture = new Fixture();

			_menuItemRepositoryMock = new Mock<IMenuItemRepository>();
			_menuItemRepository = _menuItemRepositoryMock.Object;
			_blobServiceMock= new Mock<IBlobService>();
			_blobService = _blobServiceMock.Object;

			_menuItemService = new MenuItemService(_menuItemRepository,_blobService);
		


		}

		#region GetAllMenuItems
		[Fact]
		public async Task GetAllMenuItems_EmptyList()
		{
			//Arrange
			
			List<MenuItem> menuItems_empty_list = new List<MenuItem>();
			_menuItemRepositoryMock.Setup(temp => temp.GetAllMenuItems()).ReturnsAsync(menuItems_empty_list);
			//Act
			List<MenuItem> actual_menuItems_list = await _menuItemService.GetAllMenuItems();
			//Assert
			Assert.Empty(actual_menuItems_list);
		}
		[Fact]
		public async Task GetAllMenuItems_ShouldHaveFewMenuItems()
		{
			//Arrange
			List<MenuItem> menuItems_list = new List<MenuItem>() {
				_fixture.Build<MenuItem>().Create(),
			_fixture.Build<MenuItem>().Create(),
				_fixture.Build<MenuItem>().Create(),
			};

			_menuItemRepositoryMock.Setup(temp => temp.GetAllMenuItems()).ReturnsAsync(menuItems_list);


			//Act
			List<MenuItem> actual_menuItems_list = await _menuItemRepository.GetAllMenuItems();

			//Assert
			actual_menuItems_list.Should().BeEquivalentTo(menuItems_list);
		}
		#endregion




	}
}

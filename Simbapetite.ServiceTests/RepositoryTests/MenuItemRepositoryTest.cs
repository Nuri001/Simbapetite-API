using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Simbapetite.Core.Domain.Entities;
using Simbapetite.Core.Domain.RepositoryContracts;
using Simbapetite.Infrastructure.DbContext;
using Simbapetite.Infrastructure.Repositories;
using AutoFixture;
using Xunit;
using FluentAssertions;
using System.Diagnostics.Metrics;


namespace Simbapetite.ServiceTests.RepositoryTests
{
	public class MenuItemRepositoryTest
	{
		private readonly IMenuItemRepository _menuItemRepository;
		private readonly IFixture _fixture;
		private readonly DbContextMock<ApplicationDbContext> _dbContextMock;
		public MenuItemRepositoryTest()
		{
			_fixture = new Fixture();
			var menuItemsInitialData = new List<MenuItem>() { };
			_dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

			ApplicationDbContext dbContext = _dbContextMock.Object;
			_dbContextMock.CreateDbSetMock(temp => temp.MenuItems, menuItemsInitialData);

			_menuItemRepository = new MenuItemRepository(dbContext);

		}

		#region GetAllMenuItems
		[Fact]
		//The list of MenuItems should be empty by default (before adding any countries)
		public async Task GetAllMenuItems_EmptyList()
		{
			//Act
			List<MenuItem> actual_menuItems_list = await _menuItemRepository.GetAllMenuItems();

			//Assert
			Assert.Empty(actual_menuItems_list);
		}
		[Fact]
		public async Task GetAllMenuItems_ShouldHaveFewMenuItems()
		{
			//Arrange
			List<MenuItem> menuItem_list = new List<MenuItem>() {
				_fixture.Build<MenuItem>().Create(),
			_fixture.Build<MenuItem>().Create(),
			_fixture.Build<MenuItem>().Create(),
			};

			_dbContextMock.CreateDbSetMock(temp => temp.MenuItems, menuItem_list);


			//Act
			List<MenuItem> actual_menuItems_list = await _menuItemRepository.GetAllMenuItems();

			//Assert
			actual_menuItems_list.Should().BeEquivalentTo(menuItem_list);
		}
		#endregion

		#region AddMenuItem
		//When MenuIten is null, it should throw ArgumentNullException
		[Fact]
		public async Task AddCountry_NullMenuItem_ToBeArgumentNullException()
		{
			//Arrange
			MenuItem? menuItem = null;


			//Act
			var action = async () =>
			{
				await _menuItemRepository.AddMenuItem(menuItem);
			};

			//Assert
			await action.Should().ThrowAsync<ArgumentNullException>();
		}

		[Fact]
		public async Task AddMenuItem_FullMenuItem_ToBeSuccessful()
		{
			//Arrange
			MenuItem menuItem = _fixture.Create<MenuItem>();

			//Act
			MenuItem menuItem_from_add_menuItem = await _menuItemRepository.AddMenuItem(menuItem);

			//Assert
			menuItem_from_add_menuItem.Should().NotBeNull();

		}
		#endregion
		#region DeleteMenuItemByMenuItemID
		[Fact]
		public async Task DeleteMenuItem_ValidMenuItemID_ToBeSuccessful()
		{
			//Arrange
			MenuItem menuItem = _fixture.Build<MenuItem>().Create();

			List<MenuItem> menuItem_list = new List<MenuItem>() {
				menuItem 
			};

			_dbContextMock.CreateDbSetMock(temp => temp.MenuItems, menuItem_list);


			//Act
			bool isDeleted = await _menuItemRepository.DeleteMenuItemByID(menuItem.Id);

			//Assert
			isDeleted.Should().BeTrue();
		}

		[Fact]
		public async Task DeleteMenuItem_InvalidMenuItemID()
		{
			//Act
			bool isDeleted = await _menuItemRepository.DeleteMenuItemByID(-1);

			//Assert
			isDeleted.Should().BeFalse();
		}

		#endregion

		#region GetMenuItem
		[Fact]
		public async Task GetMenuItem_InvalidMenuItemID_ToBeNull()
		{
			//Arrange
			int id = -1;

			//Act
			MenuItem? menuItem_from_get = await _menuItemRepository.GetMenuItem(id);

			//Assert
			menuItem_from_get.Should().BeNull();
		}


		[Fact]
		public async Task GetMenuItem_WithMenuItemID_ToBeSuccessful()
		{
			//Arange

			MenuItem menuItem = _fixture.Build<MenuItem>().Create();

			List<MenuItem> menuItem_list = new List<MenuItem>() {
				menuItem
			};

			_dbContextMock.CreateDbSetMock(temp => temp.MenuItems, menuItem_list);

			//Act
			MenuItem? menuItem_from_get = await _menuItemRepository.GetMenuItem(menuItem.Id);

			//Assert
			menuItem_from_get.Should().NotBeNull();
		}
		#endregion
		[Fact]


		#region UpdateMenuIteme
		public async Task UpdateMenuIteme_ToBeSuccessful()
		{
			//Arange
			MenuItem menuItem = _fixture.Build<MenuItem>().Create();

			List<MenuItem> menuItem_list = new List<MenuItem>() {
				menuItem
			};

			_dbContextMock.CreateDbSetMock(temp => temp.MenuItems, menuItem_list);

			//Act
			menuItem.Name = "updatedName";
			MenuItem? menuItem_from_update = await _menuItemRepository.UpdateMenuItem(menuItem);

			//Assert
			menuItem_from_update.Should().Be(menuItem);
		}
		#endregion

	}
}

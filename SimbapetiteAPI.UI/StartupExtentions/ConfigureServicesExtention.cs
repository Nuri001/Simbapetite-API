using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Simbapetite.Core.ServicesContracts;
using Simbapetite.Core.Services;
using Simbapetite.Core.Domain.RepositoryContracts;
using Simbapetite.Infrastructure.DbContext;
using Simbapetite.Infrastructure.Repositories;
using Azure.Storage.Blobs;
using Simbapetite_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Simbapetite.Core.Domain.IdentityEntities;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Simbapetite.UI.StartupExtentions
{
	public static class ConfigureServicesExtention
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			

			

			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultDbConnection"));
			});



			//add services into IoC container
			services.AddSingleton(u => new BlobServiceClient(configuration.GetConnectionString("StorageAccount")));
			services.AddSingleton<IBlobService, BlobService>();
			services.AddScoped<IMenuItemRepository, MenuItemRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IShoppingCartService, ShoppingCartService>();
			services.AddScoped<IMenuItemRepository, MenuItemRepository>();

			services.AddScoped<IMenuItemService, MenuItemService>();


			services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 1;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
			});
			var key = configuration.GetValue<string>("ApiSettings:Secret");
			services.AddAuthentication(u =>
			{
				u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(u =>
			{
				u.RequireHttpsMetadata = false;
				u.SaveToken = true;
				u.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});



			services.AddCors();
			services.AddControllers();

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Description =
						"JWT Authorization header using the Bearer scheme. \r\n\r\n " +
						"Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
						"Example: \"Bearer 12345abcdef\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Scheme = JwtBearerDefaults.AuthenticationScheme
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
		   new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
			});

			return services;

		}
	}
}



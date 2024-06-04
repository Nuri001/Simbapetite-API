using Simbapetite.UI.StartupExtentions;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
	
	app.UseSwaggerUI();
}
else
{
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
		c.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();
app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Text.Json.Serialization;
using app;
using app.Data;
using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration["Data:DefaultConnection"];
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddJsonApi<AppDbContext>(options =>
{
    options.UseRelativeLinks = true;
    options.IncludeTotalResourceCount = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    Seeder.EnsureSampleData(scope.ServiceProvider.GetRequiredService<AppDbContext>());
}

app.UseRouting();
app.UseJsonApi();
app.MapControllers();

app.Run();

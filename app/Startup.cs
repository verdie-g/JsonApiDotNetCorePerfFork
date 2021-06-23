using app.Data;
using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace app
{
    public class Startup
    {
        private readonly string _connectionString;

        public Startup(IConfiguration config)
        {
            _connectionString = config["Data:DefaultConnection"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_connectionString));

            services.AddJsonApi<AppDbContext>(options =>
            {
                options.UseRelativeLinks = true;
                options.ValidateModelState = true;
                options.IncludeTotalResourceCount = true;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, AppDbContext dbContext)
        {
            Seeder.EnsureSampleData(dbContext);

            app.UseRouting();
            app.UseJsonApi();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}

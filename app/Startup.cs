using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Data;
using JsonApiDotNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace app
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // add the db context like you normally would
            services.AddDbContext<AppDbContext>(options =>
            { // use whatever provider you want, this is just an example
                options.UseNpgsql(_config["Data:DefaultConnection"]);
            }, ServiceLifetime.Scoped);

            // add jsonapi dotnet core
            services.AddJsonApi<AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, AppDbContext context)
        {
            context.Database.EnsureCreated();
            app.UseJsonApi();
        }
    }
}

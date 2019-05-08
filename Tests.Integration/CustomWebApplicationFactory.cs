using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Api.Startup>
    {
        public Data.SqliteContext DbContext
        {
            get
            {
                return Server.Host.Services.CreateScope().ServiceProvider.GetService<Data.SqliteContext>();
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Add a database context using an in-memory database for testing.
                services.AddDbContext<Data.SqliteContext>(options =>
                {
                    options.UseInMemoryDatabase("Test");
                });
            });
        }
    }
}

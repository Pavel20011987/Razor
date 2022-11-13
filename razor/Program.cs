using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using razor.Models;
using razor.Data;
using Microsoft.EntityFrameworkCore;

namespace razor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*Исходник запуска*/    
            //CreateHostBuilder(args).Build().Run();

            var host = CreateHostBuilder(args).Build();            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                    var config = host.Services.GetRequiredService<IConfiguration>();
                    SeedData.InitializeRole(services);
                    SeedData.InitializeMask(services);
                    SeedData.InitializeVlan(services);   
                    SeedData.InitializeVendor(services);   
                    var testUserPw = config["SeedUserPW"];
                    SeedData.InitializeUser(services, testUserPw).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

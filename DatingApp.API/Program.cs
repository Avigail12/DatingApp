using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {            
            var host = CreateHostBuilder(args).Build();
            using (var scope=host.Services.CreateScope())
            {
                var services=scope.ServiceProvider;
                try
                {
                    var contex=services.GetRequiredService<DataContext>();
                    await contex.Database.MigrateAsync();
                    Seed.SeedUsers(contex);
                }
                catch (Exception ex)
                {
                    var logger=services.GetRequiredService<ILogger<Program>>();
                   logger.LogError(ex, "An error occur douring migration");
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

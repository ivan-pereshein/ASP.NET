using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.DataAccess.Data;


namespace PromoCodeFactory.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EfContext>();
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                // db.Roles.AddRange(FakeDataFactory.Roles);                
                db.Employees.AddRange(FakeDataFactory.Employees);
                db.Customers.AddRange(FakeDataFactory.Customers);
                db.Preferences.AddRange(FakeDataFactory.Preferences);
                
                db.SaveChanges();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
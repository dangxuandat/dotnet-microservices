using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

namespace Platform_Service;

public static class PrepDatabase
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProductEnvironment)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProductEnvironment);
        }
    }

    private static void SeedData(AppDbContext context, bool isProductEnvironment)
    {
        if (isProductEnvironment)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> could not run migrations: {e.Message}");
                throw;
            }
        }
        else
        {
            
        }
        if (!context.Platforms.Any())
        {
            Console.WriteLine("Seeding data");
            context.Platforms.AddRange(new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new Platform() { Name = "Java", Publisher = "Oracle", Cost = "Free"},
                new Platform() { Name = "Python", Publisher = "C++", Cost = "Free"});
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("->>> We already have data");
        }
    }
}
using PlatformService.Data;
using PlatformService.Models;

namespace Platform_Service;

public static class PrepDatabase
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }
    }

    private static void SeedData(AppDbContext context)
    {
        if (!context.Platforms.Any())
        {
            Console.WriteLine("Seeding data");
            context.Platforms.AddRange(new Platform() { Id = 1, Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new Platform() { Id = 2, Name = "Java", Publisher = "Oracle", Cost = "Free"},
                new Platform() { Id = 3, Name = "Python", Publisher = "C++", Cost = "Free"});
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("->>> We already have data");
        }
    }
}
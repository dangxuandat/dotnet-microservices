using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }

    public DbSet<Platform> Platforms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Platform>().HasData(
            new Platform() { Id = 1, Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
            new Platform() { Id = 2, Name = "Java", Publisher = "Oracle", Cost = "Free"},
            new Platform() { Id = 3, Name = "Python", Publisher = "C++", Cost = "Free"}
            );
    }
}
using Microsoft.EntityFrameworkCore;
using Wpm.Managment.Api.Models;

namespace Wpm.Managment.Api.DataAccess;

public class ManagmentDbContext(DbContextOptions<ManagmentDbContext> options) : DbContext(options)
{
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Breed> Breeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var breed1Id = Guid.NewGuid();
        var breed2Id = Guid.NewGuid();
        var breed3Id = Guid.NewGuid();
        
        modelBuilder.Entity<Breed>().HasData(
        [
            new (breed1Id, "Dogo Argentino"),
            new (breed2Id,"Shar Pei"),
            new (breed3Id,"Jack Rusell")
        ]);

        modelBuilder.Entity<Pet>().HasData([
            new Pet() { Id = Guid.NewGuid(), Name = "Chicho", Age = 5, BreedId = breed1Id },
            new Pet() { Id = Guid.NewGuid(), Name = "Carlitos", Age = 8, BreedId = breed2Id },
            new Pet() { Id = Guid.NewGuid(), Name = "Adrian", Age = 10, BreedId = breed3Id },
            new Pet() { Id = Guid.NewGuid(), Name = "Pablo", Age = 4, BreedId = breed2Id },
        ]);
    }
}
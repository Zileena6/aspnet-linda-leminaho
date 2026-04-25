using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreFitness.Infrastructure;

public class CoreFitnessDbContextFactory : IDesignTimeDbContextFactory<CoreFitnessDbContext>
{
    public CoreFitnessDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CoreFitnessDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CoreFitness;Trusted_Connection=True;")
            .Options;

        return new CoreFitnessDbContext(options);
    }
}
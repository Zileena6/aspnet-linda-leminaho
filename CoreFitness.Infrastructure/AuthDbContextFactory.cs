using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreFitness.Infrastructure;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CoreFitnessAuth;Trusted_Connection=True;")
            .Options;

        return new AuthDbContext(options);
    }
}
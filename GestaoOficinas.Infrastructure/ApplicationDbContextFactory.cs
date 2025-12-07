using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using GestaoOficinas.Infrastructure.Persistence; // Ajuste este namespace, se necessário

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=GestaoOficinasDb;Username=postgres;Password=R@i210613");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
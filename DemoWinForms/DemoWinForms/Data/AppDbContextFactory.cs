using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DemoWinForms.Data;

/// <summary>
/// Factory para crear DbContext en tiempo de diseño (migrations)
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Usar una ruta temporal para las migraciones
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tareas.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
        
      return new AppDbContext(optionsBuilder.Options);
    }
}

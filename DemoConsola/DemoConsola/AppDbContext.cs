using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Linq;

namespace DemoConsola
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Start from the app base directory (usually bin/Debug/netX) and walk up
            // until a .csproj file is found — that directory is assumed to be the project root.
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (dir != null && !dir.EnumerateFiles("*.csproj").Any())
            {
                dir = dir.Parent;
            }

            string projectDir = dir?.FullName ?? AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = Path.GetFullPath(Path.Combine(projectDir, "usuarios.db"));

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}

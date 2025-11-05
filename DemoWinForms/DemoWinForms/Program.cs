using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using DemoWinForms.Data;
using DemoWinForms.Data.Repositories;
using DemoWinForms.Business.Services;
using DemoWinForms.Presentation.Forms;

namespace DemoWinForms;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Configurar Serilog
        Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Information()
        .WriteTo.File("logs/app-.txt", rollingInterval: Serilog.RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Iniciando aplicación Gestor de Tareas");

            // Cargar configuración
            var configuration = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

            // Configurar Dependency Injection
            var services = new ServiceCollection();

            // DbContext con SQLite
            var dbPath = GetDatabasePath();
            services.AddDbContext<AppDbContext>(options =>
     options.UseSqlite($"Data Source={dbPath}"));

            // Repositorios
   services.AddScoped<ITareaRepository, TareaRepository>();
            services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();

   // Servicios
    services.AddScoped<ITareaService, TareaService>();

 // Logging
  services.AddLogging(loggingBuilder =>
     {
      loggingBuilder.ClearProviders();
             loggingBuilder.AddSerilog(dispose: true);
        });

        // Formularios
            services.AddTransient<FormPrincipal>();
     services.AddTransient<FormTarea>();

            // Configuración
      services.AddSingleton<IConfiguration>(configuration);

            ServiceProvider = services.BuildServiceProvider();

   // Aplicar migraciones automáticamente
   using (var scope = ServiceProvider.CreateScope())
            {
         var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
     context.Database.Migrate();
          Log.Information("Base de datos inicializada correctamente");
        }

        // Iniciar aplicación WinForms
     ApplicationConfiguration.Initialize();

            var formPrincipal = ServiceProvider.GetRequiredService<FormPrincipal>();
            Application.Run(formPrincipal);

    Log.Information("Aplicación cerrada correctamente");
        }
        catch (Exception ex)
        {
Log.Fatal(ex, "La aplicación falló al iniciar");
         MessageBox.Show($"Error crítico al iniciar la aplicación:\n\n{ex.Message}", 
 "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
        finally
        {
        Log.CloseAndFlush();
    }
    }

    private static string GetDatabasePath()
    {
    // Buscar directorio del proyecto
        var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (dir != null && !dir.EnumerateFiles("*.csproj").Any())
        {
            dir = dir.Parent;
        }

        var projectDir = dir?.FullName ?? AppDomain.CurrentDomain.BaseDirectory;
      var dbPath = Path.Combine(projectDir, "tareas.db");

 Log.Information("Ruta de la base de datos: {DbPath}", dbPath);
        return dbPath;
    }
}
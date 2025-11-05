using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DemoWinForms.Data;
using DemoWinForms.Data.Repositories;
using DemoWinForms.Business.Services;
using DemoWpf.ViewModels;
using DemoWpf.Views;
using DemoWpf.Services;

namespace DemoWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Configurar DbContext
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite("Data Source=tareas.db"));

                    // Registrar repositorios
                    services.AddScoped<ITareaRepository, TareaRepository>();
                    services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();

                    // Registrar servicios
                    services.AddScoped<ITareaService, TareaService>();
                    services.AddSingleton<IExportService, ExportService>();

                    // Registrar ViewModels
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<TareaListViewModel>();
                    services.AddTransient<TareaEditViewModel>();
                    services.AddSingleton<ThemeViewModel>();
                    services.AddTransient<EtiquetaManagerViewModel>();

                    // Registrar ventanas
                    services.AddTransient<MainWindow>();
                    services.AddTransient<TareaListWindow>();
                    services.AddTransient<TareaEditWindow>();
                    services.AddTransient<EtiquetaManagerWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Asegurar que la base de datos existe
            using (var scope = _host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}

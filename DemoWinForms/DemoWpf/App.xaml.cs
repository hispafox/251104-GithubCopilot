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

                    // Registrar repositorios existentes
                    services.AddScoped<ITareaRepository, TareaRepository>();
                    services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();

                    // ========== Nuevos repositorios para sistema de listas de tareas ==========
                    services.AddScoped<ITaskRepository, TaskRepository>();

                    // Registrar servicios existentes
                    services.AddScoped<ITareaService, TareaService>();
                    services.AddSingleton<IExportService, ExportService>();

                    // ========== Nuevo servicio para sistema de listas de tareas ==========
                    services.AddScoped<ITaskService, TaskService>();

                    // Registrar ViewModels existentes
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<TareaListViewModel>();
                    services.AddTransient<TareaEditViewModel>();
                    services.AddSingleton<ThemeViewModel>();
                    services.AddTransient<EtiquetaManagerViewModel>();

                    // ========== Nuevos ViewModels para sistema de listas de tareas ==========
                    services.AddTransient<TaskListManagementViewModel>();
                    services.AddTransient<TaskListDialogViewModel>();
                    services.AddTransient<TaskItemDialogViewModel>();
                    services.AddTransient<TaskStatisticsViewModel>();
                    services.AddTransient<MoveTaskDialogViewModel>();

                    // Registrar ventanas existentes
                    services.AddTransient<MainWindow>();
                    services.AddTransient<TareaListWindow>();
                    services.AddTransient<TareaEditWindow>();
                    services.AddTransient<EtiquetaManagerWindow>();

                    // ========== Nuevas ventanas para sistema de listas de tareas ==========
                    services.AddTransient<TaskListDialog>();
                    services.AddTransient<TaskItemDialog>();
                    services.AddTransient<MoveTaskDialog>();
                    services.AddTransient<TaskListManagementView>();
                    services.AddTransient<TaskListDetailView>();
                    services.AddTransient<TaskStatisticsView>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Asegurar que la base de datos existe y aplicar migraciones
            using (var scope = _host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                // Usar MigrateAsync en lugar de EnsureCreatedAsync para soporte de migraciones
                await dbContext.Database.MigrateAsync();
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

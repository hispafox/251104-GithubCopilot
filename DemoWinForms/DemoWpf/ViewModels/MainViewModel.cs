using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using Microsoft.Extensions.Logging;

namespace DemoWpf.ViewModels;

/// <summary>
/// ViewModel principal de la aplicación con navegación
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<MainViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    [ObservableProperty]
    private string _currentViewTitle = "Dashboard";

    [ObservableProperty]
    private bool _isMenuOpen = true;

    // Instancias de ViewModels para navegación
    private TaskListManagementViewModel? _taskListManagementViewModel;
    private TaskStatisticsViewModel? _statisticsViewModel;

    public MainViewModel(
      ITaskService taskService,
        ILogger<MainViewModel> logger,
        IServiceProvider serviceProvider)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
  _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        // Iniciar en vista de estadísticas
  NavigateToStatistics();
    }

    /// <summary>
    /// Navega a la vista de gestión de listas
    /// </summary>
    [RelayCommand]
    private void NavigateToTaskLists()
    {
        try
        {
    if (_taskListManagementViewModel == null)
     {
              _taskListManagementViewModel = _serviceProvider.GetService(typeof(TaskListManagementViewModel)) 
           as TaskListManagementViewModel;
     }

        CurrentViewModel = _taskListManagementViewModel;
   CurrentViewTitle = "Mis Listas de Tareas";
            
            // Cargar listas al navegar
            _taskListManagementViewModel?.LoadTaskListsCommand.Execute(null);

    _logger.LogInformation("Navegado a vista de listas de tareas");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al navegar a listas de tareas");
        }
    }

    /// <summary>
    /// Navega a la vista de estadísticas
    /// </summary>
    [RelayCommand]
    private void NavigateToStatistics()
    {
        try
        {
            if (_statisticsViewModel == null)
            {
    _statisticsViewModel = _serviceProvider.GetService(typeof(TaskStatisticsViewModel)) 
as TaskStatisticsViewModel;
     }

            CurrentViewModel = _statisticsViewModel;
            CurrentViewTitle = "Dashboard de Estadísticas";
   
         // Cargar estadísticas al navegar
        _statisticsViewModel?.LoadStatisticsCommand.Execute(null);

  _logger.LogInformation("Navegado a vista de estadísticas");
        }
        catch (Exception ex)
        {
       _logger.LogError(ex, "Error al navegar a estadísticas");
      }
    }

 /// <summary>
    /// Alterna el menú lateral
  /// </summary>
    [RelayCommand]
    private void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
  }

    /// <summary>
    /// Refresca la vista actual
    /// </summary>
    [RelayCommand]
    private void RefreshCurrentView()
    {
        try
        {
         if (CurrentViewModel is TaskListManagementViewModel taskListVm)
     {
    taskListVm.LoadTaskListsCommand.Execute(null);
       }
       else if (CurrentViewModel is TaskStatisticsViewModel statsVm)
   {
     statsVm.LoadStatisticsCommand.Execute(null);
      }

            _logger.LogInformation("Vista actual refrescada");
        }
     catch (Exception ex)
        {
     _logger.LogError(ex, "Error al refrescar vista actual");
        }
    }
}

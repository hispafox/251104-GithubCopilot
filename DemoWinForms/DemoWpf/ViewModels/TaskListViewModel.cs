using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;

namespace DemoWpf.ViewModels;

/// <summary>
/// ViewModel para una lista de tareas individual
/// </summary>
public partial class TaskListViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger _logger;
    private List<TaskItem> _allTasks = new(); // Cache de todas las tareas

    /// <summary>
    /// Modelo de dominio subyacente
    /// </summary>
    public TaskList Model { get; }

    [ObservableProperty]
    private ObservableCollection<TaskItemViewModel> _tasks = new();

    [ObservableProperty]
    private TaskItemViewModel? _selectedTask;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _filterPending = true;

    [ObservableProperty]
    private bool _filterInProgress = true;

    [ObservableProperty]
    private bool _filterCompleted;

    [ObservableProperty]
    private TaskListPriority? _selectedPriority;

    [ObservableProperty]
    private string _sortBy = "Fecha"; // Fecha, Prioridad, Estado, Titulo

    [ObservableProperty]
    private bool _showOverdueOnly;

    // Propiedades que se actualizan del modelo
    public string Name => Model.Name;
    public string? Description => Model.Description;
    public string? ColorCode => Model.ColorCode;
    public int TotalTasks => Model.TotalTasks;
    public int CompletedTasksCount => Model.CompletedTasksCount;
    public int PendingTasksCount => Model.PendingTasksCount;
    public double ProgressPercentage => Model.ProgressPercentage;

    public TaskListViewModel(
        TaskList model, 
        ITaskService taskService,
     ILogger logger)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Cargar tareas iniciales
   LoadTasksFromModel();
   
    // Suscribirse a cambios en filtros
        PropertyChanged += (s, e) =>
        {
         if (e.PropertyName == nameof(SearchText) ||
            e.PropertyName == nameof(FilterPending) ||
           e.PropertyName == nameof(FilterInProgress) ||
                e.PropertyName == nameof(FilterCompleted) ||
       e.PropertyName == nameof(SelectedPriority) ||
        e.PropertyName == nameof(SortBy) ||
     e.PropertyName == nameof(ShowOverdueOnly))
            {
            ApplyFiltersAndSort();
}
        };
    }

    /// <summary>
    /// Carga las tareas desde el modelo
    /// </summary>
  private void LoadTasksFromModel()
    {
        _allTasks = Model.Tasks.ToList();
        ApplyFiltersAndSort();
    }

    /// <summary>
    /// Recarga las tareas desde la base de datos
    /// </summary>
    [RelayCommand]
    public async Task LoadTasksAsync()
    {
        try
      {
            var result = await _taskService.GetTaskItemsByListIdAsync(Model.Id);

       if (result.IsSuccess)
    {
         _allTasks = result.Value!.ToList();
                ApplyFiltersAndSort();

                // Actualizar propiedades calculadas
     OnPropertyChanged(nameof(TotalTasks));
  OnPropertyChanged(nameof(CompletedTasksCount));
    OnPropertyChanged(nameof(PendingTasksCount));
                OnPropertyChanged(nameof(ProgressPercentage));

    _logger.LogInformation("Tareas cargadas para lista {ListId}: {Count}", 
  Model.Id, _allTasks.Count);
            }
            else
            {
                _logger.LogWarning("Error al cargar tareas: {Error}", result.Error);
 MessageBox.Show(result.Error, "Error", 
      MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
{
        _logger.LogError(ex, "Excepción al cargar tareas");
    MessageBox.Show($"Error al cargar tareas: {ex.Message}", "Error",
         MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Aplica filtros y ordenamiento a las tareas
    /// </summary>
    private void ApplyFiltersAndSort()
    {
        var filtered = _allTasks.AsEnumerable();

        // Filtro por búsqueda
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
      var search = SearchText.ToLower();
          filtered = filtered.Where(t => 
      t.Title.ToLower().Contains(search) ||
            (t.Description != null && t.Description.ToLower().Contains(search)));
 }

        // Filtro por estado
        var selectedStatuses = new List<TaskItemStatus>();
        if (FilterPending) selectedStatuses.Add(TaskItemStatus.Pending);
        if (FilterInProgress) selectedStatuses.Add(TaskItemStatus.InProgress);
  if (FilterCompleted) selectedStatuses.Add(TaskItemStatus.Completed);

 if (selectedStatuses.Any())
        {
            filtered = filtered.Where(t => selectedStatuses.Contains(t.Status));
        }

 // Filtro por prioridad
   if (SelectedPriority.HasValue)
        {
            filtered = filtered.Where(t => t.Priority == SelectedPriority.Value);
        }

      // Filtro por tareas vencidas
        if (ShowOverdueOnly)
   {
 filtered = filtered.Where(t => t.IsOverdue);
        }

        // Ordenamiento
     filtered = SortBy switch
   {
       "Prioridad" => filtered.OrderByDescending(t => t.Priority)
        .ThenBy(t => t.DueDate),
            "Estado" => filtered.OrderBy(t => t.Status)
     .ThenByDescending(t => t.Priority),
      "Titulo" => filtered.OrderBy(t => t.Title),
   "Vencimiento" => filtered.OrderBy(t => t.DueDate ?? DateTime.MaxValue),
        _ => filtered.OrderBy(t => t.Status)
         .ThenByDescending(t => t.Priority)
  .ThenBy(t => t.DueDate)
        };

     // Actualizar colección observable
     Tasks.Clear();
   foreach (var task in filtered)
  {
Tasks.Add(new TaskItemViewModel(task));
   }
    }

    /// <summary>
    /// Limpia todos los filtros
    /// </summary>
    [RelayCommand]
    private void ClearFilters()
    {
        SearchText = string.Empty;
        FilterPending = true;
        FilterInProgress = true;
      FilterCompleted = false;
        SelectedPriority = null;
    ShowOverdueOnly = false;
   SortBy = "Fecha";
  }

    /// <summary>
    /// Crea una nueva tarea en esta lista
    /// </summary>
 [RelayCommand]
    private async Task CreateTaskAsync()
    {
        try
  {
            var dialog = new Views.TaskItemDialog
            {
           Owner = Application.Current.MainWindow,
Title = "Nueva Tarea"
  };

            var dialogViewModel = new TaskItemDialogViewModel(
     _taskService, 
           _logger, 
        Model.Id);
      dialog.DataContext = dialogViewModel;

            if (dialog.ShowDialog() == true)
            {
     await LoadTasksAsync();
     }
    }
     catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear tarea");
  MessageBox.Show($"Error al crear tarea: {ex.Message}", "Error",
     MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Edita la tarea seleccionada
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditOrDeleteTask))]
    private async Task EditTaskAsync()
    {
      if (SelectedTask == null) return;

 try
        {
   var dialog = new Views.TaskItemDialog
  {
              Owner = Application.Current.MainWindow,
                Title = "Editar Tarea"
            };

            var dialogViewModel = new TaskItemDialogViewModel(
     _taskService,
 _logger,
  Model.Id,
 SelectedTask.Model);
     dialog.DataContext = dialogViewModel;

   if (dialog.ShowDialog() == true)
      {
    await LoadTasksAsync();
            }
        }
 catch (Exception ex)
        {
  _logger.LogError(ex, "Error al editar tarea");
   MessageBox.Show($"Error al editar tarea: {ex.Message}", "Error",
   MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Elimina la tarea seleccionada
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditOrDeleteTask))]
    private async Task DeleteTaskAsync()
    {
        if (SelectedTask == null) return;

        try
        {
            var result = MessageBox.Show(
     $"¿Está seguro que desea eliminar la tarea '{SelectedTask.Title}'?",
 "Confirmar eliminación",
      MessageBoxButton.YesNo,
    MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
    var deleteResult = await _taskService.DeleteTaskItemAsync(SelectedTask.Model.Id);

        if (deleteResult.IsSuccess)
                {
 Tasks.Remove(SelectedTask);
         SelectedTask = null;
      await LoadTasksAsync();
          _logger.LogInformation("Tarea eliminada correctamente");
}
       else
      {
    MessageBox.Show(deleteResult.Error, "Error",
   MessageBoxButton.OK, MessageBoxImage.Error);
                }
   }
    }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar tarea");
 MessageBox.Show($"Error al eliminar tarea: {ex.Message}", "Error",
           MessageBoxButton.OK, MessageBoxImage.Error);
        }
 }

  /// <summary>
    /// Marca la tarea seleccionada como completada
  /// </summary>
    [RelayCommand(CanExecute = nameof(CanCompleteTask))]
    private async Task CompleteTaskAsync()
    {
   if (SelectedTask == null) return;

        try
        {
       var result = await _taskService.MarkTaskAsCompletedAsync(SelectedTask.Model.Id);

       if (result.IsSuccess)
 {
                await LoadTasksAsync();
          _logger.LogInformation("Tarea marcada como completada: {TaskId}", 
  SelectedTask.Model.Id);
     }
    else
  {
          MessageBox.Show(result.Error, "Error",
      MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al completar tarea");
            MessageBox.Show($"Error al completar tarea: {ex.Message}", "Error",
     MessageBoxButton.OK, MessageBoxImage.Error);
    }
    }

    /// <summary>
    /// Mueve la tarea seleccionada a otra lista
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditOrDeleteTask))]
    private async Task MoveTaskAsync()
    {
        if (SelectedTask == null) return;

 try
   {
 var dialog = new Views.MoveTaskDialog
     {
      Owner = Application.Current.MainWindow,
          Title = "Mover Tarea a Otra Lista"
      };

        // Usar el servicio directamente sin precargar listas
   var dialogViewModel = new MoveTaskDialogViewModel(
_taskService,
     Microsoft.Extensions.Logging.Abstractions.NullLogger<MoveTaskDialogViewModel>.Instance,
       SelectedTask.Model.Id,
   Model.Id,
SelectedTask.Title);
     dialog.DataContext = dialogViewModel;

         // Cargar listas disponibles
       await dialogViewModel.LoadListsCommand.ExecuteAsync(null);

  if (dialog.ShowDialog() == true)
{
    await LoadTasksAsync();
  _logger.LogInformation("Tarea movida a otra lista");
     }
 }
   catch (Exception ex)
        {
            _logger.LogError(ex, "Error al mover tarea");
            MessageBox.Show($"Error al mover tarea: {ex.Message}", "Error",
        MessageBoxButton.OK, MessageBoxImage.Error);
  }
    }

    private bool CanEditOrDeleteTask() => SelectedTask != null;
    private bool CanCompleteTask() => SelectedTask != null && 
         SelectedTask.Model.Status != TaskItemStatus.Completed;
}

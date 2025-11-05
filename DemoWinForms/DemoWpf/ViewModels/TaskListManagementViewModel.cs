using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;

namespace DemoWpf.ViewModels;

/// <summary>
/// ViewModel principal para la gestión de listas de tareas
/// </summary>
public partial class TaskListManagementViewModel : ViewModelBase
{
 private readonly ITaskService _taskService;
  private readonly ILogger<TaskListManagementViewModel> _logger;

  [ObservableProperty]
    private ObservableCollection<TaskListViewModel> _taskLists = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditTaskListCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteTaskListCommand))]
    [NotifyCanExecuteChangedFor(nameof(ViewTasksCommand))]
    private TaskListViewModel? _selectedTaskList;

    [ObservableProperty]
    private string _statusMessage = "Listo";

    [ObservableProperty]
  private bool _isLoading;

    public TaskListManagementViewModel(
        ITaskService taskService,
        ILogger<TaskListManagementViewModel> logger)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Carga todas las listas de tareas
    /// </summary>
    [RelayCommand]
    private async Task LoadTaskListsAsync()
    {
        try
  {
    IsLoading = true;
            StatusMessage = "Cargando listas...";

       var result = await _taskService.GetAllTaskListsAsync();

     if (result.IsSuccess)
            {
          TaskLists.Clear();
          foreach (var taskList in result.Value!)
             {
    TaskLists.Add(new TaskListViewModel(taskList, _taskService, _logger));
  }

        StatusMessage = $"{TaskLists.Count} lista(s) cargada(s)";
     _logger.LogInformation("Listas de tareas cargadas: {Count}", TaskLists.Count);
 }
   else
            {
    StatusMessage = $"Error: {result.Error}";
     _logger.LogWarning("Error al cargar listas: {Error}", result.Error);
    MessageBox.Show(result.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
      {
     _logger.LogError(ex, "Excepción al cargar listas de tareas");
       StatusMessage = "Error inesperado al cargar listas";
   MessageBox.Show($"Error inesperado: {ex.Message}", "Error", 
           MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
          IsLoading = false;
        }
    }

 /// <summary>
  /// Crea una nueva lista de tareas
    /// </summary>
    [RelayCommand]
    private async Task CreateTaskListAsync()
    {
try
        {
          var dialog = new Views.TaskListDialog
            {
      Owner = Application.Current.MainWindow,
        Title = "Nueva Lista de Tareas"
            };

        var dialogViewModel = new TaskListDialogViewModel(_taskService, _logger);
    dialog.DataContext = dialogViewModel;

            if (dialog.ShowDialog() == true)
  {
                await LoadTaskListsAsync();
          StatusMessage = "Lista creada exitosamente";
            }
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error al crear lista de tareas");
            MessageBox.Show($"Error al crear lista: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
   }
    }

    /// <summary>
    /// Edita la lista de tareas seleccionada
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
    private async Task EditTaskListAsync()
    {
   if (SelectedTaskList == null) return;

        try
        {
            var dialog = new Views.TaskListDialog
     {
      Owner = Application.Current.MainWindow,
                Title = "Editar Lista de Tareas"
            };

    var dialogViewModel = new TaskListDialogViewModel(
   _taskService, 
     _logger, 
    SelectedTaskList.Model);
         dialog.DataContext = dialogViewModel;

            if (dialog.ShowDialog() == true)
       {
             await LoadTaskListsAsync();
       StatusMessage = "Lista actualizada exitosamente";
            }
        }
   catch (Exception ex)
    {
  _logger.LogError(ex, "Error al editar lista de tareas");
      MessageBox.Show($"Error al editar lista: {ex.Message}", "Error",
          MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Elimina la lista de tareas seleccionada
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
    private async Task DeleteTaskListAsync()
    {
        if (SelectedTaskList == null) return;

        try
        {
         var result = MessageBox.Show(
        $"¿Está seguro que desea eliminar la lista '{SelectedTaskList.Name}'?\n\n" +
    $"Se eliminarán también todas las tareas ({SelectedTaskList.TotalTasks}) de esta lista.",
           "Confirmar eliminación",
      MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

  if (result == MessageBoxResult.Yes)
       {
     IsLoading = true;
      StatusMessage = "Eliminando lista...";

    var deleteResult = await _taskService.DeleteTaskListAsync(SelectedTaskList.Model.Id);

   if (deleteResult.IsSuccess)
      {
  TaskLists.Remove(SelectedTaskList);
SelectedTaskList = null;
      StatusMessage = "Lista eliminada exitosamente";
       _logger.LogInformation("Lista eliminada correctamente");
            }
            else
           {
        StatusMessage = $"Error: {deleteResult.Error}";
        MessageBox.Show(deleteResult.Error, "Error",
          MessageBoxButton.OK, MessageBoxImage.Error);
         }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar lista de tareas");
          MessageBox.Show($"Error al eliminar lista: {ex.Message}", "Error",
        MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
     {
     IsLoading = false;
        }
  }

    /// <summary>
    /// Abre la vista de tareas de la lista seleccionada
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
    private async Task ViewTasksAsync()
    {
        if (SelectedTaskList == null) return;

        try
        {
         // Recargar las tareas de la lista
        await SelectedTaskList.LoadTasksAsync();

         StatusMessage = $"Mostrando {SelectedTaskList.TotalTasks} tarea(s) de '{SelectedTaskList.Name}'";
        }
 catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar tareas");
   MessageBox.Show($"Error al cargar tareas: {ex.Message}", "Error",
   MessageBoxButton.OK, MessageBoxImage.Error);
}
    }

    /// <summary>
    /// Determina si se pueden ejecutar los comandos de edición/eliminación
    /// </summary>
    private bool CanEditOrDelete() => SelectedTaskList != null && !IsLoading;
}

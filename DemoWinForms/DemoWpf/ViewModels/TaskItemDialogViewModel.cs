using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace DemoWpf.ViewModels;

/// <summary>
/// ViewModel para el diálogo de crear/editar tarea
/// </summary>
public partial class TaskItemDialogViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger _logger;
    private readonly Guid _taskListId;
    private readonly TaskItem? _existingTask;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private DateTime? _dueDate;

    [ObservableProperty]
    private TaskListPriority _priority = TaskListPriority.Medium;

    [ObservableProperty]
    private TaskItemStatus _status = TaskItemStatus.Pending;

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    [ObservableProperty]
    private bool _isSaving;

    /// <summary>
 /// Indica si estamos editando (true) o creando (false)
 /// </summary>
    public bool IsEditMode => _existingTask != null;

    /// <summary>
    /// Título del diálogo
    /// </summary>
    public string DialogTitle => IsEditMode ? "Editar Tarea" : "Nueva Tarea";

    /// <summary>
    /// Resultado del diálogo
    /// </summary>
    public bool DialogResult { get; private set; }

    /// <summary>
    /// Lista de prioridades disponibles
    /// </summary>
    public Array PriorityOptions => Enum.GetValues(typeof(TaskListPriority));

    /// <summary>
    /// Lista de estados disponibles
    /// </summary>
    public Array StatusOptions => Enum.GetValues(typeof(TaskItemStatus));

    /// <summary>
    /// Constructor para crear nueva tarea
    /// </summary>
    public TaskItemDialogViewModel(
     ITaskService taskService,
      ILogger logger,
    Guid taskListId)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
 _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _taskListId = taskListId;
    }

    /// <summary>
 /// Constructor para editar tarea existente
 /// </summary>
    public TaskItemDialogViewModel(
     ITaskService taskService,
        ILogger logger,
      Guid taskListId,
        TaskItem existingTask) : this(taskService, logger, taskListId)
    {
        _existingTask = existingTask ?? throw new ArgumentNullException(nameof(existingTask));
        
        // Cargar datos existentes
        Title = existingTask.Title;
        Description = existingTask.Description ?? string.Empty;
        DueDate = existingTask.DueDate;
        Priority = existingTask.Priority;
        Status = existingTask.Status;
    }

    /// <summary>
    /// Guarda la tarea (crear o actualizar)
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync(Window? window)
    {
        try
        {
 IsSaving = true;
            ValidationMessage = string.Empty;

        // Validación
            if (string.IsNullOrWhiteSpace(Title))
         {
   ValidationMessage = "El título es obligatorio";
  return;
            }

          if (Title.Length > 200)
            {
                ValidationMessage = "El título no puede exceder 200 caracteres";
 return;
      }

  if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 1000)
            {
   ValidationMessage = "La descripción no puede exceder 1000 caracteres";
         return;
     }

        if (IsEditMode)
            {
      // Actualizar tarea existente
    _existingTask!.Title = Title.Trim();
          _existingTask.Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim();
       _existingTask.DueDate = DueDate;
    _existingTask.Priority = Priority;
  _existingTask.Status = Status;

        if (Status == TaskItemStatus.Completed && !_existingTask.CompletedAt.HasValue)
       {
          _existingTask.CompletedAt = DateTime.UtcNow;
        }

            var result = await _taskService.UpdateTaskItemAsync(_existingTask);

          if (result.IsSuccess)
      {
    _logger.LogInformation("Tarea actualizada: {Title}", Title);
 DialogResult = true;
     window?.Close();
    }
     else
       {
  ValidationMessage = result.Error;
           _logger.LogWarning("Error al actualizar tarea: {Error}", result.Error);
         }
      }
            else
          {
     // Crear nueva tarea
       var newTask = new TaskItem
            {
             TaskListId = _taskListId,
           Title = Title.Trim(),
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
DueDate = DueDate,
 Priority = Priority,
  Status = Status
        };

    var result = await _taskService.CreateTaskItemAsync(newTask);

   if (result.IsSuccess)
                {
      _logger.LogInformation("Tarea creada: {Title}", Title);
DialogResult = true;
        window?.Close();
            }
     else
          {
      ValidationMessage = result.Error;
           _logger.LogWarning("Error al crear tarea: {Error}", result.Error);
        }
     }
        }
    catch (Exception ex)
        {
          _logger.LogError(ex, "Excepción al guardar tarea");
            ValidationMessage = $"Error inesperado: {ex.Message}";
        }
        finally
      {
            IsSaving = false;
        }
    }

    /// <summary>
    /// Cancela el diálogo
    /// </summary>
    [RelayCommand]
    private void Cancel(Window? window)
    {
     DialogResult = false;
    window?.Close();
    }

    /// <summary>
    /// Limpia la fecha de vencimiento
    /// </summary>
    [RelayCommand]
    private void ClearDueDate()
    {
      DueDate = null;
    }

    /// <summary>
    /// Establece la fecha de vencimiento a hoy
    /// </summary>
    [RelayCommand]
    private void SetDueDateToday()
    {
        DueDate = DateTime.Today;
    }

    /// <summary>
    /// Establece la fecha de vencimiento a mañana
    /// </summary>
    [RelayCommand]
    private void SetDueDateTomorrow()
    {
      DueDate = DateTime.Today.AddDays(1);
    }

    /// <summary>
    /// Establece la fecha de vencimiento en una semana
    /// </summary>
    [RelayCommand]
    private void SetDueDateNextWeek()
    {
        DueDate = DateTime.Today.AddDays(7);
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Title) && !IsSaving;
}

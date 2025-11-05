using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace DemoWpf.ViewModels;

/// <summary>
/// ViewModel para el diálogo de crear/editar lista de tareas
/// </summary>
public partial class TaskListDialogViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger _logger;
    private readonly TaskList? _existingList;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _colorCode = "#4A90E2"; // Azul por defecto

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    [ObservableProperty]
    private bool _isSaving;

    /// <summary>
    /// Indica si estamos editando (true) o creando (false)
    /// </summary>
    public bool IsEditMode => _existingList != null;

    /// <summary>
    /// Título del diálogo
    /// </summary>
    public string DialogTitle => IsEditMode ? "Editar Lista de Tareas" : "Nueva Lista de Tareas";

    /// <summary>
    /// Resultado del diálogo
    /// </summary>
    public bool DialogResult { get; private set; }

    /// <summary>
    /// Constructor para crear nueva lista
    /// </summary>
    public TaskListDialogViewModel(
    ITaskService taskService,
ILogger logger)
    {
   _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
     _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Constructor para editar lista existente
    /// </summary>
    public TaskListDialogViewModel(
 ITaskService taskService,
        ILogger logger,
   TaskList existingList) : this(taskService, logger)
    {
        _existingList = existingList ?? throw new ArgumentNullException(nameof(existingList));
        
        // Cargar datos existentes
        Name = existingList.Name;
        Description = existingList.Description ?? string.Empty;
    ColorCode = existingList.ColorCode ?? "#4A90E2";
}

    /// <summary>
    /// Guarda la lista (crear o actualizar)
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync(Window? window)
    {
        try
        {
            IsSaving = true;
       ValidationMessage = string.Empty;

          // Validación
   if (string.IsNullOrWhiteSpace(Name))
    {
         ValidationMessage = "El nombre es obligatorio";
    return;
    }

            if (Name.Length > 100)
    {
   ValidationMessage = "El nombre no puede exceder 100 caracteres";
        return;
 }

   if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 500)
 {
        ValidationMessage = "La descripción no puede exceder 500 caracteres";
     return;
        }

          if (IsEditMode)
   {
           // Actualizar lista existente
          _existingList!.Name = Name.Trim();
         _existingList.Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim();
                _existingList.ColorCode = ColorCode;

      var result = await _taskService.UpdateTaskListAsync(_existingList);

                if (result.IsSuccess)
       {
       _logger.LogInformation("Lista actualizada: {Name}", Name);
   DialogResult = true;
       window?.Close();
                }
     else
           {
       ValidationMessage = result.Error;
          _logger.LogWarning("Error al actualizar lista: {Error}", result.Error);
                }
   }
    else
  {
                // Crear nueva lista
           var newList = new TaskList
                {
  Name = Name.Trim(),
        Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
  ColorCode = ColorCode
   };

         var result = await _taskService.CreateTaskListAsync(newList);

  if (result.IsSuccess)
       {
         _logger.LogInformation("Lista creada: {Name}", Name);
       DialogResult = true;
window?.Close();
                }
        else
           {
   ValidationMessage = result.Error;
  _logger.LogWarning("Error al crear lista: {Error}", result.Error);
}
   }
     }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Excepción al guardar lista");
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
    /// Selecciona un color predefinido
    /// </summary>
    [RelayCommand]
    private void SelectColor(string color)
    {
        ColorCode = color;
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Name) && !IsSaving;
}

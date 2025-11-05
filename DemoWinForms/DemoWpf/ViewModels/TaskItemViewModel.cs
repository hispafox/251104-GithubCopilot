using CommunityToolkit.Mvvm.ComponentModel;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;

namespace DemoWpf.ViewModels;

/// <summary>
/// ViewModel para una tarea individual
/// </summary>
public partial class TaskItemViewModel : ViewModelBase
{
    /// <summary>
    /// Modelo de dominio subyacente
    /// </summary>
    public TaskItem Model { get; }

    // Propiedades que se exponen del modelo
    public Guid Id => Model.Id;
    public string Title => Model.Title;
    public string? Description => Model.Description;
    public DateTime CreatedAt => Model.CreatedAt;
 public DateTime? DueDate => Model.DueDate;
    public TaskListPriority Priority => Model.Priority;
    public TaskItemStatus Status => Model.Status;
    public DateTime? CompletedAt => Model.CompletedAt;
    public bool IsOverdue => Model.IsOverdue;
    public bool IsCompleted => Model.IsCompleted;
    public string StatusDescription => Model.StatusDescription;
    public string PriorityDescription => Model.PriorityDescription;

    /// <summary>
    /// Color de fondo según prioridad
    /// </summary>
    public string PriorityColor => Priority switch
    {
        TaskListPriority.High => "#FFE6E6",     // Rojo claro
    TaskListPriority.Medium => "#FFF4E6",   // Naranja claro
        TaskListPriority.Low => "#E6FFE6",      // Verde claro
        _ => "Transparent"
  };

    /// <summary>
    /// Color de texto según estado
    /// </summary>
    public string StatusColor => Status switch
    {
        TaskItemStatus.Completed => "Gray",
        TaskItemStatus.InProgress => "Blue",
  TaskItemStatus.Pending => "Black",
        _ => "Black"
    };

    /// <summary>
    /// Ícono según estado
    /// </summary>
    public string StatusIcon => Status switch
    {
        TaskItemStatus.Completed => "?",
    TaskItemStatus.InProgress => "?",
    TaskItemStatus.Pending => "?",
        _ => "?"
    };

    /// <summary>
    /// Ícono según prioridad
    /// </summary>
    public string PriorityIcon => Priority switch
 {
  TaskListPriority.High => "??",
      TaskListPriority.Medium => "??",
TaskListPriority.Low => "??",
        _ => ""
    };

    /// <summary>
    /// Formato de fecha de vencimiento
    /// </summary>
    public string DueDateDisplay
    {
        get
        {
   if (!DueDate.HasValue)
      return "Sin fecha";

     if (IsOverdue)
        return $"?? Vencida: {DueDate.Value:dd/MM/yyyy}";

            var daysUntil = (DueDate.Value - DateTime.UtcNow).Days;
        if (daysUntil == 0)
                return "? Vence hoy";
    if (daysUntil == 1)
  return "?? Vence mañana";
    if (daysUntil <= 7)
            return $"?? Vence en {daysUntil} días";

   return $"?? {DueDate.Value:dd/MM/yyyy}";
        }
    }

    /// <summary>
    /// Estilo de fuente (tachado si está completada)
  /// </summary>
    public string TextDecoration => IsCompleted ? "Strikethrough" : "None";

 public TaskItemViewModel(TaskItem model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }

    /// <summary>
    /// Actualiza las propiedades notificando cambios
    /// </summary>
    public void Refresh()
    {
        OnPropertyChanged(nameof(Title));
    OnPropertyChanged(nameof(Description));
     OnPropertyChanged(nameof(DueDate));
        OnPropertyChanged(nameof(Priority));
        OnPropertyChanged(nameof(Status));
        OnPropertyChanged(nameof(CompletedAt));
        OnPropertyChanged(nameof(IsOverdue));
    OnPropertyChanged(nameof(IsCompleted));
     OnPropertyChanged(nameof(StatusDescription));
     OnPropertyChanged(nameof(PriorityDescription));
        OnPropertyChanged(nameof(PriorityColor));
     OnPropertyChanged(nameof(StatusColor));
        OnPropertyChanged(nameof(StatusIcon));
        OnPropertyChanged(nameof(PriorityIcon));
        OnPropertyChanged(nameof(DueDateDisplay));
        OnPropertyChanged(nameof(TextDecoration));
    }
}

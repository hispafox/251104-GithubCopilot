using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoWinForms.Domain.Enums;

namespace DemoWinForms.Domain.Entities;

/// <summary>
/// Representa una tarea individual dentro de una lista de tareas
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Identificador único de la tarea
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID de la lista a la que pertenece esta tarea
    /// </summary>
    [Required]
  public Guid TaskListId { get; set; }

    /// <summary>
    /// Título de la tarea (obligatorio, máximo 200 caracteres)
    /// </summary>
    [Required(ErrorMessage = "El título de la tarea es obligatorio")]
    [MaxLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada de la tarea (máximo 1000 caracteres)
    /// </summary>
    [MaxLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Fecha y hora de creación de la tarea
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de vencimiento opcional
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Prioridad de la tarea (Baja, Media, Alta)
    /// </summary>
    public TaskListPriority Priority { get; set; } = TaskListPriority.Medium;

/// <summary>
    /// Estado actual de la tarea (Pendiente, En Progreso, Completada)
    /// </summary>
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    /// <summary>
    /// Fecha y hora en que se completó la tarea
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Navegación a la lista padre
    /// </summary>
    [ForeignKey(nameof(TaskListId))]
    public virtual TaskList? TaskList { get; set; }

    /// <summary>
    /// Indica si la tarea está vencida
    /// </summary>
    [NotMapped]
public bool IsOverdue => DueDate.HasValue && 
           DueDate.Value < DateTime.UtcNow && 
          Status != TaskItemStatus.Completed;

    /// <summary>
    /// Indica si la tarea está completada
    /// </summary>
  [NotMapped]
    public bool IsCompleted => Status == TaskItemStatus.Completed;

    /// <summary>
    /// Obtiene la descripción del estado
    /// </summary>
    [NotMapped]
    public string StatusDescription => Status switch
    {
        TaskItemStatus.Pending => "Pendiente",
  TaskItemStatus.InProgress => "En Progreso",
      TaskItemStatus.Completed => "Completada",
        _ => "Desconocido"
    };

    /// <summary>
    /// Obtiene la descripción de la prioridad
    /// </summary>
    [NotMapped]
    public string PriorityDescription => Priority switch
    {
        TaskListPriority.Low => "Baja",
      TaskListPriority.Medium => "Media",
     TaskListPriority.High => "Alta",
        _ => "Desconocida"
    };
}

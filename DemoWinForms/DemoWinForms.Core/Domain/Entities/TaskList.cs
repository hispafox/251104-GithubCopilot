using System.ComponentModel.DataAnnotations;

namespace DemoWinForms.Domain.Entities;

/// <summary>
/// Representa una lista de tareas que agrupa múltiples tareas individuales
/// </summary>
public class TaskList
{
    /// <summary>
    /// Identificador único de la lista
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la lista (obligatorio, máximo 100 caracteres)
    /// </summary>
    [Required(ErrorMessage = "El nombre de la lista es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción opcional de la lista (máximo 500 caracteres)
    /// </summary>
    [MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Código de color identificativo en formato hexadecimal (#RRGGBB)
    /// </summary>
    [MaxLength(7)]
    public string? ColorCode { get; set; }

  /// <summary>
    /// Fecha y hora de creación de la lista
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha y hora de la última modificación
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
/// Colección de tareas asociadas a esta lista
/// </summary>
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    /// <summary>
    /// Obtiene el número total de tareas en la lista
    /// </summary>
    public int TotalTasks => Tasks.Count;

    /// <summary>
    /// Obtiene el número de tareas completadas
    /// </summary>
    public int CompletedTasksCount => Tasks.Count(t => t.Status == Enums.TaskItemStatus.Completed);

    /// <summary>
    /// Obtiene el número de tareas pendientes
    /// </summary>
    public int PendingTasksCount => Tasks.Count(t => t.Status == Enums.TaskItemStatus.Pending);

    /// <summary>
    /// Obtiene el porcentaje de progreso (0-100)
    /// </summary>
    public double ProgressPercentage => TotalTasks > 0 ? (CompletedTasksCount * 100.0 / TotalTasks) : 0;
}

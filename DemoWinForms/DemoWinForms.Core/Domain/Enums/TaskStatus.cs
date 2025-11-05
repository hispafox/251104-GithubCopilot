namespace DemoWinForms.Domain.Enums;

/// <summary>
/// Estado de una tarea en una lista
/// </summary>
public enum TaskItemStatus
{
    /// <summary>
  /// Tarea pendiente de iniciar
    /// </summary>
  Pending = 0,

    /// <summary>
    /// Tarea en progreso
    /// </summary>
    InProgress = 1,

    /// <summary>
/// Tarea completada
    /// </summary>
    Completed = 2
}

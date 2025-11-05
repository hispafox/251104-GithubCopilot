using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;

namespace DemoWinForms.Data.Repositories;

/// <summary>
/// Interface para operaciones de repositorio de tareas y listas
/// </summary>
public interface ITaskRepository
{
    // ========== Operaciones con TaskList ==========
    
    /// <summary>
    /// Obtiene todas las listas de tareas
  /// </summary>
    Task<IEnumerable<TaskList>> GetAllTaskListsAsync();

    /// <summary>
    /// Obtiene una lista de tareas por su ID
    /// </summary>
 Task<TaskList?> GetTaskListByIdAsync(Guid id);

    /// <summary>
    /// Crea una nueva lista de tareas
    /// </summary>
    Task<TaskList> CreateTaskListAsync(TaskList taskList);

 /// <summary>
    /// Actualiza una lista de tareas existente
    /// </summary>
    Task<TaskList> UpdateTaskListAsync(TaskList taskList);

    /// <summary>
    /// Elimina una lista de tareas
  /// </summary>
    Task<bool> DeleteTaskListAsync(Guid id);

    /// <summary>
    /// Verifica si existe una lista con el nombre especificado
    /// </summary>
    Task<bool> TaskListExistsAsync(string name, Guid? excludeId = null);

  // ========== Operaciones con TaskItem ==========
    
    /// <summary>
    /// Obtiene todas las tareas de una lista específica
    /// </summary>
    Task<IEnumerable<TaskItem>> GetTaskItemsByListIdAsync(Guid taskListId);

    /// <summary>
    /// Obtiene una tarea por su ID
    /// </summary>
    Task<TaskItem?> GetTaskItemByIdAsync(Guid id);

    /// <summary>
    /// Crea una nueva tarea
    /// </summary>
    Task<TaskItem> CreateTaskItemAsync(TaskItem taskItem);

    /// <summary>
    /// Actualiza una tarea existente
    /// </summary>
    Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem);

    /// <summary>
    /// Elimina una tarea
    /// </summary>
    Task<bool> DeleteTaskItemAsync(Guid id);

    /// <summary>
    /// Marca una tarea como completada
    /// </summary>
    Task<TaskItem?> MarkTaskAsCompletedAsync(Guid id);

 /// <summary>
    /// Mueve una tarea a otra lista
    /// </summary>
    Task<TaskItem?> MoveTaskToListAsync(Guid taskId, Guid newTaskListId);

    // ========== Operaciones de consulta avanzada ==========
  
    /// <summary>
    /// Busca tareas por texto en título o descripción
    /// </summary>
    Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchText);

    /// <summary>
    /// Obtiene tareas filtradas por estado
    /// </summary>
    Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Guid taskListId, TaskItemStatus status);

    /// <summary>
    /// Obtiene tareas filtradas por prioridad
    /// </summary>
    Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(Guid taskListId, TaskListPriority priority);

    /// <summary>
    /// Obtiene tareas vencidas
    /// </summary>
    Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();

  /// <summary>
    /// Obtiene estadísticas generales
    /// </summary>
    Task<Dictionary<string, int>> GetStatisticsAsync();
}

using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Common;

namespace DemoWinForms.Business.Services;

/// <summary>
/// Interface del servicio de lógica de negocio para listas de tareas
/// </summary>
public interface ITaskService
{
    // ========== Operaciones con TaskList ==========
    
    /// <summary>
    /// Obtiene todas las listas de tareas
    /// </summary>
    Task<Result<IEnumerable<TaskList>>> GetAllTaskListsAsync();

    /// <summary>
    /// Obtiene una lista de tareas por su ID
    /// </summary>
    Task<Result<TaskList>> GetTaskListByIdAsync(Guid id);

    /// <summary>
    /// Crea una nueva lista de tareas
    /// </summary>
    Task<Result<TaskList>> CreateTaskListAsync(TaskList taskList);

    /// <summary>
    /// Actualiza una lista de tareas existente
    /// </summary>
    Task<Result<TaskList>> UpdateTaskListAsync(TaskList taskList);

    /// <summary>
    /// Elimina una lista de tareas
    /// </summary>
    Task<Result<bool>> DeleteTaskListAsync(Guid id);

    // ========== Operaciones con TaskItem ==========
 
    /// <summary>
    /// Obtiene todas las tareas de una lista específica
    /// </summary>
    Task<Result<IEnumerable<TaskItem>>> GetTaskItemsByListIdAsync(Guid taskListId);

    /// <summary>
    /// Obtiene una tarea por su ID
    /// </summary>
    Task<Result<TaskItem>> GetTaskItemByIdAsync(Guid id);

    /// <summary>
    /// Crea una nueva tarea
    /// </summary>
  Task<Result<TaskItem>> CreateTaskItemAsync(TaskItem taskItem);

    /// <summary>
 /// Actualiza una tarea existente
    /// </summary>
    Task<Result<TaskItem>> UpdateTaskItemAsync(TaskItem taskItem);

    /// <summary>
    /// Elimina una tarea
    /// </summary>
    Task<Result<bool>> DeleteTaskItemAsync(Guid id);

  /// <summary>
    /// Marca una tarea como completada
    /// </summary>
    Task<Result<TaskItem>> MarkTaskAsCompletedAsync(Guid id);

/// <summary>
    /// Mueve una tarea a otra lista
    /// </summary>
    Task<Result<TaskItem>> MoveTaskToListAsync(Guid taskId, Guid newTaskListId);

    // ========== Operaciones de consulta ==========
    
    /// <summary>
    /// Busca tareas por texto
    /// </summary>
    Task<Result<IEnumerable<TaskItem>>> SearchTasksAsync(string searchText);

    /// <summary>
    /// Obtiene tareas por estado
    /// </summary>
 Task<Result<IEnumerable<TaskItem>>> GetTasksByStatusAsync(Guid taskListId, TaskItemStatus status);

    /// <summary>
    /// Obtiene tareas por prioridad
    /// </summary>
    Task<Result<IEnumerable<TaskItem>>> GetTasksByPriorityAsync(Guid taskListId, TaskListPriority priority);

    /// <summary>
    /// Obtiene tareas vencidas
    /// </summary>
    Task<Result<IEnumerable<TaskItem>>> GetOverdueTasksAsync();

    /// <summary>
    /// Obtiene estadísticas generales
    /// </summary>
    Task<Result<Dictionary<string, int>>> GetStatisticsAsync();
}

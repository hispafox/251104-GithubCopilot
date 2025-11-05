using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;

namespace DemoWinForms.Data.Repositories;

/// <summary>
/// Implementación del repositorio para operaciones con tareas y listas
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskRepository> _logger;

public TaskRepository(AppDbContext context, ILogger<TaskRepository> logger)
    {
  _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // ========== Operaciones con TaskList ==========

    public async Task<IEnumerable<TaskList>> GetAllTaskListsAsync()
    {
        try
     {
return await _context.TaskLists
       .Include(tl => tl.Tasks)
          .OrderByDescending(tl => tl.CreatedAt)
 .AsNoTracking()
  .ToListAsync();
        }
     catch (Exception ex)
        {
  _logger.LogError(ex, "Error al obtener todas las listas de tareas");
            throw;
        }
 }

    public async Task<TaskList?> GetTaskListByIdAsync(Guid id)
    {
        try
   {
          return await _context.TaskLists
        .Include(tl => tl.Tasks)
      .FirstOrDefaultAsync(tl => tl.Id == id);
 }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error al obtener lista de tareas con ID: {Id}", id);
   throw;
        }
    }

    public async Task<TaskList> CreateTaskListAsync(TaskList taskList)
    {
        try
        {
            taskList.Id = Guid.NewGuid();
            taskList.CreatedAt = DateTime.UtcNow;

            _context.TaskLists.Add(taskList);
            await _context.SaveChangesAsync();

     _logger.LogInformation("Lista de tareas creada: {Name} (ID: {Id})", 
       taskList.Name, taskList.Id);

          return taskList;
        }
   catch (Exception ex)
        {
 _logger.LogError(ex, "Error al crear lista de tareas: {Name}", taskList.Name);
      throw;
        }
    }

    public async Task<TaskList> UpdateTaskListAsync(TaskList taskList)
    {
        try
 {
            taskList.ModifiedAt = DateTime.UtcNow;

       _context.TaskLists.Update(taskList);
       await _context.SaveChangesAsync();

  _logger.LogInformation("Lista de tareas actualizada: {Name} (ID: {Id})", 
        taskList.Name, taskList.Id);

        return taskList;
}
        catch (Exception ex)
      {
       _logger.LogError(ex, "Error al actualizar lista de tareas con ID: {Id}", 
              taskList.Id);
            throw;
        }
    }

    public async Task<bool> DeleteTaskListAsync(Guid id)
  {
        try
{
 var taskList = await _context.TaskLists.FindAsync(id);
         if (taskList == null)
       {
         _logger.LogWarning("Intento de eliminar lista inexistente: {Id}", id);
            return false;
   }

   _context.TaskLists.Remove(taskList);
         await _context.SaveChangesAsync();

            _logger.LogInformation("Lista de tareas eliminada: {Name} (ID: {Id})", 
            taskList.Name, id);

   return true;
        }
        catch (Exception ex)
        {
 _logger.LogError(ex, "Error al eliminar lista de tareas con ID: {Id}", id);
            throw;
     }
    }

    public async Task<bool> TaskListExistsAsync(string name, Guid? excludeId = null)
{
        try
    {
      var query = _context.TaskLists.AsQueryable();

     if (excludeId.HasValue)
     {
     query = query.Where(tl => tl.Id != excludeId.Value);
            }

            return await query.AnyAsync(tl => tl.Name.ToLower() == name.ToLower());
      }
 catch (Exception ex)
    {
          _logger.LogError(ex, "Error al verificar existencia de lista: {Name}", name);
            throw;
        }
    }

    // ========== Operaciones con TaskItem ==========

    public async Task<IEnumerable<TaskItem>> GetTaskItemsByListIdAsync(Guid taskListId)
    {
        try
        {
      return await _context.TaskItems
       .Where(ti => ti.TaskListId == taskListId)
     .OrderBy(ti => ti.Status)
        .ThenByDescending(ti => ti.Priority)
                .ThenBy(ti => ti.DueDate)
 .AsNoTracking()
         .ToListAsync();
}
    catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tareas de la lista: {ListId}", taskListId);
            throw;
        }
    }

    public async Task<TaskItem?> GetTaskItemByIdAsync(Guid id)
    {
        try
        {
            return await _context.TaskItems
                .Include(ti => ti.TaskList)
.FirstOrDefaultAsync(ti => ti.Id == id);
        }
        catch (Exception ex)
     {
       _logger.LogError(ex, "Error al obtener tarea con ID: {Id}", id);
  throw;
        }
    }

    public async Task<TaskItem> CreateTaskItemAsync(TaskItem taskItem)
  {
        try
        {
            taskItem.Id = Guid.NewGuid();
  taskItem.CreatedAt = DateTime.UtcNow;

            _context.TaskItems.Add(taskItem);
    await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea creada: {Title} (ID: {Id})", 
                taskItem.Title, taskItem.Id);

      return taskItem;
        }
        catch (Exception ex)
 {
            _logger.LogError(ex, "Error al crear tarea: {Title}", taskItem.Title);
    throw;
        }
    }

  public async Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem)
    {
        try
     {
   _context.TaskItems.Update(taskItem);
         await _context.SaveChangesAsync();

      _logger.LogInformation("Tarea actualizada: {Title} (ID: {Id})", 
      taskItem.Title, taskItem.Id);

  return taskItem;
  }
        catch (Exception ex)
        {
    _logger.LogError(ex, "Error al actualizar tarea con ID: {Id}", taskItem.Id);
     throw;
    }
}

    public async Task<bool> DeleteTaskItemAsync(Guid id)
    {
        try
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
  if (taskItem == null)
            {
      _logger.LogWarning("Intento de eliminar tarea inexistente: {Id}", id);
                return false;
     }

            _context.TaskItems.Remove(taskItem);
         await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea eliminada: {Title} (ID: {Id})", 
   taskItem.Title, id);

  return true;
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error al eliminar tarea con ID: {Id}", id);
            throw;
        }
    }

    public async Task<TaskItem?> MarkTaskAsCompletedAsync(Guid id)
    {
    try
        {
       var taskItem = await _context.TaskItems.FindAsync(id);
     if (taskItem == null)
  {
        return null;
       }

 taskItem.Status = TaskItemStatus.Completed;
         taskItem.CompletedAt = DateTime.UtcNow;

 await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea marcada como completada: {Id}", id);

     return taskItem;
   }
        catch (Exception ex)
        {
       _logger.LogError(ex, "Error al marcar tarea como completada: {Id}", id);
     throw;
  }
    }

    public async Task<TaskItem?> MoveTaskToListAsync(Guid taskId, Guid newTaskListId)
    {
    try
        {
    var taskItem = await _context.TaskItems.FindAsync(taskId);
            if (taskItem == null)
   {
         return null;
          }

    var newList = await _context.TaskLists.FindAsync(newTaskListId);
     if (newList == null)
      {
 _logger.LogWarning("Lista destino no existe: {ListId}", newTaskListId);
   return null;
   }

      taskItem.TaskListId = newTaskListId;
            await _context.SaveChangesAsync();

     _logger.LogInformation("Tarea {TaskId} movida a lista {ListId}", 
          taskId, newTaskListId);

          return taskItem;
        }
    catch (Exception ex)
        {
   _logger.LogError(ex, "Error al mover tarea {TaskId} a lista {ListId}", 
          taskId, newTaskListId);
          throw;
     }
 }

    // ========== Operaciones de consulta avanzada ==========

    public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchText)
    {
  try
 {
            if (string.IsNullOrWhiteSpace(searchText))
  {
          return Enumerable.Empty<TaskItem>();
  }

        var search = searchText.ToLower();

            return await _context.TaskItems
        .Include(ti => ti.TaskList)
         .Where(ti => ti.Title.ToLower().Contains(search) || 
          (ti.Description != null && ti.Description.ToLower().Contains(search)))
        .AsNoTracking()
      .ToListAsync();
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error al buscar tareas con texto: {SearchText}", searchText);
       throw;
        }
    }

  public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Guid taskListId, TaskItemStatus status)
    {
     try
        {
return await _context.TaskItems
    .Where(ti => ti.TaskListId == taskListId && ti.Status == status)
      .AsNoTracking()
       .ToListAsync();
   }
   catch (Exception ex)
        {
     _logger.LogError(ex, "Error al obtener tareas por estado {Status} en lista {ListId}", 
     status, taskListId);
            throw;
        }
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(Guid taskListId, TaskListPriority priority)
    {
        try
        {
 return await _context.TaskItems
    .Where(ti => ti.TaskListId == taskListId && ti.Priority == priority)
          .AsNoTracking()
   .ToListAsync();
        }
     catch (Exception ex)
   {
     _logger.LogError(ex, "Error al obtener tareas por prioridad {Priority} en lista {ListId}", 
     priority, taskListId);
     throw;
        }
 }

    public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
 {
 try
   {
      var now = DateTime.UtcNow;

  return await _context.TaskItems
         .Include(ti => ti.TaskList)
  .Where(ti => ti.DueDate.HasValue && 
     ti.DueDate.Value < now && 
       ti.Status != TaskItemStatus.Completed)
    .AsNoTracking()
      .ToListAsync();
        }
        catch (Exception ex)
    {
       _logger.LogError(ex, "Error al obtener tareas vencidas");
   throw;
     }
    }

 public async Task<Dictionary<string, int>> GetStatisticsAsync()
    {
        try
   {
        var stats = new Dictionary<string, int>
   {
     ["TotalLists"] = await _context.TaskLists.CountAsync(),
      ["TotalTasks"] = await _context.TaskItems.CountAsync(),
      ["CompletedTasks"] = await _context.TaskItems
     .CountAsync(ti => ti.Status == TaskItemStatus.Completed),
    ["PendingTasks"] = await _context.TaskItems
          .CountAsync(ti => ti.Status == TaskItemStatus.Pending),
     ["InProgressTasks"] = await _context.TaskItems
 .CountAsync(ti => ti.Status == TaskItemStatus.InProgress),
  ["OverdueTasks"] = await _context.TaskItems
        .CountAsync(ti => ti.DueDate.HasValue && 
        ti.DueDate.Value < DateTime.UtcNow && 
    ti.Status != TaskItemStatus.Completed)
    };

            return stats;
        }
  catch (Exception ex)
        {
       _logger.LogError(ex, "Error al obtener estadísticas");
      throw;
        }
    }
}

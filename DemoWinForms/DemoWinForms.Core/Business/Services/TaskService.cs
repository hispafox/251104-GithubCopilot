using Microsoft.Extensions.Logging;
using DemoWinForms.Data.Repositories;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Common;

namespace DemoWinForms.Business.Services;

/// <summary>
/// Servicio de lógica de negocio para operaciones con listas de tareas
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // ========== TaskList Operations ==========

  public async Task<Result<IEnumerable<TaskList>>> GetAllTaskListsAsync()
  {
  try
        {
            var lists = await _repository.GetAllTaskListsAsync();
  return Result<IEnumerable<TaskList>>.Success(lists);
        }
     catch (Exception ex)
        {
         _logger.LogError(ex, "Error en servicio al obtener listas");
            return Result<IEnumerable<TaskList>>.Failure("Error al obtener las listas de tareas");
        }
    }

    public async Task<Result<TaskList>> GetTaskListByIdAsync(Guid id)
  {
        try
        {
 var list = await _repository.GetTaskListByIdAsync(id);
  if (list == null)
  {
        return Result<TaskList>.Failure("Lista no encontrada");
  }

            return Result<TaskList>.Success(list);
        }
        catch (Exception ex)
        {
 _logger.LogError(ex, "Error en servicio al obtener lista {Id}", id);
            return Result<TaskList>.Failure("Error al obtener la lista");
        }
    }

    public async Task<Result<TaskList>> CreateTaskListAsync(TaskList taskList)
    {
 try
        {
     // Validar nombre vacío
        if (string.IsNullOrWhiteSpace(taskList.Name))
            {
       return Result<TaskList>.Failure("El nombre de la lista es obligatorio");
            }

     // Validar nombre duplicado
            if (await _repository.TaskListExistsAsync(taskList.Name))
    {
      return Result<TaskList>.Failure("Ya existe una lista con ese nombre");
            }

            var created = await _repository.CreateTaskListAsync(taskList);
      return Result<TaskList>.Success(created);
      }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al crear lista");
            return Result<TaskList>.Failure("Error al crear la lista");
        }
    }

    public async Task<Result<TaskList>> UpdateTaskListAsync(TaskList taskList)
    {
try
     {
          // Validar nombre vacío
  if (string.IsNullOrWhiteSpace(taskList.Name))
            {
     return Result<TaskList>.Failure("El nombre de la lista es obligatorio");
      }

     // Validar nombre duplicado (excluyendo esta lista)
            if (await _repository.TaskListExistsAsync(taskList.Name, taskList.Id))
            {
          return Result<TaskList>.Failure("Ya existe otra lista con ese nombre");
     }

        var updated = await _repository.UpdateTaskListAsync(taskList);
            return Result<TaskList>.Success(updated);
        }
        catch (Exception ex)
    {
        _logger.LogError(ex, "Error en servicio al actualizar lista {Id}", taskList.Id);
            return Result<TaskList>.Failure("Error al actualizar la lista");
     }
    }

 public async Task<Result<bool>> DeleteTaskListAsync(Guid id)
    {
        try
        {
        var result = await _repository.DeleteTaskListAsync(id);
    if (!result)
            {
  return Result<bool>.Failure("Lista no encontrada");
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
       _logger.LogError(ex, "Error en servicio al eliminar lista {Id}", id);
       return Result<bool>.Failure("Error al eliminar la lista");
      }
    }

    // ========== TaskItem Operations ==========

    public async Task<Result<IEnumerable<TaskItem>>> GetTaskItemsByListIdAsync(Guid taskListId)
    {
        try
 {
            var items = await _repository.GetTaskItemsByListIdAsync(taskListId);
  return Result<IEnumerable<TaskItem>>.Success(items);
        }
        catch (Exception ex)
    {
     _logger.LogError(ex, "Error en servicio al obtener tareas de lista {ListId}", taskListId);
     return Result<IEnumerable<TaskItem>>.Failure("Error al obtener las tareas");
  }
    }

    public async Task<Result<TaskItem>> GetTaskItemByIdAsync(Guid id)
    {
try
{
     var item = await _repository.GetTaskItemByIdAsync(id);
    if (item == null)
          {
         return Result<TaskItem>.Failure("Tarea no encontrada");
    }

     return Result<TaskItem>.Success(item);
        }
 catch (Exception ex)
        {
       _logger.LogError(ex, "Error en servicio al obtener tarea {Id}", id);
      return Result<TaskItem>.Failure("Error al obtener la tarea");
    }
    }

    public async Task<Result<TaskItem>> CreateTaskItemAsync(TaskItem taskItem)
    {
        try
        {
            // Validar título vacío
          if (string.IsNullOrWhiteSpace(taskItem.Title))
  {
              return Result<TaskItem>.Failure("El título de la tarea es obligatorio");
   }

    // Validar que la lista existe
 var list = await _repository.GetTaskListByIdAsync(taskItem.TaskListId);
            if (list == null)
            {
   return Result<TaskItem>.Failure("La lista especificada no existe");
 }

   var created = await _repository.CreateTaskItemAsync(taskItem);
 return Result<TaskItem>.Success(created);
        }
 catch (Exception ex)
    {
       _logger.LogError(ex, "Error en servicio al crear tarea");
            return Result<TaskItem>.Failure("Error al crear la tarea");
     }
    }

    public async Task<Result<TaskItem>> UpdateTaskItemAsync(TaskItem taskItem)
    {
   try
   {
  // Validar título vacío
         if (string.IsNullOrWhiteSpace(taskItem.Title))
      {
                return Result<TaskItem>.Failure("El título de la tarea es obligatorio");
            }

            var updated = await _repository.UpdateTaskItemAsync(taskItem);
return Result<TaskItem>.Success(updated);
 }
        catch (Exception ex)
        {
    _logger.LogError(ex, "Error en servicio al actualizar tarea {Id}", taskItem.Id);
       return Result<TaskItem>.Failure("Error al actualizar la tarea");
        }
    }

    public async Task<Result<bool>> DeleteTaskItemAsync(Guid id)
    {
     try
    {
            var result = await _repository.DeleteTaskItemAsync(id);
     if (!result)
            {
    return Result<bool>.Failure("Tarea no encontrada");
      }

          return Result<bool>.Success(true);
        }
   catch (Exception ex)
        {
       _logger.LogError(ex, "Error en servicio al eliminar tarea {Id}", id);
            return Result<bool>.Failure("Error al eliminar la tarea");
     }
    }

 public async Task<Result<TaskItem>> MarkTaskAsCompletedAsync(Guid id)
    {
        try
      {
      var result = await _repository.MarkTaskAsCompletedAsync(id);
   if (result == null)
         {
          return Result<TaskItem>.Failure("Tarea no encontrada");
            }

     return Result<TaskItem>.Success(result);
        }
        catch (Exception ex)
        {
    _logger.LogError(ex, "Error en servicio al completar tarea {Id}", id);
            return Result<TaskItem>.Failure("Error al marcar tarea como completada");
        }
    }

    public async Task<Result<TaskItem>> MoveTaskToListAsync(Guid taskId, Guid newTaskListId)
    {
   try
  {
            var result = await _repository.MoveTaskToListAsync(taskId, newTaskListId);
    if (result == null)
            {
 return Result<TaskItem>.Failure("No se pudo mover la tarea. Verifica que tanto la tarea como la lista destino existan.");
       }

          return Result<TaskItem>.Success(result);
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error en servicio al mover tarea {TaskId} a lista {ListId}", taskId, newTaskListId);
 return Result<TaskItem>.Failure("Error al mover la tarea");
      }
    }

    // ========== Query Operations ==========

    public async Task<Result<IEnumerable<TaskItem>>> SearchTasksAsync(string searchText)
    {
        try
        {
   var items = await _repository.SearchTasksAsync(searchText);
          return Result<IEnumerable<TaskItem>>.Success(items);
    }
        catch (Exception ex)
   {
     _logger.LogError(ex, "Error en servicio al buscar tareas");
            return Result<IEnumerable<TaskItem>>.Failure("Error al buscar tareas");
  }
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetTasksByStatusAsync(Guid taskListId, TaskItemStatus status)
    {
        try
        {
       var items = await _repository.GetTasksByStatusAsync(taskListId, status);
 return Result<IEnumerable<TaskItem>>.Success(items);
        }
     catch (Exception ex)
        {
  _logger.LogError(ex, "Error en servicio al obtener tareas por estado");
        return Result<IEnumerable<TaskItem>>.Failure("Error al obtener tareas por estado");
        }
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetTasksByPriorityAsync(Guid taskListId, TaskListPriority priority)
    {
        try
  {
      var items = await _repository.GetTasksByPriorityAsync(taskListId, priority);
         return Result<IEnumerable<TaskItem>>.Success(items);
     }
        catch (Exception ex)
   {
     _logger.LogError(ex, "Error en servicio al obtener tareas por prioridad");
 return Result<IEnumerable<TaskItem>>.Failure("Error al obtener tareas por prioridad");
  }
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetOverdueTasksAsync()
    {
     try
        {
            var items = await _repository.GetOverdueTasksAsync();
   return Result<IEnumerable<TaskItem>>.Success(items);
        }
        catch (Exception ex)
   {
 _logger.LogError(ex, "Error en servicio al obtener tareas vencidas");
   return Result<IEnumerable<TaskItem>>.Failure("Error al obtener tareas vencidas");
  }
    }

    public async Task<Result<Dictionary<string, int>>> GetStatisticsAsync()
    {
        try
        {
        var stats = await _repository.GetStatisticsAsync();
            return Result<Dictionary<string, int>>.Success(stats);
      }
 catch (Exception ex)
        {
        _logger.LogError(ex, "Error en servicio al obtener estadísticas");
    return Result<Dictionary<string, int>>.Failure("Error al obtener estadísticas");
  }
    }
}

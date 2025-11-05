using Microsoft.Extensions.Logging;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Business.DTOs;
using DemoWinForms.Common;
using DemoWinForms.Data.Repositories;

namespace DemoWinForms.Business.Services;

/// <summary>
/// Implementación del servicio de Tareas
/// </summary>
public class TareaService : ITareaService
{
    private readonly ITareaRepository _tareaRepository;
  private readonly ILogger<TareaService> _logger;

    public TareaService(ITareaRepository tareaRepository, ILogger<TareaService> logger)
    {
        _tareaRepository = tareaRepository;
     _logger = logger;
    }

    public async Task<Result<Tarea>> GetByIdAsync(int id)
    {
        try
{
      var tarea = await _tareaRepository.GetByIdAsync(id);
     if (tarea == null)
     return Result<Tarea>.Failure("Tarea no encontrada");

        return Result<Tarea>.Success(tarea);
        }
      catch (Exception ex)
        {
   _logger.LogError(ex, "Error al obtener tarea {TareaId}", id);
       return Result<Tarea>.Failure($"Error al obtener la tarea: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Tarea>>> GetAllAsync()
    {
        try
  {
     var tareas = await _tareaRepository.GetAllAsync();
    return Result<IEnumerable<Tarea>>.Success(tareas);
        }
        catch (Exception ex)
        {
   _logger.LogError(ex, "Error al obtener todas las tareas");
  return Result<IEnumerable<Tarea>>.Failure($"Error al obtener las tareas: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Tarea>>> GetByFiltrosAsync(FiltroTareasDto filtros)
 {
        try
        {
        var tareas = await _tareaRepository.GetByFiltrosAsync(filtros);
   return Result<IEnumerable<Tarea>>.Success(tareas);
    }
        catch (Exception ex)
    {
            _logger.LogError(ex, "Error al filtrar tareas");
      return Result<IEnumerable<Tarea>>.Failure($"Error al filtrar las tareas: {ex.Message}");
        }
    }

    public async Task<Result<int>> GetCountByFiltrosAsync(FiltroTareasDto filtros)
    {
        try
        {
            var count = await _tareaRepository.GetCountByFiltrosAsync(filtros);
         return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
_logger.LogError(ex, "Error al contar tareas");
            return Result<int>.Failure($"Error al contar las tareas: {ex.Message}");
   }
    }

    public async Task<Result<Tarea>> CrearTareaAsync(Tarea tarea)
    {
        try
        {
     // Validaciones
            var validationResult = ValidarTarea(tarea);
            if (!validationResult.IsSuccess)
      return Result<Tarea>.Failure(validationResult.Error);

            tarea.FechaCreacion = DateTime.Now;
  tarea.UltimaModificacion = DateTime.Now;

            var tareaCreada = await _tareaRepository.AddAsync(tarea);
     _logger.LogInformation("Tarea creada: {TareaId} - {Titulo}", tareaCreada.Id, tareaCreada.Titulo);

   return Result<Tarea>.Success(tareaCreada);
        }
        catch (Exception ex)
   {
 _logger.LogError(ex, "Error al crear tarea");
 return Result<Tarea>.Failure($"Error al crear la tarea: {ex.Message}");
        }
  }

    public async Task<Result> ActualizarTareaAsync(Tarea tarea)
    {
 try
        {
  // Validaciones
    var validationResult = ValidarTarea(tarea);
  if (!validationResult.IsSuccess)
        return validationResult;

            // Verificar que existe
 var tareaExistente = await _tareaRepository.GetByIdAsync(tarea.Id);
 if (tareaExistente == null)
                return Result.Failure("Tarea no encontrada");

await _tareaRepository.UpdateAsync(tarea);
       _logger.LogInformation("Tarea actualizada: {TareaId}", tarea.Id);

       return Result.Success();
        }
        catch (Exception ex)
    {
       _logger.LogError(ex, "Error al actualizar tarea {TareaId}", tarea.Id);
            return Result.Failure($"Error al actualizar la tarea: {ex.Message}");
        }
    }

    public async Task<Result> EliminarTareaAsync(int id)
    {
        try
        {
       var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea == null)
   return Result.Failure("Tarea no encontrada");

   await _tareaRepository.DeleteAsync(id);
    _logger.LogInformation("Tarea eliminada: {TareaId}", id);

 return Result.Success();
        }
        catch (Exception ex)
{
          _logger.LogError(ex, "Error al eliminar tarea {TareaId}", id);
 return Result.Failure($"Error al eliminar la tarea: {ex.Message}");
        }
    }

    public async Task<Result> MarcarComoCompletadaAsync(int id)
    {
        try
{
            var tarea = await _tareaRepository.GetByIdAsync(id);
    if (tarea == null)
   return Result.Failure("Tarea no encontrada");

tarea.Estado = EstadoTarea.Completada;
  tarea.FechaCompletado = DateTime.Now;
      tarea.UltimaModificacion = DateTime.Now;

     await _tareaRepository.UpdateAsync(tarea);
       _logger.LogInformation("Tarea marcada como completada: {TareaId}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
  _logger.LogError(ex, "Error al completar tarea {TareaId}", id);
       return Result.Failure($"Error al completar la tarea: {ex.Message}");
 }
 }

    public async Task<Result<EstadisticasDto>> GetEstadisticasAsync(int? usuarioId = null)
    {
    try
      {
  var filtros = new FiltroTareasDto
            {
  UsuarioId = usuarioId,
      TamañoPagina = 0 // Sin paginación para estadísticas
       };

     var tareas = await _tareaRepository.GetByFiltrosAsync(filtros);
       var listaTareas = tareas.ToList();

     var estadisticas = new EstadisticasDto
            {
   TotalTareas = listaTareas.Count,
      TareasPendientes = listaTareas.Count(t => t.Estado == EstadoTarea.Pendiente),
    TareasEnProgreso = listaTareas.Count(t => t.Estado == EstadoTarea.EnProgreso),
                TareasCompletadas = listaTareas.Count(t => t.Estado == EstadoTarea.Completada),
   TareasCanceladas = listaTareas.Count(t => t.Estado == EstadoTarea.Cancelada),
     TareasVencidas = listaTareas.Count(t => 
   t.FechaVencimiento.HasValue && 
          t.FechaVencimiento.Value < DateTime.Now && 
       t.Estado != EstadoTarea.Completada),
          TareasPorCategoria = listaTareas.GroupBy(t => t.Categoria)
          .ToDictionary(g => g.Key, g => g.Count()),
        TareasPorPrioridad = listaTareas.GroupBy(t => t.Prioridad.ToString())
      .ToDictionary(g => g.Key, g => g.Count())
 };

         return Result<EstadisticasDto>.Success(estadisticas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas");
   return Result<EstadisticasDto>.Failure($"Error al obtener estadísticas: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Tarea>>> GetProximasVencerAsync(int dias = 3)
  {
     try
        {
          var tareas = await _tareaRepository.GetProximasVencerAsync(dias);
            return Result<IEnumerable<Tarea>>.Success(tareas);
        }
catch (Exception ex)
        {
  _logger.LogError(ex, "Error al obtener tareas próximas a vencer");
   return Result<IEnumerable<Tarea>>.Failure($"Error: {ex.Message}");
}
    }

    private Result ValidarTarea(Tarea tarea)
    {
   if (string.IsNullOrWhiteSpace(tarea.Titulo))
return Result.Failure("El título es obligatorio");

  if (tarea.Titulo.Length > 200)
   return Result.Failure("El título no puede exceder 200 caracteres");

     if (!string.IsNullOrWhiteSpace(tarea.Descripcion) && tarea.Descripcion.Length > 2000)
            return Result.Failure("La descripción no puede exceder 2000 caracteres");

if (tarea.FechaVencimiento.HasValue && tarea.FechaVencimiento.Value < DateTime.Now.Date)
return Result.Failure("La fecha de vencimiento no puede ser anterior a hoy");

        if (tarea.UsuarioId <= 0)
  return Result.Failure("Debe asignar un usuario a la tarea");

        return Result.Success();
    }
}

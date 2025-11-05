using DemoWinForms.Domain.Entities;
using DemoWinForms.Business.DTOs;
using DemoWinForms.Common;

namespace DemoWinForms.Business.Services;

/// <summary>
/// Interfaz del servicio de Tareas
/// </summary>
public interface ITareaService
{
    Task<Result<Tarea>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Tarea>>> GetAllAsync();
    Task<Result<IEnumerable<Tarea>>> GetByFiltrosAsync(FiltroTareasDto filtros);
    Task<Result<int>> GetCountByFiltrosAsync(FiltroTareasDto filtros);
    Task<Result<Tarea>> CrearTareaAsync(Tarea tarea);
    Task<Result> ActualizarTareaAsync(Tarea tarea);
    Task<Result> EliminarTareaAsync(int id);
 Task<Result> MarcarComoCompletadaAsync(int id);
    Task<Result<EstadisticasDto>> GetEstadisticasAsync(int? usuarioId = null);
    Task<Result<IEnumerable<Tarea>>> GetProximasVencerAsync(int dias = 3);
}

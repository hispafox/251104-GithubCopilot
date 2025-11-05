using DemoWinForms.Domain.Entities;
using DemoWinForms.Business.DTOs;

namespace DemoWinForms.Data.Repositories;

/// <summary>
/// Interfaz del repositorio de Tareas
/// </summary>
public interface ITareaRepository
{
 Task<Tarea?> GetByIdAsync(int id);
    Task<IEnumerable<Tarea>> GetAllAsync();
    Task<IEnumerable<Tarea>> GetByFiltrosAsync(FiltroTareasDto filtros);
    Task<int> GetCountByFiltrosAsync(FiltroTareasDto filtros);
    Task<Tarea> AddAsync(Tarea tarea);
    Task UpdateAsync(Tarea tarea);
    Task DeleteAsync(int id);
    Task<IEnumerable<Tarea>> GetProximasVencerAsync(int dias = 3);
}

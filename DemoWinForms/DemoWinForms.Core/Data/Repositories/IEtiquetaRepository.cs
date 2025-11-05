using DemoWinForms.Domain.Entities;

namespace DemoWinForms.Data.Repositories;

/// <summary>
/// Interfaz del repositorio de Etiquetas
/// </summary>
public interface IEtiquetaRepository
{
    Task<Etiqueta?> GetByIdAsync(int id);
    Task<IEnumerable<Etiqueta>> GetAllAsync();
    Task<Etiqueta> AddAsync(Etiqueta etiqueta);
    Task UpdateAsync(Etiqueta etiqueta);
 Task DeleteAsync(int id);
}

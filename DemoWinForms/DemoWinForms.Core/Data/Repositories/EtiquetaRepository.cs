using Microsoft.EntityFrameworkCore;
using DemoWinForms.Domain.Entities;

namespace DemoWinForms.Data.Repositories;

/// <summary>
/// Implementación del repositorio de Etiquetas
/// </summary>
public class EtiquetaRepository : IEtiquetaRepository
{
    private readonly AppDbContext _context;

    public EtiquetaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Etiqueta?> GetByIdAsync(int id)
    {
     return await _context.Etiquetas.FindAsync(id);
    }

    public async Task<IEnumerable<Etiqueta>> GetAllAsync()
    {
   return await _context.Etiquetas
        .OrderBy(e => e.Nombre)
   .AsNoTracking()
         .ToListAsync();
    }

  public async Task<Etiqueta> AddAsync(Etiqueta etiqueta)
 {
        _context.Etiquetas.Add(etiqueta);
  await _context.SaveChangesAsync();
        return etiqueta;
    }

    public async Task UpdateAsync(Etiqueta etiqueta)
    {
      _context.Entry(etiqueta).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
     var etiqueta = await _context.Etiquetas.FindAsync(id);
        if (etiqueta != null)
   {
     _context.Etiquetas.Remove(etiqueta);
         await _context.SaveChangesAsync();
      }
    }
}

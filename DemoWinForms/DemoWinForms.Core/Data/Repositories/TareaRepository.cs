using Microsoft.EntityFrameworkCore;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Business.DTOs;

namespace DemoWinForms.Data.Repositories;

/// <summary>
/// Implementación del repositorio de Tareas
/// </summary>
public class TareaRepository : ITareaRepository
{
    private readonly AppDbContext _context;

    public TareaRepository(AppDbContext context)
    {
        _context = context;
    }

  public async Task<Tarea?> GetByIdAsync(int id)
    {
  return await _context.Tareas
    .Include(t => t.Usuario)
            .Include(t => t.TareaEtiquetas)
      .ThenInclude(te => te.Etiqueta)
       .Include(t => t.Subtareas)
         .FirstOrDefaultAsync(t => t.Id == id && !t.EliminadoLogico);
    }

    public async Task<IEnumerable<Tarea>> GetAllAsync()
    {
        return await _context.Tareas
            .Include(t => t.Usuario)
  .Include(t => t.TareaEtiquetas)
             .ThenInclude(te => te.Etiqueta)
 .Where(t => !t.EliminadoLogico)
          .OrderByDescending(t => t.FechaCreacion)
 .AsNoTracking()
 .ToListAsync();
    }

    public async Task<IEnumerable<Tarea>> GetByFiltrosAsync(FiltroTareasDto filtros)
    {
     var query = _context.Tareas
    .Include(t => t.Usuario)
          .Include(t => t.TareaEtiquetas)
 .ThenInclude(te => te.Etiqueta)
            .Where(t => !t.EliminadoLogico)
      .AsQueryable();

      // Aplicar filtros
        query = AplicarFiltros(query, filtros);

        // Ordenamiento
        query = filtros.OrdenarPor switch
    {
     "prioridad" => query.OrderByDescending(t => t.Prioridad).ThenBy(t => t.FechaVencimiento),
            "fecha_vencimiento" => query.OrderBy(t => t.FechaVencimiento),
            "titulo" => query.OrderBy(t => t.Titulo),
    "estado" => query.OrderBy(t => t.Estado),
       _ => query.OrderByDescending(t => t.FechaCreacion)
      };

        // Paginación
        if (filtros.Pagina > 0 && filtros.TamañoPagina > 0)
     {
        query = query
          .Skip((filtros.Pagina - 1) * filtros.TamañoPagina)
       .Take(filtros.TamañoPagina);
    }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<int> GetCountByFiltrosAsync(FiltroTareasDto filtros)
    {
   var query = _context.Tareas
          .Where(t => !t.EliminadoLogico)
            .AsQueryable();

        query = AplicarFiltros(query, filtros);

      return await query.CountAsync();
    }

    private IQueryable<Tarea> AplicarFiltros(IQueryable<Tarea> query, FiltroTareasDto filtros)
    {
      // Soporte para múltiples estados
    if (filtros.Estados != null && filtros.Estados.Any())
     query = query.Where(t => filtros.Estados.Contains(t.Estado));
        else if (filtros.Estado.HasValue)
            query = query.Where(t => t.Estado == filtros.Estado.Value);

        if (filtros.Prioridad.HasValue)
            query = query.Where(t => t.Prioridad == filtros.Prioridad.Value);

     if (!string.IsNullOrWhiteSpace(filtros.Categoria))
            query = query.Where(t => t.Categoria == filtros.Categoria);

        if (!string.IsNullOrWhiteSpace(filtros.BusquedaTexto))
   {
            var busqueda = filtros.BusquedaTexto.ToLower();
   query = query.Where(t =>
      t.Titulo.ToLower().Contains(busqueda) ||
   (t.Descripcion != null && t.Descripcion.ToLower().Contains(busqueda)));
    }

     if (filtros.FechaDesde.HasValue)
            query = query.Where(t => t.FechaCreacion >= filtros.FechaDesde.Value);

    if (filtros.FechaHasta.HasValue)
            query = query.Where(t => t.FechaCreacion <= filtros.FechaHasta.Value);

    if (filtros.UsuarioId.HasValue)
            query = query.Where(t => t.UsuarioId == filtros.UsuarioId.Value);

    return query;
    }

    public async Task<Tarea> AddAsync(Tarea tarea)
    {
        tarea.FechaCreacion = DateTime.Now;
   tarea.UltimaModificacion = DateTime.Now;

        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();

     return tarea;
    }

    public async Task UpdateAsync(Tarea tarea)
    {
     tarea.UltimaModificacion = DateTime.Now;
        _context.Entry(tarea).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
    if (tarea != null)
        {
            // Eliminación lógica
    tarea.EliminadoLogico = true;
            tarea.UltimaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
    }
    }

    public async Task<IEnumerable<Tarea>> GetProximasVencerAsync(int dias = 3)
    {
        var fechaLimite = DateTime.Now.AddDays(dias);
        return await _context.Tareas
            .Include(t => t.Usuario)
   .Where(t => !t.EliminadoLogico &&
              t.Estado != Domain.Enums.EstadoTarea.Completada &&
     t.FechaVencimiento.HasValue &&
   t.FechaVencimiento.Value <= fechaLimite &&
          t.FechaVencimiento.Value >= DateTime.Now)
      .OrderBy(t => t.FechaVencimiento)
       .AsNoTracking()
      .ToListAsync();
    }
}

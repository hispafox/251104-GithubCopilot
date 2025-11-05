using DemoWinForms.Domain.Enums;

namespace DemoWinForms.Business.DTOs;

/// <summary>
/// DTO para filtrar tareas
/// </summary>
public class FiltroTareasDto
{
    public List<EstadoTarea>? Estados { get; set; }
    public EstadoTarea? Estado { get; set; }
    public PrioridadTarea? Prioridad { get; set; }
    public string? Categoria { get; set; }
    public string? BusquedaTexto { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? UsuarioId { get; set; }
    public string OrdenarPor { get; set; } = "fecha_creacion";
    public int Pagina { get; set; } = 1;
    public int TamañoPagina { get; set; } = 50;
}

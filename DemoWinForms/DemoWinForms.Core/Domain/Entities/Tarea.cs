using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoWinForms.Domain.Enums;

namespace DemoWinForms.Domain.Entities;

/// <summary>
/// Entidad Tarea
/// </summary>
[Table("Tareas")]
public class Tarea
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

  [MaxLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? Descripcion { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? FechaVencimiento { get; set; }

    public DateTime? FechaCompletado { get; set; }

    [Required]
    public PrioridadTarea Prioridad { get; set; } = PrioridadTarea.Media;

    [Required]
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;

    [Required]
    [MaxLength(50)]
    public string Categoria { get; set; } = "Personal";

    // Relación con Usuario
    [Required]
    public int UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }

    // Subtareas (relación auto-referencial)
    public int? TareaPadreId { get; set; }

    [ForeignKey(nameof(TareaPadreId))]
    public Tarea? TareaPadre { get; set; }

    public ICollection<Tarea> Subtareas { get; set; } = new List<Tarea>();

    // Etiquetas (many-to-many)
    public ICollection<TareaEtiqueta> TareaEtiquetas { get; set; } = new List<TareaEtiqueta>();

    // Auditoría
    [Required]
    public DateTime UltimaModificacion { get; set; } = DateTime.Now;

    public bool EliminadoLogico { get; set; } = false;
}

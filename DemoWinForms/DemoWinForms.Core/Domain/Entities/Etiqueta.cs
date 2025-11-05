using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWinForms.Domain.Entities;

/// <summary>
/// Entidad Etiqueta para categorizar tareas
/// </summary>
[Table("Etiquetas")]
public class Etiqueta
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

 [MaxLength(7)]
    public string ColorHex { get; set; } = "#808080"; // Color por defecto gris

    public ICollection<TareaEtiqueta> TareaEtiquetas { get; set; } = new List<TareaEtiqueta>();
}

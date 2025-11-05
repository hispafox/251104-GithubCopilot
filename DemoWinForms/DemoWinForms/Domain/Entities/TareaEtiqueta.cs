using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWinForms.Domain.Entities;

/// <summary>
/// Tabla intermedia para relación many-to-many entre Tarea y Etiqueta
/// </summary>
[Table("TareasEtiquetas")]
public class TareaEtiqueta
{
    public int TareaId { get; set; }

    [ForeignKey(nameof(TareaId))]
    public Tarea Tarea { get; set; } = null!;

    public int EtiquetaId { get; set; }

    [ForeignKey(nameof(EtiquetaId))]
    public Etiqueta Etiqueta { get; set; } = null!;
}

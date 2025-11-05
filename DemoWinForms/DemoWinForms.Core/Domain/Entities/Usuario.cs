using System.ComponentModel.DataAnnotations;

namespace DemoWinForms.Domain.Entities;

/// <summary>
/// Entidad Usuario
/// </summary>
public class Usuario
{
    [Key]
    public int Id { get; set; }

 [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
  [Range(0, 150, ErrorMessage = "La edad debe estar entre 0 y 150")]
    public int Edad { get; set; }

    [Required(ErrorMessage = "El país es obligatorio")]
    [MaxLength(100)]
    public string Pais { get; set; } = string.Empty;

 [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

 [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone(ErrorMessage = "Formato de teléfono inválido")]
  [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace DemoConsola
{
 public class Usuario
 {
 [Key]
 public int Id { get; set; }
 public string Nombre { get; set; }
 public int Edad { get; set; }
 public string Pais { get; set; }
 public string Email { get; set; }
 public string Telefono { get; set; }
 }
}
